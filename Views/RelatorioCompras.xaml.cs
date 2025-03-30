namespace MauiAppMinhasCompras.Views
{
    public partial class RelatorioCompras : ContentPage
    {
        public RelatorioCompras()
        {
            InitializeComponent();
        }

        public async void OnFiltrarClicked(object sender, EventArgs e)
        {
            DateTime dataInicio = dpInicio.Date;
            DateTime dataFim = dpFim.Date;

            if (dataFim < dataInicio)
            {
                await DisplayAlert("Erro", "Data final não pode ser menor que a data inicial.", "OK");
                return;
            }

            string categoriaFiltro = "categoriaExemplo";

            var produtosFiltrados = await App.Db.GetProdutosPorPeriodo(dataInicio, dataFim, categoriaFiltro);
            lvRelatorio.ItemsSource = produtosFiltrados;
        }
    }
}
