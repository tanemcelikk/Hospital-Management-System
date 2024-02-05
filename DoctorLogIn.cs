using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _21118080036_Project
{
    public partial class DoctorLogIn : Form
    {
        public DoctorLogIn()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLConnection sqlConnection = new SQLConnection();
            SqlConnection connect = sqlConnection.connection();
            SqlCommand command = new SqlCommand();
            command.Connection = connect;
            command.CommandText = "SELECT * FROM Doctors WHERE Doctor_Personalid='" + maskedTextBox1.Text.ToString() + "'And Doctor_Password='" + maskedTextBox2.Text.ToString() + "'";
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                DoctorHomePage fr = new DoctorHomePage();
                fr.Show();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Doctors WHERE Doctor_Personalid=@p1", connect);
                cmd.Parameters.AddWithValue("@p1", maskedTextBox1.Text.ToString());
                fr.label1.Text = dr["Doctor_Name"].ToString().ToString();
                fr.label5.Text = dr["Doctor_Personalid"].ToString();
                this.Hide();
                connect.Close();

                SQLConnection sqlConnection1 = new SQLConnection();
                SqlConnection connect1 = sqlConnection1.connection();
                SqlCommand command1 = new SqlCommand();
                command1.Connection = connect1;
                command1.CommandText = "SELECT * FROM Appointments WHERE appointment_doctor='" + fr.label1.Text + "'";
                SqlDataReader reader = command1.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                fr.dataGridView1.DataSource = dt;
                connect1.Close();
            }
            else
            {
                MessageBox.Show("Invalid Personal ID or Password !");
            }
        }
    }
}
