using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();
                List<Produto> tmp = await App.Db.GetAll();
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem is Produto p)
                {
                    await Navigation.PushAsync(new EditarProduto(p));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lista.Clear();
                List<Produto> tmp = await App.Db.GetAll();
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string query = e.NewTextValue.ToLower();
                lista.Clear();
                List<Produto> tmp = await App.Db.Search(query);
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem menuItem = sender as MenuItem;
                Produto produtoSelecionado = menuItem?.BindingContext as Produto;

                if (produtoSelecionado != null)
                {
                    bool confirmacao = await DisplayAlert("Confirmar", $"Você tem certeza que deseja remover o produto {produtoSelecionado.Descricao}?", "Sim", "Não");

                    if (confirmacao)
                    {
                        await App.Db.Delete(produtoSelecionado.Id);
                        lista.Remove(produtoSelecionado);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }


        private void ToolbarItem_Somar_Clicked(object sender, EventArgs e)
        {
            double soma = lista.Sum(i => i.Total);
            DisplayAlert("Total dos Produtos", $"O total é {soma:C}", "OK");
        }

        private async void ToolbarItem_NovoProduto_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NovoProduto());
        }
    }
}