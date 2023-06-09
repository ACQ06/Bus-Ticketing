﻿using MySql.Data.MySqlClient;
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
    public partial class Home : Form
    {
        
        public Home()
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

        public void displayUpcomingTrip()
        {
            try
            {
                string myConnection = "datasource=127.0.0.1; port=3306; username=root; database=bus_data; password=";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand SelectCommand = new MySqlCommand($"SELECT * FROM bus_data.transaction_info WHERE date >= CURDATE() AND userid={User.id} AND paid=1 ORDER BY date ASC LIMIT 1;", myConn);
                MySqlDataReader myReader;
                myConn.Open();
                myReader = SelectCommand.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        upcomingControlNumber.Text = myReader.GetString("ctrlNumber");
                        upcomingDate.Text = myReader.GetMySqlDateTime("date").ToString();
                    }
                }

                else
                {
                    UpcomingTitle.Text = "No upcoming trips";
                    upcomingControlNumber.Hide();
                    upcomingDate.Hide();
                }

                myConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void exitProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ticketsButton_Click(object sender, EventArgs e)
        {
            FormManager.CloseForm(FormManager.home);
            FormManager.home = new Home();

            FormManager.ShowForm(FormManager.tickets);
            
        }

        private void bookButton_Click(object sender, EventArgs e)
        {
            FormManager.CloseForm(FormManager.home);
            FormManager.home = new Home();

            FormManager.ShowForm(FormManager.book);
        }
        private void Home_Load(object sender, EventArgs e)
        {
            fullnameText.Text = User.fullname;
            displayUpcomingTrip();
        }
        private void signOutButton_Click(object sender, EventArgs e)
        {
            User.fullname = "";
            User.id = 0;

            FormManager.CloseForm(FormManager.home);
            FormManager.home = new Home();

            FormManager.ShowForm(FormManager.login);
            
        }
    }
}