using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio ,M.Descripcion Marca , C.Descripcion Categoria, A.IdCategoria, A.IdMarca From ARTICULOS A , MARCAS M , CATEGORIAS C where M.id = A.IdMarca  AND A.IdCategoria =  C.Id";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)lector["ImagenUrl"];

                    aux.Marca = new Marca();
                    aux.Marca.id = (int)lector["IdMarca"];
                    aux.Marca.NombreMarca = (string)lector["Marca"];                    
                    aux.Categoria = new Categoria();
                    aux.Categoria.id = (int)lector["IdCategoria"];
                    aux.Categoria.NombreCategoria = (string)lector["Categoria"];
                    if (!(lector["Precio"] is DBNull))
                        aux.Precio = (decimal)lector["Precio"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void agregar (Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {                                
                datos.SetearCosulta("insert into ARTICULOS(Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values( @Codigo, @Nombre, @Descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                datos.setearParametro("@Codigo", nuevo.Codigo);
                datos.setearParametro("@Nombre", nuevo.Nombre);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Marca.id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.id);
                datos.setearParametro("@imagenUrl", nuevo.ImagenUrl);
                datos.setearParametro("@precio", nuevo.Precio);
                datos.EjecutarAccion();                
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

        }

        public void modificar(Articulo modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearCosulta("update ARTICULOS set Codigo = @cod, Nombre = @nom, Descripcion = @des, IdMarca = @idmarca, IdCategoria = @idcategoria, ImagenUrl = @img, Precio = @prec where Id = @id");
                datos.setearParametro("@cod", modificar.Codigo);
                datos.setearParametro("@nom", modificar.Nombre);
                datos.setearParametro("@des", modificar.Descripcion);
                datos.setearParametro("@idmarca", modificar.Marca.id);
                datos.setearParametro("@idcategoria", modificar.Categoria.id);
                datos.setearParametro("@img", modificar.ImagenUrl);
                datos.setearParametro("@prec", modificar.Precio);
                datos.setearParametro("@id", modificar.Id);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearCosulta("delete from ARTICULOS where Id = @id");
                datos.setearParametro("@id", id);
                datos.EjecutarAccion();


            }
            catch ( Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio ,M.Descripcion Marca , C.Descripcion Categoria, A.IdCategoria, A.IdMarca From ARTICULOS A , MARCAS M , CATEGORIAS C where M.id = A.IdMarca  AND A.IdCategoria =  C.Id AND ";

                if (campo == "Categoria")
                {
                    consulta += "C.Descripcion = '" + criterio + "' ";
                    consulta += "AND Nombre like '" + filtro + "%'";
                }
                else
                {
                    consulta += "M.Descripcion = '"+ criterio +"' ";
                    consulta += "AND Nombre like '" + filtro + "%'";
                }

                datos.SetearCosulta(consulta);
                datos.EjecutarConsulta();

                datos.CargarLista(lista);

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
