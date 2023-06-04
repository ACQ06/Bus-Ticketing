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
        private char class_;
        private string to;
        private string from;
        private string date;
        private bool is_roundtrip;
        private bool is_insurance;
        private string lastname;
        private string firstname;
        private char middleInitial;
        private string alias;
        private int age;

        public char Class_
        {
            get { return class_; }
            set { class_ = value; }
        }

        public string To
        {
            get { return to; }
            set { to = value; }
        }

        public string From
        {
            get { return from; }
            set { from = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public bool IsRoundtrip
        {
            get { return is_roundtrip; }
            set { is_roundtrip = value; }
        }

        public bool IsInsurance
        {
            get { return is_insurance; }
            set { is_insurance = value; }
        }

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public char MiddleInitial
        {
            get { return middleInitial; }
            set { middleInitial = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string showInfos()
        {
            return $"Control Number: {controlNumber}\n" +
                   $"Class: {Class_}\n" +
                   $"To: {To}\n" +
                   $"From: {From}\n" +
                   $"Date: {Date}\n" +
                   $"Is Roundtrip: {IsRoundtrip}\n" +
                   $"Is Insurance: {IsInsurance}\n" +
                   $"Lastname: {Lastname}\n" +
                   $"Firstname: {Firstname}\n" +
                   $"Middle Initial: {MiddleInitial}\n" +
                   $"Alias: {Alias}\n" +
                   $"Age: {Age}";
        }
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

    //DO COMPUTATION HERE
    public class Charge
    {
        //GET THE PASSENGER DETAILS FROM THE SQL USING THE USERID AND CONTROL NUMBER AS A FILTER FOR SQL LOOKUP
        //NOTE: IT SHOULD BE PER PASSENGER
        public static double computeBusFee(string From, string To)
        {
            double busFee = 0;
            return busFee;
        }

        public static double computeStandardProcessingFee()
        {
            double standardProcessingFee = 0;
            return standardProcessingFee;
        }

        public static double computeAdditionalProcessingFee()
        {
            double additionalProcessingFee = 0;
            return additionalProcessingFee;
        }

        public static double computeInsuranceFee()
        {
            double insuranceFee = 0;
            return insuranceFee;
        }

        public static double computeTotalDiscount()
        {
            double totalDiscount = 0;
            return totalDiscount;
        }

        public static double computeTotalTax()
        {
            double totalTax = 0;
            return totalTax;
        }

        public static double computeTotalCharge()
        {
            double totalCharge = 0;
            return totalCharge;
        }
    }
}