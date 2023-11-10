using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PruebasJALE
{

    public partial class Form1 : Form
    {

        readonly SqlConnection conexionMS = new("Data Source=JACQUES; Initial Catalog = EjerPrac; integrated security=true");
        int depa = 0, clas = 0, fami = 0, accion = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                dateAlta.MaxDate = DateTime.Today;
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                conexionMS.Open();
                MessageBox.Show("Conexión Correcta");
                conexionMS.Close();
            }
            catch
            {
                MessageBox.Show("Error al conectar");
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            conexionMS.Open();
            if ((txtArticulo.Text != "") && (txtMarca.Text != ""))
            {
                try
                {
                    SqlCommand query = new SqlCommand("sp_InsertInto", conexionMS);
                    query.CommandType = CommandType.StoredProcedure;

                    query.Parameters.AddWithValue("spNombre", txtArticulo.Text);
                    query.Parameters.AddWithValue("spApellido", txtMarca.Text);

                    query.ExecuteNonQuery();
                    MessageBox.Show("Agregado Correctamente");
                }
                catch
                {
                    MessageBox.Show("No se pudo agregar");
                }
            }

            conexionMS.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            conexionMS.Open();
            if ((txtSKU.Text != "") && (txtArticulo.Text != "") && (txtMarca.Text != ""))
            {
                try
                {
                    SqlCommand query = new SqlCommand("sp_Update", conexionMS);
                    query.CommandType = CommandType.StoredProcedure;

                    query.Parameters.AddWithValue("spID", txtSKU.Text);
                    query.Parameters.AddWithValue("spNombre", txtArticulo.Text);
                    query.Parameters.AddWithValue("spApellido", txtMarca.Text);


                    query.ExecuteNonQuery();
                    MessageBox.Show("Agregado Correctamente");
                }
                catch
                {
                    MessageBox.Show("No se pudo agregar");
                }
            }

            conexionMS.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            conexionMS.Open();
            if ((txtSKU.Text != ""))
            {
                try
                {
                    SqlCommand query = new("delete from persona WHERE personaID ='" + txtSKU.Text + "'", conexionMS);
                    query.ExecuteNonQuery();
                    MessageBox.Show("Borrado Correctamente");
                }
                catch
                {
                    MessageBox.Show("No se pudo borrar");
                }
            }
            conexionMS.Close();
        }

        private void cmbDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbClase.Items.Clear();
            cmbClase.SelectedIndex = -1;
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;

            conexionMS.Open();
            try
            {
                SqlCommand query = new SqlCommand("exec sp_LlenaClas '" + (cmbDepartamento.SelectedIndex + 1) + "'", conexionMS);
                SqlDataReader datos = query.ExecuteReader();

                while (datos.Read())
                {
                    cmbClase.Items.Add(datos.GetValue(1).ToString());
                }
            }
            catch
            {
                MessageBox.Show("No se pudo cargar");
            }
            conexionMS.Close();
        }

        private void txtSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void cmbClase_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;

            conexionMS.Open();
            try
            {
                SqlCommand query = new SqlCommand("exec sp_LlenaFami '" + (cmbDepartamento.SelectedIndex + 1) + "', '"+ (cmbClase.SelectedIndex + 1) + "'", conexionMS);
                SqlDataReader datos = query.ExecuteReader();

                while (datos.Read())
                {
                    cmbFamilia.Items.Add(datos.GetValue(1).ToString());
                }
            }
            catch
            {
                MessageBox.Show("No se pudo cargar");
            }
            conexionMS.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtSKU.Enabled = true;
            btnBuscar.Enabled = true;
            txtArticulo.Enabled = false;
            txtMarca.Enabled = false;
            txtModelo.Enabled = false;
            cmbDepartamento.Enabled = false;
            cmbClase.Enabled = false;
            cmbFamilia.Enabled = false;
            txtStock.Enabled = false;
            txtCantidad.Enabled = false;
            btnModificar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            txtSKU.Text = "";
            txtArticulo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            cmbDepartamento.Items.Clear();
            cmbDepartamento.SelectedIndex = -1;
            cmbClase.Items.Clear();
            cmbClase.SelectedIndex = -1;
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;
            txtStock.Text = "";
            txtCantidad.Text = "";
            chkDescontinuado.Checked = false;
            dateAlta.Value = DateTime.Today;
            dateBaja.Value = new DateTime(1900, 01, 01);
        }

        private void txtStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            txtSKU.Enabled              = false;
            btnBuscar.Enabled           = false;
            txtArticulo.Enabled         = true;
            txtMarca.Enabled            = true;
            txtModelo.Enabled           = true;
            cmbDepartamento.Enabled     = true;
            cmbClase.Enabled            = true;
            cmbFamilia.Enabled          = true;
            txtStock.Enabled            = true;
            txtCantidad.Enabled         = true;
            chkDescontinuado.Enabled    = true;
            btnModificar.Enabled        = false;
            btnBorrar.Enabled           = false;
            btnGuardar.Enabled          = true;
            btnCancelar.Enabled         = true;

            accion = 2;    
        }

        private void chkDescontinuado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDescontinuado.Checked)
            {
                dateBaja.Value = DateTime.Today;
            }
            else
            {
                dateBaja.Value = new DateTime(1900, 01, 01);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
             DialogResult dr = MessageBox.Show("¿Seguro que desea eliminar el artículo?", "Borrar artículo",MessageBoxButtons.YesNo);

            switch (dr)
            {
                case DialogResult.Yes:
                        conexionMS.Open();
                        SqlCommand query = new SqlCommand("exec sp_Baja '" + (txtSKU.Text) + "'", conexionMS);
                        query.ExecuteNonQuery();
                        MessageBox.Show("Artículo eliminado correctamente.");
                        conexionMS.Close();
                    break;
                case DialogResult.No:

                    break;
            }

            txtSKU.Enabled = true;
            btnBuscar.Enabled = true;
            txtArticulo.Enabled = false;
            txtMarca.Enabled = false;
            txtModelo.Enabled = false;
            cmbDepartamento.Enabled = false;
            cmbClase.Enabled = false;
            cmbFamilia.Enabled = false;
            txtStock.Enabled = false;
            txtCantidad.Enabled = false;
            chkDescontinuado.Enabled = false;
            btnModificar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            txtSKU.Text = "";
            txtArticulo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            cmbDepartamento.Items.Clear();
            cmbDepartamento.SelectedIndex = -1;
            cmbClase.Items.Clear();
            cmbClase.SelectedIndex = -1;
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;
            txtStock.Text = "";
            txtCantidad.Text = "";
            chkDescontinuado.Checked = false;
            dateAlta.Value = DateTime.Today;
            dateBaja.Value = new DateTime(1900, 01, 01);

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (int.Parse(txtStock.Text) < int.Parse(txtCantidad.Text))
            {
                MessageBox.Show("La cantadidad del artículo no puede ser mayor al Stock");
                return;
            }
            switch (accion)
            {
                case 1: //Alta

                    conexionMS.Open();
                    SqlCommand query = new SqlCommand("exec sp_Alta '" + (txtSKU.Text) + "', '" 
                                                                       + (txtArticulo.Text) + "', '" 
                                                                       + (txtMarca.Text) + "', '"
                                                                       + (txtModelo.Text) + "', '"
                                                                       + (cmbDepartamento.SelectedIndex + 1) + "', '"
                                                                       + (cmbClase.SelectedIndex + 1) + "', '"
                                                                       + (cmbFamilia.SelectedIndex + 1) + "', '"
                                                                       + (dateAlta.Value.ToShortDateString()) + "', '"
                                                                       + (txtStock.Text) + "', '"
                                                                       + (txtCantidad.Text) + "', '"
                                                                       + (chkDescontinuado.Checked ? 1:0 ) + "','"
                                                                       + (dateBaja.Value.ToShortDateString()) + "'", conexionMS);
                    query.ExecuteNonQuery();
                    MessageBox.Show("Artículo dado de alta correctamente.");
                    conexionMS.Close();

                    break;
                case 2: //Cambio

                    conexionMS.Open();
                    SqlCommand query3 = new SqlCommand("exec sp_Cambio '" + (txtSKU.Text) + "', '"
                                                                       + (txtArticulo.Text) + "', '"
                                                                       + (txtMarca.Text) + "', '"
                                                                       + (txtModelo.Text) + "', '"
                                                                       + (cmbDepartamento.SelectedIndex + 1) + "', '"
                                                                       + (cmbClase.SelectedIndex + 1) + "', '"
                                                                       + (cmbFamilia.SelectedIndex + 1) + "', '"
                                                                       + (txtStock.Text) + "', '"
                                                                       + (txtCantidad.Text) + "', '"
                                                                       + (chkDescontinuado.Checked ? 1 : 0) + "','"
                                                                       + (dateBaja.Value.ToShortDateString()) + "'", conexionMS);
                    query3.ExecuteNonQuery();
                    MessageBox.Show("Artículo actualizado correctamente.");
                    conexionMS.Close();
                    break;
            }

            txtSKU.Enabled = true;
            btnBuscar.Enabled = true;
            txtArticulo.Enabled = false;
            txtMarca.Enabled = false;
            txtModelo.Enabled = false;
            cmbDepartamento.Enabled = false;
            cmbClase.Enabled = false;
            cmbFamilia.Enabled = false;
            txtStock.Enabled = false;
            txtCantidad.Enabled = false;
            chkDescontinuado.Enabled = false;
            btnModificar.Enabled = false;
            btnBorrar.Enabled = false;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;

            txtSKU.Text = "";
            txtArticulo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            cmbDepartamento.Items.Clear();
            cmbDepartamento.SelectedIndex = -1;
            cmbClase.Items.Clear();
            cmbClase.SelectedIndex = -1;
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;
            txtStock.Text = "";
            txtCantidad.Text = "";
            chkDescontinuado.Checked = false;
            dateAlta.Value = DateTime.Today;
            dateBaja.Value = new DateTime(1900, 01, 01);
        }

        private void llenaDepartamento()
        {
            cmbDepartamento.Items.Clear();
            cmbDepartamento.SelectedIndex = -1;
            cmbClase.Items.Clear();
            cmbClase.SelectedIndex = -1;
            cmbFamilia.Items.Clear();
            cmbFamilia.SelectedIndex = -1;
            conexionMS.Open();
            try
            {
                SqlCommand query = new SqlCommand("exec sp_LlenaDepa", conexionMS);
                SqlDataReader datos = query.ExecuteReader();

                while (datos.Read())
                {
                    cmbDepartamento.Items.Add(datos.GetValue(1).ToString());
                    //cmbDepartamento.SelectedIndex = 0;
                }
            }
            catch
            {
                MessageBox.Show("No se pudo cargar");
            }
            conexionMS.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtSKU.Text!="") {
                this.llenaDepartamento();
                conexionMS.Open();
                try
                {
                    SqlCommand query = new SqlCommand("exec sp_BuscaArticulo '" + txtSKU.Text+ "'" , conexionMS);
                    SqlDataReader datos = query.ExecuteReader();

                    if (datos.HasRows)
                    {
                        while (datos.Read())
                        {
                            txtArticulo.Text = datos.GetValue(1).ToString();
                            txtMarca.Text = datos.GetValue(2).ToString();
                            txtModelo.Text = datos.GetValue(3).ToString();
                            depa = (int)datos.GetValue(4);
                            clas = (int)datos.GetValue(5);
                            fami = (int)datos.GetValue(6);
                            dateAlta.Value = (DateTime)datos.GetValue(7);
                            txtStock.Text = datos.GetValue(8).ToString();
                            txtCantidad.Text = datos.GetValue(9).ToString();
                            chkDescontinuado.Checked = ((int)datos.GetValue(10) == 1);
                            dateBaja.Value = (DateTime)datos.GetValue(11);
                        }
                        conexionMS.Close();
                        
                        cmbDepartamento.SelectedIndex = depa - 1;
                        cmbClase.SelectedIndex = clas - 1;
                        cmbFamilia.SelectedIndex = fami - 1;
                        txtSKU.Enabled              = false;
                        btnBuscar.Enabled           = false;
                        btnModificar.Enabled        = true;
                        btnBorrar.Enabled           = true;
                        btnCancelar.Enabled         = true;

                    }
                    else
                    {
                        conexionMS.Close();
                        txtSKU.Enabled              = false;
                        btnBuscar.Enabled           = false;
                        txtArticulo.Enabled         = true;
                        txtMarca.Enabled            = true;
                        txtModelo.Enabled           = true;
                        cmbDepartamento.Enabled     = true;
                        cmbClase.Enabled            = true;
                        cmbFamilia.Enabled          = true;
                        //dateAlta.Enabled            = true;
                        txtStock.Enabled            = true;
                        txtCantidad.Enabled         = true;
                        //chkDescontinuado.Enabled    = true;
                        //dateBaja.Enabled            = true;
                        btnGuardar.Enabled          = true;
                        btnCancelar.Enabled         = true;

                        dateAlta.Value = DateTime.Today;
                        dateBaja.Value = new DateTime(1900, 01, 01);
                        chkDescontinuado.Checked = false;
                        accion = 1;

                    }
                }
                catch
                {
                    MessageBox.Show("No se pudo cargar");
                    conexionMS.Close();
                }
            }
            else
            {
                MessageBox.Show("Por favor, introduzca un valor númerico de 6 digitos.");
            }
        }
    }
}