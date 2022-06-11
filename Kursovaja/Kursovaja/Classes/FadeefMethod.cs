using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursovaja.Classes
{
    class FadeefMethod
    {
        //ищет полином для последующего обчисления собственных значений матрицы
        private static double[] FindPolinom(Matrix input, out Matrix mat_for_vec)
        {
            mat_for_vec = new Matrix(input.Rows, input.Columns - 1);
            double[] result = new double[input.Columns];
            Matrix A_const = new Matrix(input.Rows, input.Columns);
            Matrix A_temp = new Matrix(input.Rows, input.Columns);
            Matrix B = new Matrix(input.Rows, input.Columns);
            double p;

            Matrix E = IdentityMatrix(input.Rows);
            A_const = (Matrix)input.Clone();
            p = DiagSum(A_const);
            B = A_const - E * p;

            result[0] = p;

            for (int i = 0; i < input.Rows; i++)
            {
                mat_for_vec[i, 0] = B[i, 0];
            }

            for (int i = 1; i < input.Rows; i++)
            {
                A_temp = A_const * B;
                p = (1.0 / (i + 1)) * DiagSum(A_temp);

                result[i] = p;

                if (i != input.Rows - 1)
                {
                    B = A_temp - E * p;

                    for (int j = 0; j < input.Rows; j++)
                    {
                        mat_for_vec[j, i] = B[j, 0];
                    }
                }

            }

            return result;
        }

        //ищет собственные векторы функции 
        private static List<Matrix> FindVerctors(double[] roots, Matrix mat_for_vec)
        {
            List<Matrix> result = new List<Matrix>();

            Matrix start_vec = new Matrix(mat_for_vec.Rows, 1);
            start_vec[0, 0] = 1;

            Matrix y = new Matrix(mat_for_vec.Rows, 1);


            Matrix b = new Matrix(mat_for_vec.Rows, 1);

            for (int i = 0; i < roots.Length; i++)
            {
                y = start_vec;
                for (int j = 0; j < mat_for_vec.Rows - 1; j++)
                {
                    for (int g = 0; g < mat_for_vec.Rows; g++)
                    {
                        b[g, 0] = mat_for_vec[g, j];
                    }

                    y = y * roots[i] + b;
                }

                result.Add(y);
            }

            return result;

        }

        //находит сумму диагоналей матрицы
        private static double DiagSum(Matrix a)
        {
            double sum = 0;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        sum += a[i, j];
                    }
                }
            }

            return sum;
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

        private static double FuncValue(double[] solved_system, double x)
        {
            double result = Math.Pow(x, solved_system.Length);
            for (int i = 0; i < solved_system.Length; i++)
            {
                result -= Math.Pow(x, solved_system.Length - 1 - i) * solved_system[i];
            }

            return result;
        }

        private static Matrix IdentityMatrix(int size)
        {
            Matrix result = new Matrix(size, size);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = 1;
                    }
                }
            }

            return result;
        }

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

        //реализация метода фадеева
        public static List<double> Calculate(Matrix input, out List<Matrix> vectors)
        {
            Matrix mat_for_vec = new Matrix(input.Rows, input.Columns - 1);

            double[] polinom = FindPolinom(input, out mat_for_vec);

            List<double> roots = SolvePolinom(polinom);

            vectors = FindVerctors(roots.ToArray(), mat_for_vec);

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = NormalizeVec(vectors[i]);
            }

            return roots;
        }
    }
}
