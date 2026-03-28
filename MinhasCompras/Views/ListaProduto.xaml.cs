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

            
            List<Produto> temp = await App.DB.GetAll();
            var filtro = temp.Where( p => p.Descricao.Contains(s) ||
                                    p.Categoria.Contains(s)).ToList();
            
            filtro.ForEach(i => lista.Add(i));
        }
        catch (Exception ex) { await DisplayAlert("Ops", ex.Message, "ok"); }
        finally 
        { 
            lst_produto.IsRefreshing = true;
        }
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

    private async void  lst_produto_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();
            List<Produto> temp = await App.DB.GetAll();

            temp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex) { await DisplayAlert("Ops", ex.Message, "ok"); }
        finally 
        {
            lst_produto.IsRefreshing = false;    
        };
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            var todosProdutos = await App.DB.GetAll();

            if (!todosProdutos.Any())
            {
                await DisplayAlert("Aviso", "Nenhum produto cadastrado.", "OK");
                return;
            }

            // Agrupamento por Categoria
            var relatorio = todosProdutos
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = string.IsNullOrWhiteSpace(g.Key) ? "Sem Categoria" : g.Key,
                    SomaForncada = g.Sum(p => p.Total)
                })
                .OrderByDescending(x => x.SomaForncada);

            // Montar a string para exibição
            string mensagem = "Gastos por Categoria:\n\n";
            foreach (var item in relatorio)
            {
                mensagem += $"{item.Categoria}: {item.SomaForncada:C}\n";
            }

            await DisplayAlert("Resumo Financeiro", mensagem, "Fechar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}

