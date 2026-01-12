using modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriaNegocio
    {

        public List<Categoria> listarCategoria()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos= new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Descripcion FROM Categorias");
                datos.ejecutarLectura();

                while (datos.Lector.Read()) 
                { 
                
                    Categoria aux = new Categoria();
                    aux.id = (int)datos.Lector["Id"];
                    aux.descripcion= (string)datos.Lector["Descripcion"];
                    lista.Add(aux);
                }
                return lista;


            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        
        }
    }
}
