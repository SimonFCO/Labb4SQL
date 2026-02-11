using SchoolProject.Models;
using System;

namespace SchoolProject
{
    internal class Ui
    {
        public static void ShowMenu(SchoolDatabaseContext context)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SKOLSYSTEM ===");
                Console.WriteLine("1.    Lärare per avdelning");
                Console.WriteLine("2.    Visa alla elever");
                Console.WriteLine("3.    Visa kurser");
                Console.WriteLine("4.    Personalöversikt & Lägg till");
                Console.WriteLine("5.    Elevens betyg");
                Console.WriteLine("6.    Lönestatistik");
                Console.WriteLine("7.    Hämta info");
                Console.WriteLine("8.    Sätt betyg");
                Console.WriteLine("9.    Avsluta");
                Console.Write("Val: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": 
                        EfMethods.ShowTeachersPerDepartment(context); 
                        break;
                    case "2": 
                        EfMethods.ShowAllStudents(context); 
                        break;
                    case "3": 
                        EfMethods.ShowActiveCourses(context); 
                        break;
                    case "4": 
                        AdoMethods.ManageStaff(); break;
                    case "5": 
                        AdoMethods.ShowStudentGrades(); 
                        break;
                    case "6": 
                        AdoMethods.ShowSalaryStats(); 
                        break;
                    case "7": 
                        AdoMethods.GetStudentInfoSP(); 
                        break;
                    case "8": 
                        AdoMethods.SetGradeTransaction(); 
                        break;
                    case "9": 
                        return;
                    default: 
                        Console.WriteLine("Ogiltigt val."); 
                        break;
                }
            }
        }
    }
}