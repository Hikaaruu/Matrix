using Kursovaja.Structs;
using System.Collections.Generic;

namespace Kursovaja.Classes
{
    internal static class TransferData
    {
        public static List<double> Roots { get; set; }

        public static List<Matrix> Vectors { get; set; }

        public static int IterationsCount { get; set; }

        public static int StepsCount { get; set; }

        public static bool AnalyticData { get; set; }

        public static List<Point> Points { get; set; }
    }
}
