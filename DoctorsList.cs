using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Project;

namespace _21118080036_Project
{
    public partial class DoctorsList : Form
    {
        public DoctorsList()
        {
            InitializeComponent();
            Load += DoctorsList_Load;
            dataGridView2.CellClick += dataGridView2_CellClick;
        }

        private void DoctorsList_Load(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {        

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Doctors ORDER BY Doctor_ID", connect);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }

            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int choosen = dataGridView2.SelectedCells[0].RowIndex;
            MaskedName.Text = dataGridView2.Rows[choosen].Cells[1].Value.ToString();
            Maskedbranch.Text = dataGridView2.Rows[choosen].Cells[2].Value.ToString();
            maskedTextBox1.Text = dataGridView2.Rows[choosen].Cells[3].Value.ToString();
            MaskedPassword.Text = dataGridView2.Rows[choosen].Cells[4].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                if (MaskedName.Text == "" || maskedTextBox1.Text == "" || Maskedbranch.Text == "" || MaskedPassword.Text == "")
                {
                    MessageBox.Show("Fill in the blanks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MaskedName.Focus();
                    return;
                }
                Regex Personalid = new Regex("^[1-9]{1}[0-9]{9}[02468]{1}$");
                if (!Personalid.IsMatch(maskedTextBox1.Text))
                {
                    MessageBox.Show("Invalid Personal id", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox1.Focus();
                    return;
                }
                Regex Branch = new Regex("^[A-ZIÜÖÇĞŞ][a-zA-ZIÜÖÇĞŞıüöçğş]*$");
                if (!Branch.IsMatch(Maskedbranch.Text))
                {
                    MessageBox.Show("Invalid Branch ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Maskedbranch.Focus();
                    return;
                }
                try
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO Doctors(Doctor_Name,Doctor_Branch,Doctor_Personalid,Doctor_Password) VALUES (@p1,@p2,@p3,@p4)", connect);
                    cmd2.Parameters.AddWithValue("@p1", MaskedName.Text);
                    cmd2.Parameters.AddWithValue("@p2", Maskedbranch.Text);
                    cmd2.Parameters.AddWithValue("@p3", maskedTextBox1.Text);
                    cmd2.Parameters.AddWithValue("@p4", MaskedPassword.Text);
                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Registration is successful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Doctors", connect);
                    SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                    DataTable refreshDt = new DataTable();
                    refreshDt.Load(refreshReader);
                    dataGridView2.DataSource = refreshDt;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Check if the error is due to PK violation
                    {
                        MessageBox.Show("The doctor is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (ex.Number == 547) // 547, foreign key violation hatasıdır
                    {
                        MessageBox.Show("Please select a valid branch.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Doctors WHERE Doctor_Personalid=@p1", connect);
                    cmd.Parameters.AddWithValue("@p1", maskedTextBox1.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Deletion is successful", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                   
                    SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Doctors", connect);
                    SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                    DataTable refreshDt = new DataTable();
                    refreshDt.Load(refreshReader);
                    dataGridView2.DataSource = refreshDt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                if (MaskedName.Text == "" || maskedTextBox1.Text == "" || Maskedbranch.Text == "" || MaskedPassword.Text == "")
                {
                    MessageBox.Show("Fill in the blanks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MaskedName.Focus();
                    return;
                }
                Regex Personalid = new Regex("^[1-9]{1}[0-9]{9}[02468]{1}$");
                if (!Personalid.IsMatch(maskedTextBox1.Text))
                {
                    MessageBox.Show("Invalid Personal id", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox1.Focus();
                    return;
                }
                Regex Branch = new Regex("^[A-ZIÜÖÇĞŞ][a-zA-ZIÜÖÇĞŞıüöçğş]*$");
                if (!Branch.IsMatch(Maskedbranch.Text))
                {
                    MessageBox.Show("Invalid Branch ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Maskedbranch.Focus();
                    return;
                }
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Doctors SET Doctor_Name=@p1,Doctor_Branch=@p2,Doctor_Password=@p4 WHERE Doctor_Personalid=@p3", connect);
                    cmd.Parameters.AddWithValue("@p1", MaskedName.Text);
                    cmd.Parameters.AddWithValue("@p2", Maskedbranch.Text);
                    cmd.Parameters.AddWithValue("@p3", maskedTextBox1.Text);
                    cmd.Parameters.AddWithValue("@p4", MaskedPassword.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Update is successful", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Doctors", connect);
                    SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                    DataTable refreshDt = new DataTable();
                    refreshDt.Load(refreshReader);
                    dataGridView2.DataSource = refreshDt;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Check if the error is due to PK violation
                    {
                        MessageBox.Show("The doctor is already registered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (ex.Number == 547) // 547, foreign key violation hatasıdır
                    {
                        MessageBox.Show("Please select a valid branch.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
              }
           }
    }
}
