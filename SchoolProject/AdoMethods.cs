using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace SchoolProject
{
    internal class AdoMethods
    {
        private static readonly string _connString = "Server=localhost;Database=SchoolDatabase;Integrated Security=True;TrustServerCertificate=True;";

        internal static void ManageStaff()
        {
            Console.Clear();
            Console.WriteLine("1. Visa Personal");
            Console.WriteLine("2. Ny anställd");
            if (Console.ReadLine() == "1")
            {
                ExecuteQuery("SELECT s.FirstName + ' ' + s.LastName AS Namn, r.RoleName AS Roll, DATEDIFF(YEAR, s.HireDate, GETDATE()) AS Arbetsår FROM Staff s JOIN Role r ON s.RoleId = r.RoleId");
            }
            else
            {
                using var connection = new SqlConnection(_connString);
                connection.Open();

                try
                {
                    string roleId = SelectFromList(connection, "SELECT RoleId, RoleName FROM Role", "Välj Roll");
                    string departmentId = SelectFromList(connection, "SELECT DepartmentId, DepartmentName FROM Department", "Välj Avdelning");

                    Console.Write("Förnamn: "); string firstName = Console.ReadLine();
                    Console.Write("Efternamn: "); string lastName = Console.ReadLine();
                    Console.Write("Lön: "); decimal.TryParse(Console.ReadLine(), out decimal salary);

                    string query = "INSERT INTO Staff (FirstName, LastName, RoleId, DepartmentId, Salary, HireDate) VALUES (@Fn, @Ln, @Rid, @Did, @Sal, GETDATE())";
                    using var cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Fn", firstName);
                    cmd.Parameters.AddWithValue("@Ln", lastName);
                    cmd.Parameters.AddWithValue("@Rid", roleId);
                    cmd.Parameters.AddWithValue("@Did", departmentId);
                    cmd.Parameters.AddWithValue("@Sal", salary);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Personal tillagd.");
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine("Fel: " + ex.Message);
                }
                Pause();
            }
        }

        internal static void ShowStudentGrades()
        {
            Console.Clear();
            using var connection = new SqlConnection(_connString);
            try
            {
                connection.Open();
                string studentId = SelectFromList(connection, "SELECT StudentId, FirstName + ' ' + LastName FROM Student", "Välj Elev");

                Console.WriteLine($"\n---- Betyg för elev {studentId} ----");
                string query = @"SELECT sub.SubjectName AS Ämne, g.GradeValue AS Betyg, g.DateGiven AS Datum, st.FirstName + ' ' + st.LastName AS Lärare
                                 FROM Grade g
                                 JOIN Student s ON g.StudentId = s.StudentId
                                 JOIN Subject sub ON g.SubjectId = sub.SubjectId
                                 JOIN Staff st ON g.TeacherId = st.StaffId
                                 WHERE g.StudentId = @studentId";

                using var Command = new SqlCommand(query, connection);
                Command.Parameters.AddWithValue("@studentId", studentId);

                using var reader = Command.ExecuteReader();
                PrintResults(reader); 
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
            Pause();
        }

        internal static void ShowSalaryStats()
        {
            Console.Clear();
            ExecuteQuery(@"SELECT d.DepartmentName AS Avdelning, SUM(s.Salary) AS TotalLön, AVG(s.Salary) AS SnittLön
                           FROM Staff s JOIN Department d ON s.DepartmentId = d.DepartmentId GROUP BY d.DepartmentName");
        }

        internal static void GetStudentInfoSP()
        {
            Console.Clear();
            using var connection = new SqlConnection(_connString);
            try
            {
                string studentId = "Fail";
                Console.WriteLine("1. Välj från lista");
                Console.WriteLine("2. Skriv in Personnummer");
                int answer = Utilities.GetUserNumberMinMax(1,2);
                if(answer == 1)
                {
                    connection.Open();
                    studentId = SelectFromList(connection, "SELECT StudentId, FirstName + ' ' + LastName FROM Student", "Välj Elev");
                }
                else if(answer == 2)
                {
                    connection.Open();
                    studentId = Console.ReadLine();
                }
                

                Console.WriteLine($"\n--- Elevens ID/Personnummer: {studentId} ---");

                using var cmd = new SqlCommand("GetStudentInfo", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                using var reader = cmd.ExecuteReader();

                if (!reader.HasRows) Console.WriteLine("Ingen ytterligare info hittades.");

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.WriteLine($"{reader.GetName(i)}: {reader.GetValue(i)}");
                }
            }
            catch (Exception ex) { Console.WriteLine("Fel: " + ex.Message); }
            Pause();
        }

        internal static void SetGradeTransaction()
        {
            Console.Clear();
            using var connection = new SqlConnection(_connString);
            try
            {
                connection.Open();

                string studentId = SelectFromList(connection, "SELECT StudentId, FirstName + ' ' + LastName FROM Student", "Välj Elev");

                string teacherSql = "SELECT s.StaffId, s.FirstName + ' ' + s.LastName FROM Staff s JOIN Role r ON s.RoleId = r.RoleId WHERE r.RoleName LIKE '%Teacher%' OR r.RoleName LIKE '%Lärare%'";
                string teacherId = SelectFromList(connection, teacherSql, "Välj Lärare");

                string subjectId = SelectFromList(connection, "SELECT SubjectId, SubjectName FROM Subject", "Välj Ämne");

                Console.Write("Ange Betyg (A-F): ");
                string grade = Console.ReadLine()?.ToUpper();

                using var trans = connection.BeginTransaction();
                using var cmd = new SqlCommand("INSERT INTO Grade (StudentId, TeacherId, SubjectId, GradeValue, DateGiven) VALUES (@sid, @tid, @sub, @val, GETDATE())", connection, trans);

                cmd.Parameters.AddWithValue("@sid", studentId);
                cmd.Parameters.AddWithValue("@tid", teacherId);
                cmd.Parameters.AddWithValue("@sub", subjectId);
                cmd.Parameters.AddWithValue("@val", grade ?? (object)DBNull.Value);

                try
                {
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                    Console.WriteLine("Betyg registrerat!");
                }
                catch
                {
                    trans.Rollback();
                    throw; 
                }
            }
            catch (Exception ex) { Console.WriteLine("Fel: " + ex.Message); }
            Pause();
        }


        public static void ExecuteQuery(string query)
        {
            using var connection = new SqlConnection(_connString);
            try
            {
                connection.Open();
                using var cmd = new SqlCommand(query, connection);
                using var reader = cmd.ExecuteReader();
                PrintResults(reader);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            Pause();
        }

        private static void PrintResults(SqlDataReader reader)
        {
            if (!reader.HasRows) { Console.WriteLine("Inga resultat."); return; }

            Console.WriteLine(new string('-', 60));
            for (int i = 0; i < reader.FieldCount; i++) Console.Write($"{reader.GetName(i),-20}");
            Console.WriteLine("\n" + new string('-', 60));

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++) Console.Write($"{reader.GetValue(i),-20}");
                Console.WriteLine();
            }
        }

        private static string SelectFromList(SqlConnection conn, string query, string header)
        {
            Console.WriteLine($"\n--- {header} ---");
            var ids = new List<string>();

            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                int count = 1;
                while (reader.Read())
                {
                    ids.Add(reader[0].ToString());
                    Console.WriteLine($"{count}. {reader[1]}");
                    count++;
                }
            }

            Console.Write("Ange nummer: ");
            int index = Utilities.GetUserNumberMinMax(1, ids.Count);
            return ids[index - 1];
        }

        private static void Pause()
        {
            Console.WriteLine("\nTryck Enter...");
            Console.ReadLine();
        }
    }
}