using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;
using System.Globalization;

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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            produtosOriginais = await App.Db.GetAll();
            AtualizarListaETotal(); // Chama o método centralizado para preencher a lista
        }

        // Método central que aplica o filtro de texto, o filtro de categoria e calcula o total
        private void AtualizarListaETotal()
        {
            string termoBusca = searchBarProdutos.Text?.ToLower() ?? "";
            string categoriaSelecionada = (string)pck_filtroCategoria.SelectedItem ?? "Todas as Categorias";

            produtosFiltrados.Clear();

            // Inicia com todos os produtos
            var query = produtosOriginais.AsEnumerable();

            // 1. Aplica o filtro de texto (SearchBar)
            if (!string.IsNullOrEmpty(termoBusca))
            {
                query = query.Where(p => p.Descricao.ToLower().Contains(termoBusca));
            }

            // 2. Aplica o filtro de categoria (Picker)
            if (categoriaSelecionada != "Todas as Categorias" && !string.IsNullOrEmpty(categoriaSelecionada))
            {
                query = query.Where(p => p.Categoria == categoriaSelecionada);
            }

            var listaFinal = query.ToList();
            double valorTotal = 0;

            // Preenche a tela e calcula o total gasto (Quantidade * Preço)
            foreach (var p in listaFinal)
            {
                produtosFiltrados.Add(p);
                valorTotal += (p.Preco * p.Quantidade);
            }

            // Agenda 6: Atualiza o texto do Label de total com a formatação de moeda regionalizada
            lbl_totalCategoria.Text = string.Format(new CultureInfo("pt-BR"), "Total Gasto: {0:C}", valorTotal);
        }

        // Evento disparado quando o usuário digita na busca
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            AtualizarListaETotal();
        }

        // Evento disparado quando o usuário muda a categoria no Picker
        private void pck_filtroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListaETotal();
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem mi = (MenuItem)sender;
                Produto p = (Produto)mi.CommandParameter;

                bool confirmacao = await DisplayAlert("Tem Certeza?", $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não");

                if (confirmacao)
                {
                    await App.Db.Delete(p.Id);
                    await DisplayAlert("Sucesso", "Produto excluído com sucesso!", "OK");
                    OnAppearing();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void listaProdutos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;

                Produto p = (Produto)e.SelectedItem;
                await Navigation.PushAsync(new EditarProduto { BindingContext = p });
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}