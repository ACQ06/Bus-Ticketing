﻿using System;
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
        string controlNumber = Passenger.controlNumber;
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

        private void Receipt_Load(object sender, EventArgs e)
        {
            fullnameText.Text = User.fullname;
            controlNumberLabel.Text = controlNumber;
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
    }
}
