using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> produtosFiltrados = new ObservableCollection<Produto>();
        List<Produto> produtosOriginais = new List<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
            listaProdutos.ItemsSource = produtosFiltrados;
        }

        // Carrega os dados do SQLite sempre que o ecrã for aberto
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            produtosOriginais = await App.Db.GetAll();

            produtosFiltrados.Clear();
            foreach (var p in produtosOriginais)
            {
                produtosFiltrados.Add(p);
            }
        }

        // Filtra os dados dinamicamente conforme o texto é alterado
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string termoBusca = e.NewTextValue?.ToLower() ?? "";

            produtosFiltrados.Clear();

            var produtosEncontrados = produtosOriginais.Where(p => p.Descricao.ToLower().Contains(termoBusca)).ToList();

            foreach (var p in produtosEncontrados)
            {
                produtosFiltrados.Add(p);
            }
        }

        // Agenda 5: Método acionado quando o usuário arrasta o item e clica em "Remover"
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Descobre qual item foi clicado
                MenuItem mi = (MenuItem)sender;
                Produto p = (Produto)mi.CommandParameter;

                // Alerta de confirmação usando DisplayAlert
                bool confirmacao = await DisplayAlert("Tem Certeza?", $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não");

                // Se o usuário clicar em "Sim"
                if (confirmacao)
                {
                    // Deleta do banco de dados
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso", "Produto excluído com sucesso!", "OK");

                    // Recarrega a lista para o produto sumir da tela
                    OnAppearing();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        // Agenda 5: Método acionado quando o usuário dá um toque (clique) em cima do produto na lista
        private async void listaProdutos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                // Se não tiver nada selecionado, não faz nada
                if (e.SelectedItem == null)
                    return;

                // Pega o produto que foi clicado
                Produto p = (Produto)e.SelectedItem;

                // Abre a tela de edição (que vamos criar no Passo 3) e envia o produto para ela
                await Navigation.PushAsync(new EditarProduto { BindingContext = p });

                // Tira a marcação visual de "selecionado" do item
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}