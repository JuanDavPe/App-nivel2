using dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos ()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            comando = new SqlCommand();
        }

        public void SetearCosulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void EjecutarConsulta()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void setearParametro(string nombre , object valor)
        {
            comando.Parameters.AddWithValue(nombre , valor);
        }

        public void CerrarConexion()
        {
            if (lector != null)
                lector.Close();

            conexion.Close();
        }

        public void CargarLista(List<Articulo>lista)
        {
            
            while (Lector.Read())
            {
                Articulo aux = new Articulo();
                aux.Id = (int)Lector["Id"];
                aux.Codigo = (string)Lector["Codigo"];
                aux.Nombre = (string)Lector["Nombre"];
                aux.Descripcion = (string)Lector["Descripcion"];

                if (!(Lector["ImagenUrl"] is DBNull))
                    aux.ImagenUrl = (string)Lector["ImagenUrl"];

                aux.Marca = new Marca();
                aux.Marca.id = (int)Lector["IdCategoria"];
                aux.Marca.NombreMarca = (string)Lector["Marca"];
                aux.Categoria = new Categoria();
                aux.Categoria.id = (int)Lector["IdMarca"];
                aux.Categoria.NombreCategoria = (string)Lector["Categoria"];
                if (!(Lector["Precio"] is DBNull))
                    aux.Precio = (decimal)Lector["Precio"];

                lista.Add(aux);
            }
        }
    }
}
