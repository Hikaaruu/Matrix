using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kursovaja.Classes
{
    class FadeefMethod : BaseMethod
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

        //возвращает единичную матрицу заданого размера
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

        //реалзация метода Фадеева
        public static List<double> Calculate(Matrix input, out List<Matrix> vectors, double step, ProgressBar progress)
        {
            Matrix mat_for_vec = new Matrix(input.Rows, input.Columns - 1);

            double[] polinom = FindPolinom(input, out mat_for_vec);

            List<double> roots = SolvePolinom(polinom, step, progress);

            vectors = FindVerctors(roots.ToArray(), mat_for_vec);

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = NormalizeVec(vectors[i]);
            }

            return roots;
        }

    }
}
