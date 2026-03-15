using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MinhasCompras.Models
{
    public class Produto
    {
        string _desc;
        double _quant;
        double _preco;


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {

            get => _desc;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor preencher os dados");
                }
                _desc = value;
            }

        }
        public double Quantidade
        {
            get => _quant;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Por favor preencher os dados");
                }
                _quant = value;
            }

        }


        public double Preco
        {

            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Por favor preencher os dados");
                }
                _preco = value;
            }

        }



        public double Total { get => Quantidade * Preco; }
    }
}
