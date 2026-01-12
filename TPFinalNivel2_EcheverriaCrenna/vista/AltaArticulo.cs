using modelo;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vista
{
    public partial class frmAltaArticulo : Form
    {

        private Articulo articulo = null;

        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo seleccionado)
        {
            InitializeComponent();
            this.articulo = seleccionado;
            Text = "Modifica tu artículo";
        }

        private void lblCodigo_Click(object sender, EventArgs e)
        {

        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio cate = new CategoriaNegocio();
            MarcaNegocio mar = new MarcaNegocio();

            try
            {

                cboxMarca.DataSource = mar.listarMarcas();
                cboxMarca.ValueMember = "id";
                cboxMarca.DisplayMember = "descripcion";
                cboxCategoria.DataSource= cate.listarCategoria();
                cboxCategoria.ValueMember = "id";
                cboxCategoria.DisplayMember= "descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.codigo;
                    txtNombre.Text = articulo.nombre;
                    txtUrl.Text = articulo.urlImagen;
                    cargarImagen(articulo.urlImagen);
                    txtDescripcion.Text = articulo.descripcion;
                    txtPrecio.Text = articulo.precio.ToString();
                    cboxMarca.SelectedValue = articulo.marca.id;
                    cboxCategoria.SelectedValue = articulo.categoria.id;
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        private void cargarImagen(string urlImagen)
        {
            try
            {
                pbxImagen.Load(urlImagen);

            }
            catch (Exception)
            {
                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio= new ArticuloNegocio();

            try
            {
                if (articulo == null) 
                {
                   articulo = new Articulo();
                }

                articulo.codigo = txtCodigo.Text;
                articulo.nombre = txtNombre.Text;  
                articulo.descripcion = txtDescripcion.Text;
                articulo.urlImagen = txtUrl.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cboxMarca.SelectedItem;
                articulo.categoria = (Categoria)cboxCategoria.SelectedItem;

                if (articulo.id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }



                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrl.Text);
        }
    }
}
