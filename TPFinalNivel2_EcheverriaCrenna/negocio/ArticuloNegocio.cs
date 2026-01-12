using modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ArticuloNegocio
    {
        

        public List<Articulo> listar()
        {
			List<Articulo> lista = new List<Articulo>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.setearConsulta("SELECT A.Id,A.Codigo, A.NOMBRE , A.Descripcion, A.IdMarca, M.Descripcion AS Marca,A.IdCategoria ,C.Descripcion AS Categoria,A.ImagenUrl ,A.Precio FROM ARTICULOS A INNER JOIN MARCAS M ON A.IdMarca = M.id INNER JOIN CATEGORIAS C ON A.IdCategoria = C.Id\r\n");
				datos.ejecutarLectura();

				while (datos.Lector.Read())
				{
					Articulo aux = new Articulo();
					aux.id = (int)datos.Lector["Id"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    
                    aux.marca = new Marca();
                    aux.marca.id = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["Marca"];

                    
                    aux.categoria = new Categoria();
                    aux.categoria.id = (int)datos.Lector["IdCategoria"];
                    aux.categoria.descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.urlImagen = (string)datos.Lector["ImagenUrl"];

                    aux.precio = (decimal)datos.Lector["Precio"];


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

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria, ImagenUrl) VALUES (@Codigo,@Nombre, @Descripcion,@Precio,@idMarca, @idCategoria, @imagenUrl)");
                datos.setearParametros("@Codigo", nuevo.codigo);
                datos.setearParametros("@Nombre", nuevo.nombre);
                datos.setearParametros("@Descripcion", nuevo.descripcion);
                datos.setearParametros("@Precio", nuevo.precio);
                datos.setearParametros("@idMarca", nuevo.marca.id);
                datos.setearParametros("@idCategoria", nuevo.categoria.id);
                datos.setearParametros("@imagenUrl", nuevo.urlImagen);
                datos.ejecutarAccion();
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
        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, Precio = @precio, IdMarca = @idmarca, IdCategoria = @idcategoria, IMagenUrl = @imagenUrl Where Id = @id");
                datos.setearParametros("@codigo", articulo.codigo);
                datos.setearParametros("@nombre", articulo.nombre);
                datos.setearParametros("@descripcion", articulo.descripcion);
                datos.setearParametros("@precio", articulo.precio);
                datos.setearParametros("@IdMarca", articulo.marca.id);
                datos.setearParametros("@IdCategoria", articulo.categoria.id);
                datos.setearParametros("@imagenUrl", articulo.urlImagen);
                datos.setearParametros("@id", articulo.id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw new Exception("Error al modificar el artículo en la base de datos.", ex);
            }
            finally
            {
                datos.cerrarConexion(); 
            }
        }

        public void eliminar(Articulo selecionado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Delete from ARTICULOS where id = @id");
                datos.setearParametros("@id", selecionado.id);
                datos.ejecutarLectura();


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

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta =
                "SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion, a.IdMarca, m.Descripcion AS Marca, " +
                "a.IdCategoria, c.Descripcion AS Categoria, a.Precio, a.ImagenUrl " +
                "FROM Articulos a " +
                "JOIN Marcas m ON a.IdMarca = m.Id " +
                "JOIN Categorias c ON a.IdCategoria = c.Id " +
                "WHERE ";

                if (campo == "Código")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.Codigo LIKE '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "a.Codigo LIKE '%" + filtro + "'";
                            break;

                        default:
                            consulta += "a.Codigo LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.Nombre LIKE '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "a.Nombre LIKE '%" + filtro + "'";
                            break;

                        default:
                            consulta += "a.Nombre LIKE '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.Descripcion LIKE '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "a.Descripcion LIKE '%" + filtro + "'";
                            break;

                        default:
                            consulta += "a.Descripcion LIKE '%" + filtro + "%'";
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.id = (int)datos.Lector["Id"];
                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.urlImagen = (string)datos.Lector["ImagenUrl"];

                    aux.precio = (decimal)datos.Lector["Precio"];

                    aux.marca = new Marca();
                    aux.marca.id = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["Marca"];

                    aux.categoria = new Categoria();
                    aux.categoria.id = (int)datos.Lector["IdCategoria"];
                    aux.categoria.descripcion = (string)datos.Lector["Categoria"];

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
