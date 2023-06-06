using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bus_Ticketing
{
    public static class FormManager
    {
        public static loginForm login { get; set; }
        public static SignUpForm signUp { get; set; }
        public static Home home { get; set; }
        public static Book book { get; set; }
        public static Tickets tickets { get; set; }
        public static Receipt receipt { get; set; }

        public static void ShowForm(Form form)
        {
            form.Show();
        }

        public static void HideForm(Form form)
        {
            form.Hide();
        }

        public static void CloseForm(Form form)
        {
            form.Close();
        }
    }
}
