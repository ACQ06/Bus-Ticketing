using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Bus_Ticketing
{
    public partial class loginForm : Form
    {
        
        public loginForm()
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

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                string myConnection = "datasource=127.0.0.1; port=3306; username=root; database=bus_data; password=";
                MySqlConnection myConn = new MySqlConnection(myConnection);
                MySqlCommand SelectCommand = new MySqlCommand("Select * from bus_data.user_info where email='" + this.emailInput.Text + "' and password='" + this.passwordInput.Text + "';", myConn);
                MySqlDataReader myReader;
                myConn.Open();
                myReader = SelectCommand.ExecuteReader();

                int count = 0;
                while (myReader.Read()){
                    count = count + 1;
                }

                if(count == 1) {
                    MessageBox.Show("Username and password is correct!");

                    emailInput.Clear();
                    passwordInput.Clear();

                    User.id = int.Parse(myReader.GetValue(0).ToString());
                    User.fullname = myReader.GetValue(1).ToString();

                    FormManager.ShowForm(FormManager.home);
                    FormManager.HideForm(FormManager.login);

                } else if (count > 1) {
                    MessageBox.Show("Duplicate username and password. Access not granted!");
                } else {
                    MessageBox.Show("Username and Password are not correct. Try again!");
                    myConn.Close();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void signUpButton_Click(object sender, EventArgs e)
        {
            FormManager.ShowForm(FormManager.signUp);
            FormManager.HideForm(FormManager.login);
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            FormManager.login = this;
            FormManager.signUp = new SignUpForm();
            FormManager.home = new Home();
            FormManager.book = new Book();
            FormManager.tickets = new Tickets();
            FormManager.receipt = new Receipt();
        }
    }
}