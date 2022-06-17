using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursovaja.Classes
{
    internal abstract class BaseMethod
    {
        //приводит вектор к ортонормированой системе векторов
        protected static Matrix NormalizeVec(Matrix matrix)
        {
            Matrix result = (Matrix)matrix.Clone();

            double sum = 0;
            for (int i = 0; i < matrix.Rows; i++)
            {
                sum += Math.Pow(matrix[i, 0], 2);
            }
            sum = Math.Sqrt(sum);
            for (int i = 0; i < matrix.Rows; i++)
            {
                result[i, 0] = matrix[i, 0] / sum;
            }

            return result;
        }

        //находит наибольший по модулю элемент масива
        protected static double MaxAbsElement(double[] mas)
        {
            double max = Math.Abs(mas[0]);

            for (int i = 1; i < mas.Length; i++)
            {
                if (Math.Abs(mas[i]) > Math.Abs(max))
                {
                    max = Math.Abs(mas[i]);
                }
            }
            return max;
        }

        //возвращает значение функции при заданом 
        protected static double FuncValue(double[] solved_system, double x)
        {
            double result = Math.Pow(x, solved_system.Length);
            for (int i = 0; i < solved_system.Length; i++)
            {
                result -= Math.Pow(x, solved_system.Length - 1 - i) * solved_system[i];
            }

            return result;
        }

        //реализация метода половинного деления 
        protected static double BisectionMethod(double[] solved_system, double a, double b, double epsilon = 0.001)
        {

            double x1 = a;
            double x2 = b;
            double fx1;
            double fx2;
            double fb = FuncValue(solved_system, b);
            double midpt;
            double fmidpt;
            while (true)
            {
                TransferData.IterationsCount++;

                fx1 = FuncValue(solved_system, x1);
                fx2 = FuncValue(solved_system, x2);

                midpt = 0.5 * (x1 + x2);
                fmidpt = FuncValue(solved_system, midpt);

                if (Math.Abs(x2 - x1) < epsilon)
                {
                    return midpt;
                }
                if (fmidpt * fx1 < 0)
                {
                    x2 = midpt;
                }
                if (fmidpt * fx2 < 0)
                {
                    x1 = midpt;
                }
            }

        }

        // решает полином  методом половинного деления
        protected static List<double> SolvePolinom(double[] solved_system, double step, ProgressBar progress)
        {
            progress.Visible = true;

            double max_el = MaxAbsElement(solved_system);
            int degree = solved_system.Length;

            double start = -1 * max_el * degree;
            double end = max_el * degree;

            try
            {
                TransferData.StepsCount = Convert.ToInt32((end * 2) / step);
            }
            catch (Exception)
            {
                TransferData.StepsCount = 0;
            }


            double x0 = FuncValue(solved_system, start - step);

            List<double> result_x0 = new List<double>();
            List<double> result_x1 = new List<double>();

            List<double> result = new List<double>();

            double x1;

            bool wide_graph = step * 10 > 10;

            TransferData.Points = new List<Structs.Point>();

            for (double i = start; i < end; i += step)
            {
                x1 = FuncValue(solved_system, i);

                if (wide_graph && i >= -10 * step && i <= 10 * step)
                {
                    TransferData.Points.Add(new Structs.Point(i, x1));
                }
                else
                {
                    if (i >= -10 && i <= 10)
                    {
                        TransferData.Points.Add(new Structs.Point(i, x1));
                    }
                }


                if (x0 * x1 < 0)
                {
                    result_x0.Add(i - step);
                    result_x1.Add(i);
                }

                x0 = x1;
                Console.WriteLine((i - start) / (end - start) * 100 + " %");
                progress.Value = Convert.ToInt32((i - start) / (end - start) * 100) < 1 ? 1 : Convert.ToInt32((i - start) / (end - start) * 100);
            }

            #region calculate dop points
            TransferData.DopPoints = new List<Structs.Point>();
            try
            {
                for (double i = -10; i <= 10; i += 0.01)
                {
                    x1 = FuncValue(solved_system, i);
                    TransferData.DopPoints.Add(new Structs.Point(i, x1));
                }
            }
            catch (Exception)
            {

            }

            #endregion


            for (int i = 0; i < result_x0.Count; i++)
            {
                result.Add(BisectionMethod(solved_system, result_x0[i], result_x1[i]));
            }

            progress.Value = 100;

            return result;
        }



    }
}
