using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using negocio;
using dominio;


namespace Presentacion
{
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulo;
        public Form1()
        {
            InitializeComponent();            
        }

        private void btnVerdatos_Click(object sender, EventArgs e)
        {
            Form2 ventana = new Form2();           
            ventana.ShowDialog();              
                        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Cargar();         

            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Marca");
        }

        private void dgvMostrar_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMostrar.CurrentRow != null)
            {
                Articulo ArticuloSeleccionado = (Articulo)dgvMostrar.CurrentRow.DataBoundItem;
                CargarImagen(ArticuloSeleccionado.ImagenUrl);
            }
        }

        private void Cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvMostrar.DataSource = listaArticulo;                            
                CargarImagen(listaArticulo[0].ImagenUrl);
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas ()
        {
            dgvMostrar.Columns["ImagenUrl"].Visible = false;
            dgvMostrar.Columns["Id"].Visible = false;
            dgvMostrar.Columns["Ver"].Visible = false;
            dgvMostrar.Columns["Categoria"].Visible = false;
        }

        private void CargarImagen( string imagen)
        {
            try
            {
                ptbImagen.Load(imagen);

            }
            catch (Exception)
            {

                ptbImagen.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9cSGzVkaZvJD5722MU5A-JJt_T5JMZzotcw&s");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Form2 alta = new Form2();
            alta.ShowDialog();
            Cargar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            if (dgvMostrar.CurrentRow != null)
            {
                seleccionado = (Articulo)dgvMostrar.CurrentRow.DataBoundItem;

                Form2 modificar = new Form2(seleccionado);
                modificar.Text = "Modificar";
                modificar.ShowDialog();
                Cargar();
            }
            else
                MessageBox.Show("no hay articulo seleccionado");            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado = new Articulo();
            
            try                
            {
                if (dgvMostrar.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("De verdad quiere eliminar el Articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Articulo)dgvMostrar.CurrentRow.DataBoundItem;
                        negocio.eliminar(seleccionado.Id);
                        Cargar();
                    }
                }
                else
                    MessageBox.Show("Tiene que seleccionar un articulo para eliminar");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (cboCampo.SelectedItem == null)
                {
                    MessageBox.Show("Tiene que ingresar un campo");
                }
                else
                {
                    string campo = cboCampo.SelectedItem.ToString();
                    string criterio = cboCriterio.SelectedItem.ToString();
                    string filtro = txtBusquedaAvan.Text;
                    dgvMostrar.DataSource = negocio.filtrar(campo, criterio, filtro);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txbBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listafiltrada;
            string buscar = txbBuscar.Text;

            if (buscar != "")
            {
                listafiltrada = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(buscar.ToUpper()) || x.Marca.NombreMarca.ToUpper().Contains(buscar.ToUpper()));
            }
            else
            {
                listafiltrada = listaArticulo;
            }

            dgvMostrar.DataSource = null;
            dgvMostrar.DataSource = listafiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            CategoriaNegocio categoria = new CategoriaNegocio();
            MarcaNegocio marca = new MarcaNegocio();

            if (opcion == "Categoria")
            {
                cboCriterio.DataSource = categoria.listar();
            }
            else
            {
                cboCriterio.DataSource = marca.listar();
            }
        }

        private void txtBusquedaAvan_TextChanged(object sender, EventArgs e)
        {
            string buscar = txtBusquedaAvan.Text;
            if (buscar== "")
            {
                dgvMostrar.DataSource = null;
                dgvMostrar.DataSource = listaArticulo;
                ocultarColumnas();
            }
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            if (dgvMostrar.CurrentRow != null)
            {
                seleccionado = (Articulo)dgvMostrar.CurrentRow.DataBoundItem;
                seleccionado.ver = true;
                Form2 ver = new Form2(seleccionado);
                ver.Text = "Ver";
                ver.ShowDialog();
                Cargar();
            }
            else
                MessageBox.Show("no hay articulo seleccionado");
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            Cargar();
        }
    }
}
