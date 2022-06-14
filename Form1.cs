using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CodigoPostal
{
    public partial class Form1 : Form
    {
        SqlConnection conexion = new SqlConnection("server=LAPTOP-9MQLMGGI; database=CodigoPostal; integrated security=true");
        public Form1()
        {
            InitializeComponent();
            MostrarEstado();
        }

        public void MostrarEstado() 
        {
            try
            {
                conexion.Open();
                SqlCommand Mostrar =new SqlCommand("select Estado from Direcciones group by Estado order by Estado asc",conexion);
                SqlDataAdapter Mer = new SqlDataAdapter(Mostrar);
                DataTable dt = new DataTable();
                Mer.Fill(dt);
                conexion.Close();

                DataRow fila = dt.NewRow();
                fila["Estado"] = "----Seleccionar Estado----";
                dt.Rows.InsertAt(fila, 0);

                cmbEstado.ValueMember = "Estado";
                cmbEstado.DisplayMember = "Estado";
                cmbEstado.DataSource = dt;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "");
            }
            conexion.Close();
        }

        public void Cargar_Datos(string dato)
        {
            try
            {
                conexion.Open();
                SqlCommand mostrar2 = new SqlCommand("select Municipio from Direcciones where Estado = '" + dato + "' group by Municipio order by Municipio asc", conexion);
                mostrar2.Parameters.AddWithValue("Estado", dato);
                SqlDataAdapter dr2 = new SqlDataAdapter(mostrar2);
                DataTable dt2 = new DataTable();
                dr2.Fill(dt2);
                conexion.Close();

                DataRow fila2 = dt2.NewRow();
                fila2["Municipio"] = "----Seleccione Municipio----";
                dt2.Rows.InsertAt(fila2, 0);
                cmbMunicipio.ValueMember = "Municipio";
                cmbMunicipio.DisplayMember = "Municipio";
                cmbMunicipio.DataSource = dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("El error es: " + ex.Message + "");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbEstado.SelectedValue.ToString() != null)
            {
                string dato = cmbEstado.SelectedValue.ToString();
                Cargar_Datos(dato);
            }
        }

        private void cmbMunicipio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                try 
                {
                    conexion.Open();
                    SqlCommand municipio = new SqlCommand("exec EM '" + cmbEstado.Text + "','" + cmbMunicipio.Text + "'",conexion);
                    SqlDataAdapter leer = new SqlDataAdapter(municipio);
                    DataSet ds = new DataSet();
                    leer.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    conexion.Close();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Error: " + ex.Message + "");
                    conexion.Close();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                try
                {
                    string estado = "----Seleccionar Estado----";
                    string municipio = "----Seleccione Municipio----";
                    if (estado != cmbEstado.Text && municipio != cmbMunicipio.Text && textBox1.Text == textBox1.Text)
                    {
                        conexion.Open();
                        SqlCommand asentamiento = new SqlCommand("exec EMA '" + cmbEstado.Text + "', '" + cmbMunicipio.Text + "', '%" + textBox1.Text + "%'", conexion);
                        SqlDataAdapter leer = new SqlDataAdapter(asentamiento);
                        DataSet ds = new DataSet();
                        leer.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        conexion.Close();
                    }
                    else
                    {
                        conexion.Open();
                        SqlCommand asentamiento = new SqlCommand("exec Asentamiento '%" + textBox1.Text + "%'", conexion);
                        SqlDataAdapter leer = new SqlDataAdapter(asentamiento);
                        DataSet ds = new DataSet();
                        leer.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        conexion.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message + "");
                }
                conexion.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                conexion.Open();
                SqlCommand CP = new SqlCommand("exec CP '" + textBox2.Text + "'", conexion);
                SqlDataAdapter leer = new SqlDataAdapter(CP);
                DataSet ds = new DataSet();
                leer.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "");
                conexion.Close();
            }
        }
    }
}
