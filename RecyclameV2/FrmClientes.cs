﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using RecyclameV2.Clases;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace RecyclameV2
{
    public partial class FrmClientes : MetroForm
    {
        public FrmClientes()
        {
            InitializeComponent();
            metroTabControl1.SelectedIndex = 0;
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            lblAlta.Text =  DateTime.Now.ToString("dd/MM/yyyy");
            txtNombres.Focus();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {
            int i = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombres.Text == "" || txtApellidoP.Text == "" || txtRfc.Text == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(this, "Por favor de llenar los campos requeridos", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Cliente cliente = ObtenerDatosCliente();
                    cliente.Grabar();
                    DevExpress.XtraEditors.XtraMessageBox.Show(this, "El cliente ha sido ingresado correctamente.", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpiar();

                }
            }
            catch (Exception ex) {

                    DevExpress.XtraEditors.XtraMessageBox.Show(this, string.Format("Error al Intentar guardar el cliente. Detalle:{0}", ex.Message),
                    this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Cliente ObtenerDatosCliente() {
            Cliente cliente = new Cliente();
            cliente.Nombre = txtNombres.Text;
            cliente.ApellidoPaterno = txtApellidoP.Text;
            cliente.ApellidoMaterno = txtApellidoM.Text;
            cliente.RFC = txtRfc.Text;
            cliente.Razon_Social = txtRazonSocial.Text;
            cliente.Email = txtEmail.Text;
            long telefono = 0;
            long.TryParse(txtTelefono.Text.Replace(" ", "").Replace("+", "").Replace("-", "").Replace("(", "").Replace(")", "").Trim(), out telefono);
            cliente.Telefono = telefono;
            cliente.Calle = txtCalle.Text;
            cliente.NumExt = txtNumExt.Text;
            cliente.NumInt = txtNumInt.Text;
            cliente.Colonia = txtColonia.Text;
            cliente.Codigo_Postal = txtCodigoPostal.Text;
            cliente.Localidad = txtLocalidad.Text;
            cliente.Ciudad = txtCiudad.Text;
            cliente.Estado = txtEstado.Text;
            cliente.Pais = txtPais.Text;
            cliente.Comentario = txtComentarios.Text;
            cliente.Activo = true;
            cliente.FechaAlta = Convert.ToDateTime(lblAlta.Text);




            return cliente;


        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        public void Limpiar() {
            txtNombres.Text = "";
            txtApellidoP.Text = "";
            txtApellidoM.Text = "";
            txtRfc.Text = "";
            txtRazonSocial.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            txtCalle.Text = "";
            txtNumExt.Text = "";
            txtNumInt.Text = "";
            txtColonia.Text = "";
            txtCodigoPostal.Text = "";
            txtLocalidad.Text = "";
            txtCiudad.Text = "";
            txtEstado.Text = "";
            txtPais.Text = "";
            txtComentarios.Text = "";


        }

        private void txtCodigoPostal_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Para obligar a que sólo se introduzcan números 
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
              if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            buscarClientes(txtBuscar.Text);
        }

        private void buscarClientes(string search)
        {
           // try
           // {
                gridClientes.DataSource = null;
                List<SqlParameter> parametros = new List<SqlParameter>();
                if (txtBuscar.Text.Length > 0)
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@P_Cliente_ID", Value = 0 });
                    parametros.Add(new SqlParameter() { ParameterName = "@P_RFC", Value = txtBuscar.Text });
                    parametros.Add(new SqlParameter() { ParameterName = "@P_Nombre", Value = txtBuscar.Text });
                }
                else
                {
                    parametros.Add(new SqlParameter() { ParameterName = "@P_Cliente_ID", Value = 0 });
                    parametros.Add(new SqlParameter() { ParameterName = "@P_RFC", Value = null });
                    parametros.Add(new SqlParameter() { ParameterName = "@P_Nombre", Value = string.Empty });
                }
                    gridClientes.DataSource = Global.CargarListaGrid(BaseDatos.ejecutarProcedimientoConsultaDataTable("Cliente_Consultar_sp", parametros), "cliente");
                    gridView1.BestFitColumns();
                    List<string> listColumnasOcultar = new List<string>() { "Cliente_Id", "TipoClase", "CampoId", "CampoBusqueda", "FechaAlta", "FechaNacimiento", "Calle", "ApellidoPaterno", "ApellidoMaterno", "NumExt", "NumInt", "Estado", "Pais", "Colonia", "Ciudad", "Localidad", "Codigo_Postal", "Activo", "Telefono2", "Telefono3", "Email2", "Email3", "Comentario", "Dias_de_Credito" };
                    Herramientas.GridViewEditarColumnas(gridView1, true, true, false, listColumnasOcultar, new List<string>(), new List<string>());
               
           // }
            /*
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("BUSCAR Clientes EXCEPTION: " + e.ToString());
            }*/
        }

        private void txtPais_Click(object sender, EventArgs e)
        {

        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTab = metroTabControl1.SelectedTab.Text;
            switch (strTab)
            {
                case "Listado Clientes":
                    buscarClientes(txtBuscar.Text);
                    break;
                default:
                    break;
            }
        }

        private void btnEditarCliente_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount > 0)
            {
                int rowHandle = gridView1.GetSelectedRows()[0];
                editarCliente(rowHandle);
            }
        }
        private bool editarCliente(int rowHandle)
        {
            Cliente cliente = (Cliente)gridView1.GetRow(rowHandle);
            if (cliente != null)
            {
                if (cliente.Activo)
                {
                    frmCliente fcliente = new frmCliente(cliente);
                    if (fcliente.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        cliente = fcliente.obtenerCliente();
                        gridView1.RefreshRow(rowHandle);
                        return true;
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(this, "No se puede editar el cliente porque ha sido dado de baja.\r\nfavor de seleccionar un cliente que esté activo.", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            return false;
        }

        private void btnEiminarCliente_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount > 0)
            {
                int rowHandle = gridView1.GetSelectedRows()[0];
                eliminarCliente(rowHandle);
            }
        }
        private bool eliminarCliente(int rowHandle)
        {
            Cliente cliente = (Cliente)gridView1.GetRow(rowHandle);
            if (cliente != null)
            {
                if (cliente.Activo)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(this, "¿Estás seguro de eliminar al cliente " + cliente.Nombre + "?", this.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (cliente.Borrar())
                        {
                            cliente.Activo = false;
                            cliente.Status = "Eliminado";
                            //gridView1.DeleteRow(rowHandle);
                            gridView1.RefreshData();
                            return true;
                        }
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(this, "El cliente " + cliente.Nombre + " ya ha sido dado de baja.", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return false;
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gridView1.SelectedRowsCount > 0)
                {
                    int rowHandle = gridView1.GetSelectedRows()[0];
                    editarCliente(rowHandle);
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (gridView1.SelectedRowsCount > 0)
                {
                    int rowHandle = gridView1.GetSelectedRows()[0];
                    eliminarCliente(rowHandle);
                }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int index = Global.RowIndexClicked(gridView1);
            if (index > -1)
            {
                editarCliente(index);
            }
        }
    }
}
