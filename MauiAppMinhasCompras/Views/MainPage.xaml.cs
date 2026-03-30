using MauiAppMinhasCompras.Models;
using System;
using Microsoft.Maui.Controls;

namespace MauiAppMinhasCompras.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o usuário selecionou uma categoria no Picker
                string categoriaSelecionada = (string)pck_categoria.SelectedItem;

                if (string.IsNullOrEmpty(categoriaSelecionada))
                {
                    await DisplayAlert("Aviso", "Por favor, selecione uma categoria.", "OK");
                    return; // Para a execução se não tiver categoria
                }

                // Cria o produto incluindo a Categoria
                Produto p = new Produto
                {
                    Descricao = txtDescricao.Text,
                    Quantidade = Convert.ToDouble(txtQuantidade.Text),
                    Preco = Convert.ToDouble(txtPreco.Text),
                    Categoria = categoriaSelecionada
                };

                // Salva no banco de dados
                await App.Db.Insert(p);

                await DisplayAlert("Sucesso", "Produto inserido com sucesso!", "OK");

                // Limpa os campos para o próximo cadastro
                txtDescricao.Text = "";
                txtQuantidade.Text = "";
                txtPreco.Text = "";
                pck_categoria.SelectedIndex = -1; // Volta a categoria para o estado vazio
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void VerLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaProduto());
        }
    }
}