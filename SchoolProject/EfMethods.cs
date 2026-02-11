using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using System;
using System.Linq;

namespace SchoolProject
{
    internal class EfMethods
    {
        public static void ShowTeachersPerDepartment(SchoolDatabaseContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Antal Lärare per Avdelning ---");

            var stats = context.Departments
                .Select(d => new
                {
                    DeptName = d.DepartmentName,
                    Count = d.Staff.Count(s => s.Role.RoleName == "Teacher")
                })
                .ToList();

            foreach (var item in stats)
            {
                Console.WriteLine($"{item.DeptName}: {item.Count} st");
            }
            Pause();
        }

        public static void ShowAllStudents(SchoolDatabaseContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Alla Elever ---");

            var students = context.Students
                .OrderBy(s => s.FirstName)
                .Select(s => new
                {
                    FullName = s.FirstName + " " + s.LastName,
                    StudentId = s.StudentId,
                    ClassName = s.Class != null ? s.Class.ClassName : "Ingen klass"
                })
                .ToList();

            foreach (var s in students)
            {
                Console.WriteLine($"{s.FullName} (Klass: {s.ClassName}) - ID: {s.StudentId}");
            }
            Pause();
        }

        public static void ShowActiveCourses(SchoolDatabaseContext context)
        {
            Console.Clear();
            Console.WriteLine("--- Kurser ---");
            var subjects = context.Subjects
                .Select(s => new { s.SubjectId, s.SubjectName })
                .ToList();

            foreach (var sub in subjects)
            {
                Console.WriteLine($"ID {sub.SubjectId}: {sub.SubjectName}");
            }
            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nTryck Enter...");
            Console.ReadLine();
        }
    }
}