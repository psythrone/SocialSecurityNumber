using System;
using System.Globalization;

namespace SocialSecurityNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            string socialSecurityNumber;

            if (args.Length == 0)
            {
                Console.Write("Please enter a Social security number (YYMMDD-XXXX): ");
                socialSecurityNumber = Console.ReadLine();
            }
            else
            {
                socialSecurityNumber = args[0];
            }

            Console.Clear();

            
            //get date part of Social Security Number by removing the last 5 
            string birthDateString = socialSecurityNumber.Substring(0, socialSecurityNumber.Length - 5);

            bool isFemale = socialSecurityNumber.Length == 11 ? (int.Parse(socialSecurityNumber.Substring(9, 1)) % 2 == 0) : (int.Parse(socialSecurityNumber.Substring(11, 1)) % 2 == 0);
            
            // Write:[SSN] is a [Age] year old [Sex] ([Verification])
            Console.WriteLine(
                $"{socialSecurityNumber} is a {CalculateAge(birthDateString, out string generation)} year old {(isFemale ? "female" : "male")} belonging to {generation}{(VerifySocialSecurityControlNumber(socialSecurityNumber) ? "" : " and is not a real person")}");
        }


        /// <summary>
        /// Calculates a persons current age given their birth date
        /// </summary>
        /// <param name="socialSecurityNumber">A birth date using format (yyMMdd) or (yyyyMMdd)</param>
        /// <param name="generation">Out parameter containing the persons generation</param>
        /// <returns>The persons age</returns>
        protected static int CalculateAge(string socialSecurityNumber, out string generation)
        {
            int age;
            DateTime birthDate = socialSecurityNumber.Length == 6 ? DateTime.ParseExact(socialSecurityNumber, "yyMMdd", CultureInfo.InvariantCulture) : DateTime.ParseExact(socialSecurityNumber, "yyyyMMdd", CultureInfo.InvariantCulture);

            age = DateTime.Today.Year - birthDate.Year;

            //if the person hasn't had their birthday this year yet we reduce their age by one
            if (birthDate.Month > DateTime.Today.Month
                || birthDate.Month == DateTime.Today.Month && birthDate.Day > DateTime.Today.Day)
            {
                age--;
            }

            //add generation to age
            if (birthDate.Year < 1925)
                generation = "The Greatest Generation";
            else if (birthDate.Year < 1946)
                generation = "The Silent Generation";
            else if (birthDate.Year < 1965)
                generation = "Baby Boomer Generation";
            else if (birthDate.Year < 1981)
                generation = "Gen X";
            else if (birthDate.Year < 1995)
                generation = "Millenials";
            else if (birthDate.Year < 2013)
                generation = "Gen Z";
            else if (birthDate.Year < 2026)
                generation = "Gen Alpha";
            else
                generation = "Unnamed future generation";

            return age;
        }


        /// <summary>
        /// Calculates and checks the verification number on a Swedish social security number
        /// </summary>
        /// <param name="socialSecurityNumber">A Swedish Social Security Number with the format (YYMMDD-NNNN)</param>
        /// <returns>True if Social Security Verification digit is correct</returns>
        protected static bool VerifySocialSecurityControlNumber(string socialSecurityNumber)
        {
            //Remove first two digits if format is yyyyMMdd-nnnn, format should be yyMMdd-nnnn
            if (socialSecurityNumber.Length == 13)
                socialSecurityNumber = socialSecurityNumber.Substring(2);
            //Removes the dash for calculation purposes
            socialSecurityNumber = socialSecurityNumber.Remove(6, 1);

            int accumulation = 0;

            // Calculate verification digit
            for (int i = 1; i < 10; i++)
            {
                int digit = int.Parse(socialSecurityNumber.Substring(i - 1, 1)) * ((i % 2) + 1);
                if (digit > 9)
                {
                    int n = digit % 10;
                    digit = (digit / 10) + n;
                }
                accumulation += digit;
            }
            int socialSecturityControlNumber = (10 + ((accumulation / 10) * 10)) - accumulation;

            return socialSecturityControlNumber == int.Parse(socialSecurityNumber.Substring(socialSecurityNumber.Length - 1));
        }

    }
}

/**
 * Generations:
 * 
 * 1910-1924: The Greatest Generation
 * 1925-1945: The Silent Generation
 * 1946-1964: Baby Boomer
 * 1965-1979: Gen X
 * 1980-1994: Millenial
 * 1995-2012: Gen Z
 * 2013-2025: Gen Alpha
 */




//Console.WriteLine("We come in peace!");
//Console.Beep(229, 800);
//Console.Beep(329, 800);
//Console.Beep(261, 800);
//Console.Beep(130, 800);
//Console.Beep(196, 1600);