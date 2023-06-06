using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bus_Ticketing
{
    public partial class Tickets : Form
    {
        Passenger ticketDetail;
        List<Passenger> allTickets = new List<Passenger>();

        public Tickets()
        {
            InitializeComponent();
        }

        //CUSTOM CONTROL BAR BELOW
        private Point lastMousePosition;
        private bool isDragging;

        private void controlbar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMousePosition = e.Location;
                isDragging = true;
            }
        }

        private void controlbar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void controlbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Get the form that contains the control
                Form parentForm = FindForm();

                // Calculate the offset from the last mouse position
                int offsetX = e.Location.X - lastMousePosition.X;
                int offsetY = e.Location.Y - lastMousePosition.Y;

                // Move the parent form
                parentForm.Location = new Point(parentForm.Location.X + offsetX, parentForm.Location.Y + offsetY);
            }
        }

        private void handCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void defaultCursor(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.home);
            FormManager.CloseForm(FormManager.tickets);
            FormManager.tickets = new Tickets();
        }

        private void bookButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.book);
            FormManager.CloseForm(FormManager.tickets);
            FormManager.tickets = new Tickets();
        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            User.fullname = "";
            User.id = 0;
            FormManager.ShowForm(FormManager.login);
            FormManager.CloseForm(FormManager.tickets);
            FormManager.tickets = new Tickets();
        }

        private void Tickets_Load(object sender, EventArgs e)
        {
            fullnameText.Text = User.fullname;
            SetUpcomingTrips();
        }

        private void exitProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SetUpcomingTrips()
        {
            Get4ClosestTrips();

            upcomingTitleLabel.Text = "No upcoming trips";
            upcomingControlNumber.Hide();
            UpcomingDate.Hide();

            subPanel1.Hide();
            subPanel2.Hide();
            subPanel3.Hide();

            subDetail1.Hide();
            subDetail2.Hide();
            subDetail3.Hide();

            if (allTickets.Count() < 1)
            {
                return;
            }

            upcomingTitleLabel.Text = "Upcoming trip";
            upcomingControlNumber.Text = allTickets[0].CtrlNumber;
            UpcomingDate.Text = allTickets[0].Date;

            upcomingControlNumber.Show();
            UpcomingDate.Show();

            if (allTickets.Count() < 2)
            {
                return;
            }

            subControl1.Text = allTickets[1].CtrlNumber;
            subDate1.Text = allTickets[1].Date;

            subPanel1.Show();
            subDetail1.Show();

            if (allTickets.Count() < 3)
            {
                return;
            }

            subControl2.Text = allTickets[2].CtrlNumber;
            subDate2.Text = allTickets[2].Date;

            subPanel2.Show();
            subDetail2.Show();

            if (allTickets.Count() < 4)
            {
                return;
            }

            subControl3.Text = allTickets[3].CtrlNumber;
            subDate3.Text = allTickets[3].Date;

            subPanel3.Show();
            subDetail3.Show();
        }

        private void Get4ClosestTrips()
        {
            try
            {
                string myConnection = "datasource=127.0.0.1; port=3306; username=root; database=bus_data; password=";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand SelectCommand = new MySqlCommand($"SELECT * FROM bus_data.transaction_info WHERE date >= CURDATE() AND userid={User.id} AND paid=1 ORDER BY date ASC LIMIT 4;", myConn);
                MySqlDataReader myReader;
                myConn.Open();
                myReader = SelectCommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        ticketDetail = new Passenger();

                        ticketDetail.CtrlNumber = myReader.GetString("ctrlNumber");
                        ticketDetail.Date = myReader.GetMySqlDateTime("date").ToString();

                        allTickets.Add(ticketDetail);

                    }
                }

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
