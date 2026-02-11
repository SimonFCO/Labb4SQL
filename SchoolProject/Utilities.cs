using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject
{
    internal class Utilities
    {
        public static int GetUserNumberMinMax(int min = -1000000, int max = 1000000)
        {
            int input;
            while (!int.TryParse(Console.ReadLine(), out input) || input < min || input > max)
            {
                Console.WriteLine($"Du måste ange ett tal mellan {min}-{max}");
            }
            return input;
        }
    }
}
