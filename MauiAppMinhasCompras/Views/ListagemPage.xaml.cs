using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListagemPage : ContentPage
    {
        ObservableCollection<Produto> produtosFiltrados = new ObservableCollection<Produto>();
        List<Produto> produtosOriginais = new List<Produto>();

        public ListagemPage()
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
    }
}