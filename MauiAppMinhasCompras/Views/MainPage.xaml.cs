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
                // Cria um novo objeto Produto com os dados digitados na tela
                Produto p = new Produto
                {
                    Descricao = txtDescricao.Text,
                    Quantidade = Convert.ToDouble(txtQuantidade.Text),
                    Preco = Convert.ToDouble(txtPreco.Text)
                };

                // Chama o método Insert que criamos no Helper usando a conexão do App.xaml.cs
                await App.Db.Insert(p);

                // Mostra um aviso de sucesso na tela
                await DisplayAlert("Sucesso", "Produto inserido com sucesso!", "OK");

                // Limpa os campos para o próximo cadastro
                txtDescricao.Text = "";
                txtQuantidade.Text = "";
                txtPreco.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        // Nova integração: Método para abrir a tela de listagem
        private async void VerLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListagemPage());
        }
    }
}