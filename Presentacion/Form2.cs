using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;

namespace Presentacion
{
    public partial class Form2 : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
         
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;           
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();


                if (!(SoloNumeros(txbPrecio.Text)) && articulo.Id == 0)
                {
                    MessageBox.Show("Debe ingresar solo numeros en el precio");
                    return;
                }
                if (string.IsNullOrEmpty(txbPrecio.Text))
                    txbPrecio.Text = "0";

                    articulo.Codigo = txbCodigo.Text;
                    articulo.Nombre = txbNombre.Text;                
                    articulo.Descripcion = txbDescripcion.Text;
                    articulo.ImagenUrl = txbUrlImagen.Text;
                    articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                    articulo.Marca = (Marca)cboMarca.SelectedItem;
                if (SoloNumeros(txbPrecio.Text))                
                    articulo.Precio = decimal.Parse(txbPrecio.Text);              


                if (articulo.Id != 0)
                {
                    if (!(SoloNumeros(txbPrecio.Text)))
                    {
                        MessageBox.Show("debe ingresar solo numeros en el precio");
                        return;
                    }
                    else
                        negocio.modificar(articulo);
                        MessageBox.Show("Modificado exitosamente");
                }
                else
                {

                    if (string.IsNullOrWhiteSpace(txbNombre.Text) || string.IsNullOrEmpty(txbCodigo.Text)) //|| !(SoloNumeros(txbPrecio.Text)))
                    {
                        MessageBox.Show("Debe ingresar un nombre , un codigo");
                        return;
                    }
                    else
                        negocio.agregar(articulo);
                        MessageBox.Show("Agregado exitosamente");
                }


                if (archivo != null && !(txbUrlImagen.Text.ToUpper().Contains("HTTP")))
                 File.Copy(archivo.FileName, ConfigurationManager.AppSettings["images-folder"] + archivo.SafeFileName);

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool SoloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if ((!(char.IsNumber(caracter)) && caracter != ','))
                    return false;
            }
            return true;
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "id";
                cboCategoria.DisplayMember = "NombreCategoria";

                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "id";
                cboMarca.DisplayMember = "NombreMarca";


                if(articulo != null )
                {
                    txbCodigo.Text = articulo.Codigo;
                    txbNombre.Text = articulo.Nombre;
                    txbDescripcion.Text = articulo.Descripcion;
                    txbUrlImagen.Text = articulo.ImagenUrl;
                    txbPrecio.Text = (articulo.Precio.ToString());
                    CargarImagen(articulo.ImagenUrl);
                    cboCategoria.SelectedValue = articulo.Categoria.id;
                    cboMarca.SelectedValue = articulo.Marca.id;

                    if (articulo.ver == true)
                    {
                        btnAceptar.Enabled = false;
                        btnImagen.Enabled = false;
                        txbCodigo.ReadOnly = true;
                        txbNombre.ReadOnly = true;
                        txbDescripcion.ReadOnly = true;
                        txbUrlImagen.ReadOnly = true;
                        txbPrecio.ReadOnly = true;
                        cboCategoria.Enabled = false;
                        cboMarca.Enabled = false;
                    }
                }            
               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
        }

        private void txbUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txbUrlImagen.Text);
        }

        private void CargarImagen( string imagen)
        {
            try
            {
                ptbImagen2.Load(imagen);

            }
            catch (Exception)
            {

                ptbImagen2.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9cSGzVkaZvJD5722MU5A-JJt_T5JMZzotcw&s");
            }
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg| png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txbUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);

            }
            
        }
    }
}
