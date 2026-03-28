using MinhasCompras.Models;
using System.Threading.Tasks;

namespace MinhasCompras.Views;

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
            Produto prod_anex = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = prod_anex.Id,
                Descricao = txt_desc.Text,
                Quantidade = Convert.ToDouble(txt_Quant.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = txt_cat.Text,
            };
            await App.DB.Update(p);
            await DisplayAlert("Produto editado", "sucesso", "ok");
            await Navigation.PopAsync();
        }

        catch (Exception ex) {await DisplayAlert("Ops", ex.Message, "ok"); }
    }
}