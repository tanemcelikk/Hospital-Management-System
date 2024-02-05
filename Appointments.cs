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
namespace _21118080036_Project
{
    public partial class Appointments : Form
    {
        private string patientId;
        public Appointments(string patientId)
        {
            InitializeComponent();
            this.patientId = patientId;
            Load += Appointments_Load;
        }

        private void Appointments_Load(object sender, EventArgs e)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE patient_personal_id=@p1 ORDER BY appointment_date", connect);
                    cmd.Parameters.AddWithValue("@p1", patientId);
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
        private void CancelAppointment(string appointmentId)
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                SqlCommand checkDateCmd = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE appointment_id = @p1 AND appointment_date > GETDATE()", connect);
                checkDateCmd.Parameters.AddWithValue("@p1", appointmentId);

                SqlCommand checkTimeCmd = new SqlCommand("SELECT COUNT(*) FROM Appointments WHERE appointment_id = @p1 AND appointment_date = CAST(GETDATE() AS DATE) AND CAST(appointment_time AS TIME) >= CAST(GETDATE() AS TIME)", connect);
                checkTimeCmd.Parameters.AddWithValue("@p1", appointmentId);

                int futureAppointmentsCount = (int)checkDateCmd.ExecuteScalar();
                int futureAppointmentsTimeCount = (int)checkTimeCmd.ExecuteScalar();
                if (futureAppointmentsCount > 0 || futureAppointmentsTimeCount > 0)
                {
                    SqlCommand updateCmd = new SqlCommand("UPDATE Appointments SET appointment_situation = 0, patient_personal_id = NULL, patient_complaint = NULL WHERE appointment_id = @p1", connect);
                    updateCmd.Parameters.AddWithValue("@p1", appointmentId);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    MessageBox.Show("Past appointments or appointments with past times cannot be canceled.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void RefreshAppointments()
        {
            using (SqlConnection connect = new SQLConnection().connection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments where patient_personal_id=@p1", connect);
                cmd.Parameters.AddWithValue("@p1", patientId);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dataGridView2.DataSource = dt;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
                string appointmentId = selectedRow.Cells["appointment_id"].Value.ToString();

                DialogResult result = MessageBox.Show("Do you want to cancel the appointment?", "Appointment Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    CancelAppointment(appointmentId);
                    RefreshAppointments();
                }
            }
            else
            {
                MessageBox.Show("Please select the appointment you want to cancel.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}
