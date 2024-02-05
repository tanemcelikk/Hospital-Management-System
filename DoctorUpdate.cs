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
    public partial class DoctorUpdate : Form
    {
        public string Doctorid;
        public DoctorUpdate(string Doctorid)
        {
            InitializeComponent();
            this.Doctorid = Doctorid;
            SetDoctorData();
        }

        private void SetDoctorData()
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Doctors WHERE Doctor_Personalid = @p1", connect))
                {
                    command.Parameters.AddWithValue("@p1", Doctorid);

                    using (SqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            MaskedName.Text = dr["Doctor_Name"].ToString();
                            Maskedid.Text = dr["Doctor_Personalid"].ToString();
                            MaskedPassword.Text = dr["Doctor_Password"].ToString();
                            MaskedBranch.Text = dr["Doctor_Branch"].ToString();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MaskedName.Text == "" || Maskedid.Text == "" || MaskedBranch.Text == "" || MaskedPassword.Text == "")
            {
                MessageBox.Show("Fill in the blanks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MaskedName.Focus();
                return;
            }
            Regex Personalid = new Regex("^[1-9]{1}[0-9]{9}[02468]{1}$");
            if (!Personalid.IsMatch(Maskedid.Text))
            {
                MessageBox.Show("Invalid Personal id", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Maskedid.Focus();
                return;
            }
            Regex Branch = new Regex("^[A-ZIÜÖÇĞŞ][a-zA-ZIÜÖÇĞŞıüöçğş]*$");
            if (!Branch.IsMatch(MaskedBranch.Text))
            {
                MessageBox.Show("Invalid Branch ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MaskedBranch.Focus();
                return;
            }
            using (SqlConnection connect = new SQLConnection().connection())
            {
                    using (SqlConnection connect1 = new SQLConnection().connection())
                    {
                        SqlCommand command = new SqlCommand("UPDATE Doctors SET Doctor_Name=@p1,Doctor_Branch=@p2, Doctor_Password=@p3 WHERE Doctor_Personalid=@p4", connect1);
                        command.Parameters.AddWithValue("@p1", MaskedName.Text);
                        command.Parameters.AddWithValue("@p2", MaskedBranch.Text);
                        command.Parameters.AddWithValue("@p3", MaskedPassword.Text);
                        command.Parameters.AddWithValue("@p4", Maskedid.Text);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Your information has been updated successfully");
                    }
            }
        }
    }
}
