using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto(Produto produto)
    {
        InitializeComponent();
        BindingContext = produto;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produtoAnexado = BindingContext as Produto;

            if (produtoAnexado == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            produtoAnexado.Descricao = txt_descricao.Text;
            produtoAnexado.Quantidade = Convert.ToDouble(txt_quantidade.Text);
            produtoAnexado.Preco = Convert.ToDouble(txt_preco.Text);

            await App.Db.Update(produtoAnexado);

            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
