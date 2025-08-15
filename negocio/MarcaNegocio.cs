using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listar()
        {
			List<Marca>lista = new List<Marca>();
			AccesoDatos datos = new AccesoDatos();
			try
			{
				datos.SetearCosulta("select id , Descripcion from MARCAS");
				datos.EjecutarConsulta();

				while(datos.Lector.Read())
				{
					Marca aux = new Marca();
					aux.id = (int)datos.Lector["id"];
					aux.NombreMarca = (string)datos.Lector["Descripcion"];

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
				datos.Lector.Close();
			}
        }
    }
}
