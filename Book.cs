﻿using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Bus_Ticketing
{
    public partial class Book : Form
    {
        Passenger passenger = new Passenger();

        List<Passenger> allPassenger = new List<Passenger>();

        bool hasClass;

        public Book()
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

        private void payButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<Passenger>> transaction = new Dictionary<string, List<Passenger>>()
            {
                { Passenger.controlNumber, allPassenger }
            };


            foreach (KeyValuePair<string, List<Passenger>> kvp in transaction)
            {
                string key = kvp.Key;
                List<Passenger> passengers = kvp.Value;

                foreach (Passenger passenger in passengers)
                {
                    MessageBox.Show(passenger.showInfos());
                }
            }

            //DB CONNECTION TO INSERT

            //try
            //{
            //    string myConnection = "datasource=127.0.0.1; port=3306; username=root; password=";
            //    MySqlConnection myConn = new MySqlConnection(myConnection);
            //    MySqlCommand InsertCommand = myConn.CreateCommand();
            //    InsertCommand.CommandText = "INSERT INTO bus_data.transaction_info(userid,ctrlNumber,class,to,from,date,,roundtrip,insurance,)VALUES ('" + fullname + "','" + email + "','" + password + "')";
            //    myConn.Open();
            //    InsertCommand.ExecuteNonQuery();
            //    MessageBox.Show("Record Saved!");
            //    myConn.Close();
            //}

            //catch (Exception s)
            //{
            //    MessageBox.Show(s.Message);
            //}

            FormManager.ShowForm(FormManager.receipt);
            FormManager.HideForm(FormManager.book);

        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.home);
            FormManager.HideForm(FormManager.book);
        }

        private void ticketsButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.tickets);
            FormManager.HideForm(FormManager.book);
        }
        private void signOutButton_Click(object sender, EventArgs e)
        {
            User.fullname = "";
            User.id = 0;
            FormManager.ShowForm(FormManager.login);
            FormManager.HideForm(FormManager.book);
        }

        private void Book_Load(object sender, EventArgs e)
        {
            fullnameText.Text = User.fullname;
            Passenger.controlNumber = ControlNumber.generate();
        }

        private void fromInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            passenger.From = fromInput.Text;
        }

        private void toInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            passenger.To = toInput.Text;
        }

        private void classA_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'A';
            hasClass = true;
        }

        private void classB_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'B';
            hasClass = true;
        }

        private void classC_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'C';
            hasClass = true;
        }

        private void classD_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'D';
            hasClass = true;
        }

        private void roundtripInput_CheckedChanged(object sender, EventArgs e)
        {
            passenger.IsRoundtrip = roundtripInput.Checked;
        }

        private void insuranceInput_CheckedChanged(object sender, EventArgs e)
        {
            passenger.IsInsurance = insuranceInput.Checked;
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            if (!isCompleteForm()) 
            {
                MessageBox.Show("Complete the required information");
                return;
            };

            passenger.Date = dateInput.Text;
            passenger.Lastname = lastnameInput.Text;
            passenger.Firstname = firstnameInput.Text;
            passenger.MiddleInitial = miInput.Text[0];
            passenger.Alias = aliasInput.Text;
            passenger.Age = Int32.Parse(ageInput.Text);

            allPassenger.Add(passenger);
            passenger = new Passenger();
            resetInfos();
        }
        public void resetInfos()
        {
            fromInput.SelectedIndex = -1;
            toInput.SelectedIndex = -1;
            classA.Checked = false;
            classB.Checked = false;
            classC.Checked = false;
            classD.Checked = false;
            roundtripInput.Checked = false;
            insuranceInput.Checked = false;
            lastnameInput.Clear();
            firstnameInput.Clear();
            miInput.Clear();
            aliasInput.Clear();
            ageInput.Clear();
        }
        public bool isCompleteForm()
        {
            if (fromInput.SelectedIndex == -1 || toInput.SelectedIndex == -1)
            {
                return false;
            }

            if (!hasClass)
            {
                return false;
            }

            if (string.IsNullOrEmpty(lastnameInput.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(firstnameInput.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(miInput.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(aliasInput.Text))
            {
                return false;
            }

            if (string.IsNullOrEmpty(ageInput.Text))
            {
                return false;
            }

            return true;
        }
    }
}
