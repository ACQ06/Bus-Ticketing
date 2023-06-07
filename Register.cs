using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Bus_Ticketing
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
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

        private void signUpButton_Click(object sender, EventArgs e)
        {
            string namePattern = @"[A-Za-z]";
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            string passwordPattern = namePattern;

            string fullname = fullnameInput.Text;
            string email = emailInput.Text;
            string password = passwordInput.Text;

            if (!Regex.IsMatch(fullname[0].ToString(), namePattern))
            {
                MessageBox.Show("Enter the right name");
                return;
            }

            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Enter the right email");
                return;
            }

            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Complete the password");
                return;
            }

            try
            {
                string myConnection = "datasource=127.0.0.1; port=3306; username=root; password=";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand InsertCommand = myConn.CreateCommand();
                InsertCommand.CommandText = $"INSERT INTO bus_data.user_info(fullname,email,password)VALUES ('{fullname}','{email}','{password}')";
                myConn.Open();
                InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Record Saved!");
                myConn.Close();

                fullnameInput.Clear();
                emailInput.Clear();
                passwordInput.Clear();

                loginForm login = (loginForm)this.Owner;
                login.Show();
                this.Close();
            }

            catch (Exception s)
            {
                MessageBox.Show(s.Message);
            }
            
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.login);
            FormManager.HideForm(FormManager.signUp);
        }
    }
}
