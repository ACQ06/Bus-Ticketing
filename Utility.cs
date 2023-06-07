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

        private string ctrlNumber;
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

        public string CtrlNumber
        {
            get { return ctrlNumber; }
            set { ctrlNumber = value; }
        }

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
    public class Charge
    {
        /// <summary>
        /// Computes the bus fee
        /// </summary>
        /// <param name="From">Destination from</param>
        /// <param name="To">Destination to</param>
        /// <param name="busType">Bus class</param>
        /// <returns>Bus fee</returns>
        public static double computeBusFee(string From, string To, char busType, bool IsRoundtrip)
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

            if (IsRoundtrip) busFee *= 2;

            return busFee;
        }

        /// <summary>
        /// Computes the processing fee
        /// </summary>
        /// <param name="From">Destination from</param>
        /// <param name="To">Destination to</param>
        /// <param name="busType">Bus class</param>
        /// <returns>Processing fee</returns>
        public static double computeStandardProcessingFee(string From, string To, char busType)
        {
            double standardProcessingFee = 85;
            return standardProcessingFee;
        }


        /// <summary>
        /// Computes the addtional processing fee
        /// </summary>
        /// <param name="From"></param>
        /// <param name="IsRoundtrip"></param>
        /// <param name="busType"></param>
        /// <returns></returns>
        public static double computeAdditionalProcessingFee(string From, bool IsRoundtrip, char busType)
        {
            double additionalProcessingFee = 0;

            if (!(From.Equals("Manila") && !IsRoundtrip))
            {
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
            }
            return additionalProcessingFee;
        }

        /// <summary>
        /// Computes the insurance
        /// </summary>
        /// <param name="busType">Bus class</param>
        /// <returns>Insurance</returns>
        public static double computeInsuranceFee(char busType, bool isInsured, bool IsRoundtrip)
        {
            double insuranceFee = 0;

            if (!isInsured)
            {
                return insuranceFee;
            }

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

            return IsRoundtrip? insuranceFee*2:insuranceFee;
        }

        /// <summary>
        /// Computes the total discount
        /// </summary>
        /// <param name="age">Age of the passenger</param>
        /// <param name="spf">Standard processing fee</param>
        /// <param name="apf">Additional processing fee</param>
        /// <returns>Total discount</returns>
        public static double computeTotalDiscount(int age, double spf, double apf)
        {
            double totalDiscount = 0;

            if (age > 59)
            {
                totalDiscount = (spf + apf) * .12;
            }

            return totalDiscount;
        }

        /// <summary>
        /// Computes total processing fee
        /// </summary>
        /// <param name="spf">Standard processing fee</param>
        /// <param name="apf">Additonal processing fee</param>
        /// <param name="totalDiscount">Total discount</param>
        /// <returns>Total processing fee</returns>
        public static double computeTotalProcessingFee(double spf, double apf, double totalDiscount)
        {
            double totalCharge = 0;

            totalCharge = (spf + apf) - totalDiscount;

            return totalCharge;
        }

        /// <summary>
        /// Computes Total Charges
        /// </summary>
        /// <param name="busFee">Bus fee</param>
        /// <param name="insuranceFee">Insurance fee</param>
        /// <param name="totalCharge">Total charge</param>
        /// <returns>Total charge</returns>
        public static double computeFinalCharges(double busFee, double insuranceFee, double totalProcessingFee)
        {
            double finalCharges = 0;

            finalCharges = (busFee + insuranceFee + totalProcessingFee);

            return finalCharges;
        }
    }
}