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
    public partial class Receipt : Form
    {
        int counter = 0;

        Passenger passengerDetail;
        List<Passenger> allPassengers = new List<Passenger>();

        public Receipt()
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

        private void exitProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Receipt_Load(object sender, EventArgs e)
        {
            fullnameText.Text = User.fullname;
            controlNumberLabel.Text = Passenger.controlNumber;

            getDataFromSql();
            loadPassengerDetail();
        }

        private void bookButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.book);
            FormManager.HideForm(FormManager.receipt);
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.home);
            FormManager.HideForm(FormManager.receipt);
        }

        private void ticketButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.tickets);
            FormManager.HideForm(FormManager.receipt);
        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            User.fullname = "";
            User.id = 0;
            FormManager.ShowForm(FormManager.login);
            FormManager.HideForm(FormManager.tickets);
        }

        private void getDataFromSql()
        {
            try
            {
                string myConnection = "datasource=127.0.0.1; port=3306; username=root; database=bus_data; password=";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand SelectCommand = new MySqlCommand($"Select * from bus_data.transaction_info WHERE userid='{User.id}' AND ctrlNumber='{Passenger.controlNumber}'", myConn);
                MySqlDataReader myReader;
                myConn.Open();
                myReader = SelectCommand.ExecuteReader();   

                while (myReader.Read())
                {
                    passengerDetail = new Passenger();

                    passengerDetail.Lastname = myReader.GetString("lastname");
                    passengerDetail.Firstname = myReader.GetString("firstname");
                    passengerDetail.MiddleInitial = myReader.GetChar("mi");
                    passengerDetail.Alias = myReader.GetString("alias");
                    passengerDetail.Age = myReader.GetInt32("age");
                    passengerDetail.Date = myReader.GetString("date");
                    passengerDetail.From = myReader.GetString("from");
                    passengerDetail.To = myReader.GetString("to");
                    passengerDetail.Class_ = myReader.GetChar("class");
                    passengerDetail.IsRoundtrip = myReader.GetBoolean("roundtrip");
                    passengerDetail.IsInsurance = myReader.GetBoolean("insurance");

                    allPassengers.Add(passengerDetail);
                }

                myConn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadPassengerDetail()
        {
            try
            {
                nameText.Text = $"{allPassengers[counter].Lastname}, {allPassengers[counter].Firstname} {allPassengers[counter].MiddleInitial}.";
                dateText.Text = allPassengers[counter].Date;
                fromText.Text = allPassengers[counter].From;
                toText.Text = allPassengers[counter].To;
                classText.Text = allPassengers[counter].Class_.ToString();
                roundtripText.Text = allPassengers[counter].IsRoundtrip.ToString();

                double busFee = Charge.computeBusFee(allPassengers[counter].From, allPassengers[counter].To, allPassengers[counter].Class_, allPassengers[counter].IsRoundtrip);
                double standardProcessingFee = Charge.computeStandardProcessingFee(allPassengers[counter].From, allPassengers[counter].To, allPassengers[counter].Class_);
                double additionalProcessingFee = Charge.computeAdditionalProcessingFee(allPassengers[counter].From, allPassengers[counter].IsRoundtrip, allPassengers[counter].Class_);
                double totalDiscount = Charge.computeTotalDiscount(allPassengers[counter].Age, standardProcessingFee, additionalProcessingFee);
                double totalProcessingFee = Charge.computeTotalProcessingFee(standardProcessingFee, additionalProcessingFee, totalDiscount);
                double travelInsurance = Charge.computeInsuranceFee(allPassengers[counter].Class_, allPassengers[counter].IsInsurance);
                double totalCharge = Charge.computeFinalCharges(busFee, travelInsurance, totalProcessingFee);


                busFeeText.Text = busFee.ToString();
                standardFeeText.Text = standardProcessingFee.ToString();
                additionalFeeText.Text = additionalProcessingFee.ToString();
                totalProcessingFeeText.Text = totalProcessingFee.ToString();
                insuranceFeeText.Text = travelInsurance.ToString();
                discountText.Text= totalDiscount.ToString();
                totalChargeText.Text = totalCharge.ToString();
            }

            catch
            {
                MessageBox.Show("Error loading passenger details");
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            if (counter <= 0)
            {
                MessageBox.Show("That's the start of passenger list");
                return;
            }

            counter--;
            loadPassengerDetail();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (counter >= allPassengers.Count())
            {
                MessageBox.Show("That's the end of passenger list");
                return;
            }

            counter++;
            loadPassengerDetail();
        }
    }
}
