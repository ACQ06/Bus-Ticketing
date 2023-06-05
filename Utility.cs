using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public static double computeBusFee(string From, string To, char busType)
        {
            double busFee = 0;

            if (From == "Manila")
            {
                switch (To)
                {
                    case "Ilocos":
                        switch (busType)
                        {
                            case 'A':
                                busFee = 950;
                                break;
                            case 'D':
                                busFee = 1100;
                                break;
                        }
                        break;

                    case "Pampanga":
                        switch (busType)
                        {
                            case 'B':
                                busFee = 400;
                                break;
                            case 'C':
                                busFee = 375;
                                break;
                            case 'D':
                                busFee = 600;
                                break;
                        }
                        break;

                    case "Zambales":

                        busFee = 390;
                        break;

                    case "Baguio":
                        switch (busType)
                        {
                            case 'A':
                                busFee = 785;
                                break;
                            case 'C':
                                busFee = 400;
                                break;
                            case 'D':
                                busFee = 788;
                                break;
                        }
                        break;

                    case "Tugegarao":
                        switch (busType)
                        {
                            case 'A':
                                busFee = 1300;
                                break;
                            case 'B':
                                busFee = 975;
                                break;
                        }
                        break;
                }
            } else if (From == "Ilocos") {
                switch (busType)
                {
                    case 'A':
                        busFee = 995;
                        break;
                    case 'D':
                        busFee = 1350;
                        break;
                }
            } else if (From == "Pampanga") {
                switch (busType)
                {
                    case 'B':
                        busFee = 450;
                        break;
                    case 'C':
                        busFee = 400;
                        break;
                    case 'D':
                        busFee = 700;
                        break;
                }
            } else if (From == "Zambales") {
                busFee = 430;
            } else if (From == "Baguio") {
                switch (busType)
                {
                    case 'A':
                        busFee = 830;
                        break;
                    case 'C':
                        busFee = 430;
                        break;
                    case 'D':
                        busFee = 840;
                        break;
                }
            } else if (From == "Tugegarao") {
                switch (busType)
                {
                    case 'A':
                        busFee = 1340;
                        break;
                    case 'B':
                        busFee = 1000;
                        break;
                }
            }


            return busFee;
        }

        public static double computeStandardProcessingFee(string From, string To, char busType)
        {
            double standardProcessingFee = 85;
            return standardProcessingFee;
        }

        public static double computeAdditionalProcessingFee(char busType)
        {
            double additionalProcessingFee = 0;

            switch (busType)
            {
                case 'A':
                    additionalProcessingFee = 30;
                    break;

                case 'B':
                    additionalProcessingFee = 20;
                    break;

                case 'C':
                    additionalProcessingFee = 15;
                    break;

                case 'D':
                    additionalProcessingFee = 10;
                    break;
            }
            return additionalProcessingFee;
        }

        public static double computeInsuranceFee(char busType)
        {
            double insuranceFee = 0;

            switch (busType)
            {
                case 'A':
                    insuranceFee = 195;
                    break;

                case 'B':
                    insuranceFee = 140;
                    break;

                case 'C':
                    insuranceFee = 95;
                    break;

                case 'D':
                    insuranceFee = 50;
                    break;
            }


            return insuranceFee;
        }

        public static double computeTotalDiscount(int age)
        {
            double totalDiscount = 0;

            if (age > 59)
            {
                totalDiscount = (computeStandardProcessingFee(/*insert SQL statement*/) + computeAdditionalProcessingFee(/*insert SQL statement*/)) * .12;
)           }

            return totalDiscount;
        }

        /* NOT NEEDED ANYMORE
        public static double computeTotalTax()
        {
            double totalTax = 0;
            return totalTax;
        }
        */

        public static double computeTotalCharge()
        {
            double totalCharge = 0;

            totalCharge = (computeStandardProcessingFee(/*insert SQL statement*/) + computeAdditionalProcessingFee(/*insert SQL statement*/)) - computeTotalDiscount(/*insert SQL statement*/);

            return totalCharge;
        }
    }
}