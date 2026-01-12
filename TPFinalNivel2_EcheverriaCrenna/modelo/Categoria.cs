using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo
{
    public class Categoria
    {
        public int id { get; set; }
        public string descripcion {  get; set; }

        public override string ToString()
        {
            return descripcion;
        }
    }
}
