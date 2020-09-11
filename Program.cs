using System;
using System.Globalization;

namespace SocialSecurityNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter your First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Please enter your Last name: ");
            string lastName = Console.ReadLine();

            string socialSecurityNumber;

            if (!(args.Length == 0))
            {
                socialSecurityNumber = args[0];
            }
            else
            {
                Console.Write("Please enter a Social security number (YYMMDD-XXXX): ");
                socialSecurityNumber = Console.ReadLine();
            }
            Console.Clear();

            // get date part of Social Security Number by removing the last 5 
            //string birthDateString = socialSecurityNumber.Substring(0, socialSecurityNumber.Length - 5);
            // VS told me to try this instead of substring
            string birthDateString = socialSecurityNumber[0..^5];

            // check the second to last number for sex i.e. even for female, odd for male
            bool isFemale = int.Parse(socialSecurityNumber.Substring(socialSecurityNumber.Length - 2, 1)) % 2 == 0;
            Gender gender = GetGender(socialSecurityNumber);


            Console.WriteLine($"Name:                   {firstName} {lastName}");
            Console.WriteLine($"Social Security Number: {socialSecurityNumber}");
            Console.WriteLine($"Gender:                 {gender}");
            Console.WriteLine($"Age:                    {CalculateAge(birthDateString, out Generation generation)}");
            Console.WriteLine($"Generation:             {generation.ToString().Replace('_', ' ')}");
            if (!VerifySocialSecurityControlNumber(socialSecurityNumber))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\nVerification digit is invalid!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /**
         * Name:                   
         * Social Security Number: 
         * Gender:                 
         * Age:                    
         * Generation:             
         * [VERIFICATION]
         */

        /// <summary>
        /// Calculates a persons current age given their birth date
        /// </summary>
        /// <param name="birthDateString">A birth date using format (yyMMdd) or (yyyyMMdd)</param>
        /// <param name="generation">Out parameter containing the persons generation</param>
        /// <returns>The persons age</returns>
        private static int CalculateAge(string birthDateString, out Generation generation)
        {
            int age;
            DateTime birthDate = birthDateString.Length == 6 ? DateTime.ParseExact(birthDateString, "yyMMdd", CultureInfo.InvariantCulture) : DateTime.ParseExact(birthDateString, "yyyyMMdd", CultureInfo.InvariantCulture);

            age = DateTime.Today.Year - birthDate.Year;

            //if the person hasn't had their birthday this year yet we reduce their age by one
            if (birthDate.Month > DateTime.Today.Month ||
                birthDate.Month == DateTime.Today.Month && birthDate.Day > DateTime.Today.Day)
            {
                age--;
            }

            //add generation to age
            if (birthDate.Year < 1925)
                generation = Generation.The_Greatest_Generation;
            else if (birthDate.Year < 1946)
                generation = Generation.The_Silent_Generation;
            else if (birthDate.Year < 1965)
                generation = Generation.Baby_Boomer;
            else if (birthDate.Year < 1980)
                generation = Generation.Gen_X;
            else if (birthDate.Year < 1995)
                generation = Generation.Millenial;
            else if (birthDate.Year < 2013)
                generation = Generation.Gen_Z;
            else
                generation = Generation.Gen_Alpha;



            return age;
        }


        /// <summary>
        /// Calculates and checks the verification number on a Swedish social security number
        /// </summary>
        /// <param name="socialSecurityNumber">A Swedish Social Security Number with the format (YYMMDD-NNNN)</param>
        /// <returns>True if Social Security Verification digit is correct</returns>
        private static bool VerifySocialSecurityControlNumber(string socialSecurityNumber)
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
                // multiply each digit alternatingly with 2 and 1 i.e. first digit gets doubled, second gets left alone, third gets doubled and so on
                int digit = int.Parse(socialSecurityNumber.Substring(i - 1, 1)) * ((i % 2) + 1);
                // if the result is 10 or greater, we add the component digits to each other i.e. 14 -> 1 + 4 = 5 
                if (digit > 9)
                {
                    int n = digit % 10;
                    digit = (digit / 10) + n;
                }
                // add all the digits together
                accumulation += digit;
            }
            // the final step in calculating the control number is that we take the next highest multiple of 10 and remove our calculated number from it i.e. if we got 44 we do 50 - 44 = 6 (effectivly 10 - (accumulation%10))
            int socialSecturityControlNumber = ((10 + ((accumulation / 10) * 10)) - accumulation) % 10;

            return socialSecturityControlNumber == int.Parse(socialSecurityNumber.Substring(socialSecurityNumber.Length - 1));
        }

        private static Gender GetGender(string socialSecurityNumber)
        {
            bool isFemale = int.Parse(socialSecurityNumber.Substring(socialSecurityNumber.Length - 2, 1)) % 2 == 0;

            return isFemale ? Gender.Female : Gender.Male;
        }

        private enum Gender
        {
            Female, Male
        }

        private enum Generation
        {
            The_Greatest_Generation,
            The_Silent_Generation,
            Baby_Boomer,
            Gen_X,
            Millenial,
            Gen_Z,
            Gen_Alpha
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