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
    public partial class frmListado : Form
    {
        private List<Articulo> listaArticulo;

        public frmListado()
        {
    
            InitializeComponent();
        }

        private void frmListado_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCampo.Items.Add("Código");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Descripción");
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulo = negocio.listar();
                dgvListado.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].urlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void ocultarColumnas()
        {
            dgvListado.Columns["UrlImagen"].Visible = false;
            dgvListado.Columns["Id"].Visible = false;
        }

        private void dgvListado_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvListado.CurrentRow != null)
            {
                Articulo selecionado = (Articulo)dgvListado.CurrentRow.DataBoundItem;
                cargarImagen(selecionado.urlImagen);
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvListado.CurrentRow == null)
            {
                MessageBox.Show("No hay artículo seleccionado para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Articulo seleccionado;
            seleccionado = (Articulo)dgvListado.CurrentRow.DataBoundItem;
            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvListado.CurrentRow == null)
            {
                MessageBox.Show("No hay artículo seleccionado para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo selecionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Deseas eliminar este artículo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    selecionado = (Articulo)dgvListado.CurrentRow.DataBoundItem;
                    negocio.eliminar(selecionado);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltroRapido.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulo.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.categoria.descripcion.ToUpper().Contains(filtro.ToUpper()) || x.marca.descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulo;
            }

            dgvListado.DataSource = null;
            dgvListado.DataSource = listaFiltrada;
            ocultarColumnas();


        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarFiltro())
                {
                    return;
                }
                string campo = cbxCampo.Text;
                string criterio = cbxCriterio.Text;
                string filtro = txtFiltro.Text;
                
                dgvListado.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar los articulos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private bool validarFiltro()
        {
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el campo a filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (cbxCriterio.SelectedIndex < 0 && !string.IsNullOrWhiteSpace(txtFiltro.Text.Trim()))
            {
                MessageBox.Show("Seleccione el criterio a filtrar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            if (cbxCampo.SelectedItem.ToString() == "Código")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacio para el campo 'Código'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else if (cbxCampo.SelectedItem.ToString() == "Nombre")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacio para el campo 'Nombre'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            else if (cbxCampo.SelectedItem.ToString() == "Descripción")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text.Trim()))
                {
                    MessageBox.Show("El filtro no puede estar vacio para el campo 'Descripción'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            return false;
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
            cbxCriterio.Items.Clear();

            if (opcion == "Código" || opcion == "Nombre" || opcion == "Descripción")
            {
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cargar();
        }
    }
}
