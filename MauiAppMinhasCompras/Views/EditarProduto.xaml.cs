using MauiAppMinhasCompras.Models;
using System;
using Microsoft.Maui.Controls;

namespace MauiAppMinhasCompras.Views
{
    public partial class EditarProduto : ContentPage
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Recupera os dados do produto que foi passado para a tela
                Produto produto_anexado = BindingContext as Produto;

                // Cria um novo objeto com os dados atualizados das caixas de texto
                Produto p = new Produto
                {
                    Id = produto_anexado.Id,
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text)
                };

                // Executa a atualização no banco de dados
                await App.Db.Update(p);

                // Exibe o alerta de confirmação
                await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

                // Retorna para a tela anterior (Listagem)
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Caso ocorra algum erro, o catch captura e exibe na tela sem fechar o aplicativo
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}