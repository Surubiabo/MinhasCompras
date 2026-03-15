using MinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();


    public ListaProduto()
    {
        InitializeComponent();

        lst_produto.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();
            List<Produto> temp = await App.DB.GetAll();

            temp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex) { await DisplayAlert("Ops", ex.Message, "ok"); }
    }


    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex) { DisplayAlert("Ops", ex.Message, "ok"); }

    }

    private async void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string s = e.NewTextValue;
            lista.Clear();

            List<Produto> temp = await App.DB.Search(s);
            temp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex) { await DisplayAlert("Ops", ex.Message, "ok"); }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            double soma = lista.Sum(i => i.Total);

            string tex = $"O total é {soma:C}";

            DisplayAlert("Total dos produtos é", tex, "ok");
        }
        catch (Exception ex) { DisplayAlert("Ops", ex.Message, "ok"); }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            var menuitem = sender as MenuItem;
            var item = menuitem.BindingContext as Produto;
            await App.DB.Delete(item.Id);
            lista.Remove(item);
            await DisplayAlert("Removido", "com sucesso", "ok");
        }
        catch (Exception ex) { await DisplayAlert("Ops", ex.Message, "ok"); }
    }

    private void lst_produto_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });


        }

        catch (Exception ex) { DisplayAlert("Ops", ex.Message, "ok"); }

    }
}
