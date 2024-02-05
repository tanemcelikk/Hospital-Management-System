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
using Project;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace _21118080036_Project
{
    public partial class DoctorHomePage : Form
    {
        public DoctorHomePage()
        {
            InitializeComponent();
            Load += DoctorHomePage_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int choosen = dataGridView1.SelectedCells[0].RowIndex;
            richTextBox1.Text = dataGridView1.Rows[choosen].Cells[7].Value.ToString();
        }


        private void DoctorHomePage_Load(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                string doctorName = label2.Text;

                SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE appointment_doctor = @doctorName", connect);
                cmd.Parameters.AddWithValue("@doctorName", doctorName);

                SqlDataReader dr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dr);

                dataGridView1.DataSource = dt;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show(); 
            //Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Announcements fr = new Announcements();
            fr.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DoctorUpdate fr = new DoctorUpdate(label5.Text.ToString());
            fr.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
