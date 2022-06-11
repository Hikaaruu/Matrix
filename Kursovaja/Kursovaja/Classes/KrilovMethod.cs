using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaja.Classes
{
    class KrilovMethod
    {
        // ищет начальный вектор
        private static Matrix CreateStartVector(Matrix m)
        {
            Matrix result = new Matrix(m.Rows, 1);
            result[0, 0] = 1;
            return result;
        }

        //ищeт систему уравнений и возвращает её в виде матрицы 
        private static Matrix FindSystem(Matrix a)
        {
            Matrix result = new Matrix(a.Rows, a.Columns + 1);

            Matrix vec = CreateStartVector(a);

            for (int i = 0; i < result.Columns; i++)
            {
                for (int j = 0; j < a.Rows; j++)
                {
                    if (i == result.Columns - 1)
                    {
                        result[j, result.Columns - 1] = vec[j, 0];
                    }
                    else
                    {
                        result[j, result.Columns - i - 2] = vec[j, 0];
                    }

                }
                vec = a * vec;
            }

            return result;
        }

        //приводит вектор к ортонормированой системе векторов
        private static Matrix NormalizeVec(Matrix matrix)
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

        //решает систему уравнений методом Крамера
        private static double[] SolveSystem(Matrix a)
        {
            Matrix system = a.CreateMatrixWithoutColumn(a.Columns - 1);
            Matrix temp_system;

            Matrix vec = new Matrix(a.Rows, 1);
            for (int i = 0; i < a.Rows; i++)
            {
                vec[i, 0] = a[i, a.Columns - 1];
            }

            double[] result = new double[system.Columns];

            double main_det = system.CalculateDeterminant();
            double temp_det;

            for (int j = 0; j < system.Columns; j++)
            {
                temp_system = (Matrix)system.Clone();
                for (int i = 0; i < system.Columns; i++)
                {
                    temp_system[i, j] = vec[i, 0];
                }
                temp_det = temp_system.CalculateDeterminant();
                result[j] = temp_det / main_det;
            }

            return result;
        }

        //возвращает значение функции при заданом 
        private static double FuncValue(double[] solved_system, double x)
        {
            double result = Math.Pow(x, solved_system.Length);
            for (int i = 0; i < solved_system.Length; i++)
            {
                result -= Math.Pow(x, solved_system.Length - 1 - i) * solved_system[i];
            }

            return result;
        }

        // решает полином  методом половинного деления
        private static List<double> SolvePolinom(double[] solved_system)
        {

            const double step = 0.1;
            const double start = -1000;
            const double end = 1000;


            double x0 = FuncValue(solved_system, start - step);

            List<double> result_x0 = new List<double>();
            List<double> result_x1 = new List<double>();

            List<double> result = new List<double>();

            double x1;
            for (double i = start; i < end; i += step)
            {
                x1 = FuncValue(solved_system, i);

                if (x0 * x1 < 0)
                {
                    result_x0.Add(i - step);
                    result_x1.Add(i);
                }

                x0 = x1;
                Console.WriteLine((i - start) / (end - start) * 100 + " %");
            }


            for (int i = 0; i < result_x0.Count; i++)
            {
                result.Add(BisectionMethod(solved_system, result_x0[i], result_x1[i]));
            }

            return result;
        }

        //реализация метода половинного деления 
        private static double BisectionMethod(double[] solved_system, double a, double b, double epsilon = 0.0001)
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
                fx1 = FuncValue(solved_system, x1);
                fx2 = FuncValue(solved_system, x2);

                midpt = 0.5 * (x1 + x2);
                fmidpt = FuncValue(solved_system, midpt);

                if (Math.Abs(fmidpt) < epsilon)
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

        // ищет собственные векторы матрицы
        private static List<Matrix> FindVectors(double[] solved_system, List<double> roots, Matrix system)
        {
            List<Matrix> result = new List<Matrix>();

            Matrix y = new Matrix(solved_system.Length, 1);

            double q;

            //цикл по собс. значениям 
            for (int i = 0; i < roots.Count; i++)
            {
                q = 1;
                result.Add(new Matrix(solved_system.Length, 1));
                //инициализация начальных значений вектора
                for (int g = 0; g < solved_system.Length; g++)
                {
                    result[i][g, 0] = system[g, 0];
                }

                //цикл по q
                for (int j = 0; j < solved_system.Length - 1; j++)
                {
                    for (int t = 0; t < solved_system.Length; t++)
                    {
                        y[t, 0] = system[t, j + 1];
                    }


                    q = roots[i] * q - solved_system[j];
                    result[i] += y * q;
                }
            }


            return result;
        }

        //полная реализация метода крылова
        public static List<double> Calculate(Matrix input, out List<Matrix> vectors)
        {
            Matrix system = FindSystem(input);

            double[] solved_system = SolveSystem(system);

            List<double> roots = SolvePolinom(solved_system);

            vectors = FindVectors(solved_system, roots, system);

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = NormalizeVec(vectors[i]);
            }

            return roots;
        }

    }

}
