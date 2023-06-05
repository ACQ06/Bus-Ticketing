using MySql.Data.MySqlClient;
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
        Dictionary<char, int> classSize = new Dictionary<char, int>
        {
            {'A', 0},
            {'B', 0},
            {'C', 0},
            {'D', 0}
        };
        Dictionary<char, int> classSizeLimit = new Dictionary<char, int>
        {
            {'A', 21},
            {'B', 36},
            {'C', 55},
            {'D', 8}
        };
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
        private void exitProgram(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //FORM METHODS
        private void payButton_Click(object sender, EventArgs e)
        {
            if (allPassenger.Count == 0)
            {
                MessageBox.Show("Add a passenger");
                return;
            }

            if (!isAccompanied())
            {
                MessageBox.Show("Child cannot travel alone must be accompanied by at least one (1) Adult and or Senior Citizen");
                return;
            }

            Dictionary<string, List<Passenger>> transaction = new Dictionary<string, List<Passenger>>()
            {
                { Passenger.controlNumber, allPassenger}
            };

            foreach (KeyValuePair<string, List<Passenger>> kvp in transaction)
            {
                string key = kvp.Key;
                List<Passenger> passengers = kvp.Value;

                foreach (Passenger passenger in passengers)
                {
                    try
                    {
                        string myConnection = "datasource=127.0.0.1; port=3306; username=root; password=";
                        MySqlConnection myConn = new MySqlConnection(myConnection);
                        MySqlCommand InsertCommand = myConn.CreateCommand();
                        InsertCommand.CommandText = "INSERT INTO bus_data.transaction_info(userid, ctrlNumber, class, `to`, `from`, date, roundtrip, insurance, lastname, firstname, mi, alias, age) " +
                                                    "VALUES (@userid, @ctrlNumber, @class, @to, @from, @date, @roundtrip, @insurance, @lastname, @firstname, @mi, @alias, @age)";

                        // Set parameter values
                        InsertCommand.Parameters.AddWithValue("@userid", User.id);
                        InsertCommand.Parameters.AddWithValue("@ctrlNumber", Passenger.controlNumber);
                        InsertCommand.Parameters.AddWithValue("@class", passenger.Class_);
                        InsertCommand.Parameters.AddWithValue("@to", passenger.To);
                        InsertCommand.Parameters.AddWithValue("@from", passenger.From);
                        InsertCommand.Parameters.AddWithValue("@date", passenger.Date);
                        InsertCommand.Parameters.AddWithValue("@roundtrip", passenger.IsRoundtrip);
                        InsertCommand.Parameters.AddWithValue("@insurance", passenger.IsInsurance);
                        InsertCommand.Parameters.AddWithValue("@lastname", passenger.Lastname);
                        InsertCommand.Parameters.AddWithValue("@firstname", passenger.Firstname);
                        InsertCommand.Parameters.AddWithValue("@mi", passenger.MiddleInitial);
                        InsertCommand.Parameters.AddWithValue("@alias", passenger.Alias);
                        InsertCommand.Parameters.AddWithValue("@age", passenger.Age);

                        myConn.Open();
                        InsertCommand.ExecuteNonQuery();
                        myConn.Close();
                    }
                    catch (Exception s)
                    {
                        MessageBox.Show(s.Message);
                    }
                }
            }

            FormManager.ShowForm(FormManager.receipt);
            FormManager.HideForm(FormManager.book);

        }
        private bool isAccompanied()
        {
            int childCount = 0;
            int adultCount = 0;

            foreach(Passenger passenger in allPassenger)
            {
                if (passenger.Age < 18) {
                    childCount++;
                    continue;
                }
                adultCount++;
            }

            if(childCount == 0) return true;

            if (adultCount > 0) return true;

            return false;
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
            updateClassCount();
            Passenger.controlNumber = ControlNumber.generate();
            hideForm();
        }
        private void fromInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fromInput.Text == "Select from choices")
            {
                fromInput.SelectedIndexChanged -= fromInput_SelectedIndexChanged;
                resetChoices();
                resetForm();
                hideAllForms();
                fromInput.SelectedIndexChanged += fromInput_SelectedIndexChanged;
                return;
            }
            passenger.From = fromInput.Text;
            List<string> removeChoice = getDestination(fromInput.Text);

            foreach (string items in removeChoice) toInput.Items.Remove(items);
            showHalfForm();
        }
        private void toInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toInput.Text == "Select from choices")
            {
                toInput.SelectedIndexChanged -= toInput_SelectedIndexChanged;
                resetChoices();
                resetForm();
                hideAllForms();
                toInput.SelectedIndexChanged += toInput_SelectedIndexChanged;
                return;
            }

            passenger.To = toInput.Text;
            List<string> removeChoice = getDestination(toInput.Text);

            foreach(string items in removeChoice) fromInput.Items.Remove(items);
            showHalfForm();
        }
        public void resetChoices()
        {
            List<string> choices = new List<string>()
            {
                "Select from choices",
                "Manila",
                "Ilocos",
                "Pampanga",
                "Zambales",
                "Baguio",
                "Tugegarao"
            };

            toInput.Items.Clear();
            fromInput.Items.Clear();

            foreach (string items in choices)
            {
                toInput.Items.Add(items);
                fromInput.Items.Add(items);
            }

            toInput.SelectedIndex = 0;
            fromInput.SelectedIndex = 0;

        }
        public static List<string> getDestination(string current)
        {
            List<string> destinations = new List<string>()
            {
                "Manila",
                "Ilocos",
                "Pampanga",
                "Zambales",
                "Baguio",
                "Tugegarao"
            };

            if (current == "Manila")
            {
                return new List<string> { "Manila" };
            }

            destinations.Remove("Manila");

            return destinations;
        }
        private void classA_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'A';
            hasClass = true;
            showPassengerDetails();
        }
        private void classB_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'B';
            hasClass = true;
            showPassengerDetails();
        }
        private void classC_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'C';
            hasClass = true;
            showPassengerDetails();
        }
        private void classD_CheckedChanged(object sender, EventArgs e)
        {
            passenger.Class_ = 'D';
            hasClass = true;
            showPassengerDetails();
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
                return;
            };

            passenger.Date = dateInput.Text;
            passenger.Lastname = lastnameInput.Text;
            passenger.Firstname = firstnameInput.Text;
            passenger.MiddleInitial = miInput.Text[0];
            passenger.Alias = aliasInput.Text;

            try
            {
                passenger.Age = Int32.Parse(ageInput.Text);
            }

            catch
            {
                MessageBox.Show("Invalid Age");
                return;
            }
            

            if (exceedClassSize(passenger.Class_))
            {
                MessageBox.Show("Sorry… you  have exceeded the number of passengers required");
                return;
            }
            
            allPassenger.Add(passenger);
            classSize[passenger.Class_]++;
            updateClassCount();

            passenger = new Passenger();
            resetInfos();
        }
        private bool exceedClassSize(char class_)
        {
            return classSize[class_] == classSizeLimit[class_];
        }
        private void resetInfos()
        {
            resetChoices();
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
        private bool isCompleteForm()
        {
            if ((fromInput.SelectedIndex <= 0 || toInput.SelectedIndex <= 0) &&
                !hasClass &&
                string.IsNullOrEmpty(lastnameInput.Text) &&
                string.IsNullOrEmpty(firstnameInput.Text) &&
                string.IsNullOrEmpty(miInput.Text) &&
                string.IsNullOrEmpty(ageInput.Text))    
            {
                MessageBox.Show("Input Necessary Details");
                return false;
            }

            if (fromInput.SelectedIndex <= 0 || toInput.SelectedIndex <= 0)
            {
                MessageBox.Show("Input Travel Destination / No Trip Available");
                return false;
            }

            if (!hasClass)
            {
                MessageBox.Show("Invalid Input");
                return false;
            }

            if (string.IsNullOrEmpty(lastnameInput.Text))
            {
                MessageBox.Show("Invalid Input");
                return false;
            }

            if (string.IsNullOrEmpty(firstnameInput.Text))
            {
                MessageBox.Show("Invalid Input");
                return false;
            }

            if (string.IsNullOrEmpty(miInput.Text))
            {
                MessageBox.Show("Invalid Input");
                return false;
            }

            if (string.IsNullOrEmpty(ageInput.Text))
            {
                MessageBox.Show("Invalid Input");
                return false;
            }

            return true;
        }
        private void hideForm()
        {
            classPanel.Hide();
            dateInput.Hide();
            roundtripPanel.Hide();
            insurancePanel.Hide();
            passDetailsPanel.Hide();
        }
        private void showHalfForm()
        {
            showLimitedClass();
            if(fromInput.SelectedIndex == 0 || toInput.SelectedIndex == 0) return;
            classPanel.Show();
            dateInput.Show();
            roundtripPanel.Show();
            insurancePanel.Show();
        }
        private void showPassengerDetails()
        {
            if (!hasClass) return;
            passDetailsPanel.Show();
        }
        private void showLimitedClass()
        {
            if(toInput.SelectedIndex == 0 || fromInput.SelectedIndex == 0)
            {
                return;
            }

            switch (passenger.From)
            {
                case "Manila":
                    switch (passenger.To)
                    {
                        case "Ilocos":
                            classB.Hide();
                            classC.Hide();
                            break;

                        case "Pampanga":
                            classA.Hide();
                            break;

                        case "Zambales":
                            classA.Hide();
                            classC.Hide();
                            classD.Hide();
                            break;

                        case "Baguio":
                            classB.Hide();
                            break;

                        case "Tugegarao":
                            classC.Hide();
                            classD.Hide();
                            break;
                    }
                    break;

                case "Ilocos":
                    classB.Hide();
                    classC.Hide();
                    break;

                case "Pampanga":
                    classA.Hide();
                    break;

                case "Zambales":
                    classA.Hide();
                    classC.Hide();
                    classD.Hide();
                    break;

                case "Baguio":
                    classB.Hide();
                    break;

                case "Tugegarao":
                    classC.Hide();
                    classD.Hide();
                    break;
            }
        }
        private void resetForm()
        {
            classA.Show();
            classB.Show();
            classC.Show();
            classD.Show();

            classA.Checked = false;
            classB.Checked = false;
            classC.Checked = false;
            classD.Checked = false;
            roundtripInput.Checked = false;
            insuranceInput.Checked = false;
            lastnameInput.Text = null;
            firstnameInput.Text = null;
            miInput.Text = null;
            aliasInput.Text = null;
            ageInput.Text = null;
        }
        private void hideAllForms()
        {
            classPanel.Hide();
            dateInput.Hide();
            roundtripPanel.Hide();
            insurancePanel.Hide();
            passDetailsPanel.Hide();
        }
        private void updateClassCount()
        {
            countA.Text = $"{classSize['A']}/{classSizeLimit['A']}";
            countB.Text = $"{classSize['B']}/{classSizeLimit['B']}";
            countC.Text = $"{classSize['C']}/{classSizeLimit['C']}";
            countD.Text = $"{classSize['D']}/{classSizeLimit['D']}";
        }
    }
}