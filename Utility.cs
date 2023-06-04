using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Ticketing
{
    public class User
    {
        public static string fullname { get; set; }
        public static int id { get; set; }
    }

    public class Passenger
    {
        public static string controlNumber { get; set; }
        public static char class_{ get; set; }
        public static string to { get; set; }
        public static string from { get; set; }
        public static string date { get; set; }
        public static bool is_rountrip { get; set; }
        public static bool is_insurance { get; set; }
        public static string lastname { get; set; }
        public static string firstname { get; set;}
        public static char middleInitial { get; set; }
        public static string alis { get; set; }
        public static int age { get; set; }
        public static int total_char { get; set; }
    }

    public class Transaction
    {
        /// <summary>
        /// Key: ControlNumber of the transaction
        /// Value: Passenger Details
        /// </summary>
        public Dictionary<string, object> transaction { get; set; }
    }

    public class ControlNumber
    {
        public static string generate()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder stringBuilder = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(0, chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }
    }
}