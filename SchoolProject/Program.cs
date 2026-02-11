using SchoolProject.Models;

namespace SchoolProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            using (var context = new SchoolDatabaseContext())
            {
                Ui.ShowMenu(context);
            }
        }
    }
}