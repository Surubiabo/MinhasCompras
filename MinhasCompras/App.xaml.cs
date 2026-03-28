using MinhasCompras.Helpers;

namespace MinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabasehelper _db;

        public static SQLiteDatabasehelper DB
        {
            get 
            { 
                if(_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        , "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabasehelper(path);
                }
                    

                return _db;
            }
        }




        public App()
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");

            MainPage = new NavigationPage(new Views.ListaProduto());
            //MainPage = new AppShell();
        }
    }
}
