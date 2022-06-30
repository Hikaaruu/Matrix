using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Kursovaja.Classes
{
    class KrilovMethod : BaseMethod
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

        //полная реализация метода Крылова
        public static List<double> Calculate(Matrix input, out List<Matrix> vectors, double step, ProgressBar progress, Label label)
        {
            progress.Value = progress.Minimum;

            progress.Visible = true;

            label.Visible = true;

            label.Text = "підготовка...";

            Application.DoEvents();

            Matrix system = FindSystem(input);

            double[] solved_system = SolveSystem(system);

            label.Text = "пошук коренів...";

            Application.DoEvents();

            List<double> roots = SolvePolinom(solved_system, step, progress);

            label.Text = "пошук векторів...";

            Application.DoEvents();

            vectors = FindVectors(solved_system, roots, system);

            for (int i = 0; i < vectors.Count; i++)
            {
                vectors[i] = NormalizeVec(vectors[i]);
            }

            label.Visible = false;
            progress.Visible = false;

            return roots;
        }

    }

}
