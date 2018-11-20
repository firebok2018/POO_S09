using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConec_Click(object sender, EventArgs e)
        {
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            con.Open();
            using (SqlTransaction tr = con.BeginTransaction(IsolationLevel.Serializable))
            {
                SqlCommand cmd = new SqlCommand("usp_addpais", con,tr);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idpais", txtCodigo.Text);
                cmd.Parameters.AddWithValue("@nombrepais", txtNombre.Text);
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    tr.Commit();
                    MessageBox.Show(" pais agregasdo " + i);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex + "");
                    tr.Rollback();
                   
                }


            }
            con.Close();
            dgpais.DataSource = null;
            dgpais.DataSource = listaso();
        }
        public DataTable listaso()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("usp_pais", con);
            da.Fill(dt);
            return dt;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                using (SqlTransaction tr = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    SqlCommand cmd = new SqlCommand("usp_updatepais", con,tr);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idpais", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@nombrepais", txtNombre.Text);
                    cmd.ExecuteNonQuery();
                    try
                    {
                        tr.Commit();
                        MessageBox.Show(" pais MOdificado ");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                        tr.Rollback();
                    }
                }   
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            dgpais.DataSource = null;
            dgpais.DataSource = listaso();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                using (SqlTransaction tr = con.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    SqlCommand cmd = new SqlCommand("usp_deletepais", con, tr);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idpais", txtCodigo.Text);
                 
                    cmd.ExecuteNonQuery();
                    try
                    {
                        tr.Commit();
                        MessageBox.Show(" pais Eliminado ");
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.ToString());
                        tr.Rollback();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            dgpais.DataSource = null;
            dgpais.DataSource = listaso();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgpais.DataSource = listaso();
        }
    }
   
}
