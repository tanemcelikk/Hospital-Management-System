using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project;

namespace _21118080036_Project
{
    public partial class BranchesList : Form
    {
        public BranchesList()
        {
            InitializeComponent();
            Load += BranchesList_Load;
            dataGridView2.CellClick += dataGridView2_CellClick;
        }
        private void BranchesList_Load(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Branches", connect);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dataGridView2.DataSource = dt;

                    // Enable text wrapping for each column
                    foreach (DataGridViewColumn column in dataGridView2.Columns)
                    {
                        if (column is DataGridViewTextBoxColumn)
                        {
                            ((DataGridViewTextBoxColumn)column).DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        }
                    }

                    // Set the AutoSizeColumnsMode property to fill
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            Maskedid.Text = dataGridView2.Rows[choosen].Cells[0].Value.ToString();
            MaskedName.Text = dataGridView2.Rows[choosen].Cells[1].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {
                    if (MaskedName.Text == "")
                    {
                        MessageBox.Show("Fill in the blanks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MaskedName.Focus();
                        return;
                    }
                    Regex Ad = new Regex(@"^[A-ZIÜÖÇĞŞ][a-zA-ZIÜÖÇĞŞiüöçğş]*$ || ^[A-ZIÜÖÇĞŞ]\s[a-zA-ZIÜÖÇĞŞiüöçğş]*$");
                    if (!Ad.IsMatch(MaskedName.Text))
                    {
                        MessageBox.Show("Invalid Branch Name", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MaskedName.Focus();
                        return;
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO Branches (branch_name) VALUES (@b1)", connect);
                        cmd.Parameters.AddWithValue("@b1", MaskedName.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Registration is successful", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Branches", connect);
                        SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                        DataTable refreshDt = new DataTable();
                        refreshDt.Load(refreshReader);
                        dataGridView2.DataSource = refreshDt;
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627) // Unique constraint violation error numbers
                    {
                        MessageBox.Show("Branch name already exists. Please choose a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Error during registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM Branches WHERE branch_id=@p1", connect);
                    cmd.Parameters.AddWithValue("@p1", Maskedid.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Deletion is successful", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Branches", connect);
                    SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                    DataTable refreshDt = new DataTable();
                    refreshDt.Load(refreshReader);
                    dataGridView2.DataSource = refreshDt;
                }
                catch (Exception ex)
                {
                    {
                        MessageBox.Show($"Error during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Branches SET branch_name=@p1 WHERE branch_id=@p2", connect);
                    cmd.Parameters.AddWithValue("@p1", MaskedName.Text);
                    cmd.Parameters.AddWithValue("@p2", Maskedid.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Update is successful", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    SqlCommand refreshCmd = new SqlCommand("SELECT * FROM Branches", connect);
                    SqlDataReader refreshReader = refreshCmd.ExecuteReader();
                    DataTable refreshDt = new DataTable();
                    refreshDt.Load(refreshReader);
                    dataGridView2.DataSource = refreshDt;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2601 || ex.Number == 2627) // Unique constraint violation error numbers
                    {
                        MessageBox.Show("Branch name already exists. Please choose a different name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Error during update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        } 
    }
    }

