using System;

namespace Kursovaja.Classes
{
    internal class Matrix : ICloneable
    {
        #region Constructors
        public Matrix(double[,] values)
        {
            Values = values;
            Rows = values.GetLength(0);
            Columns = values.GetLength(1);
        }

        public Matrix(int n)
        {
            Rows = n;
            Columns = n;
            Values = new double[Rows, Columns];
            this.ProcessFunctionOverData((i, j) => this.Values[i, j] = 0);
        }

        public Matrix(int rows, int columns)
        {
            if (rows <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rows));
            }

            if (columns <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(columns));
            }

            Rows = rows;
            Columns = columns;
            Values = new double[rows, columns];

            this.ProcessFunctionOverData((i, j) => this.Values[i, j] = 0);
        }
        #endregion

        #region Properties
        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public double[,] Values { get; private set; }
        #endregion

        #region Operators
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Columns != right.Rows)
            {
                throw new InvalidOperationException();
            }

            Matrix result = new Matrix(left.Rows, right.Columns);

            for (int i = 0; i < left.Rows; i++)
            {
                for (int st2 = 0; st2 < right.Columns; st2++)
                {
                    for (int j = 0; j < right.Rows; j++)
                    {
                        result.Values[i, st2] += left.Values[i, j] * right.Values[j, st2];
                    }
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix left, double right)
        {
            Matrix result = new Matrix(left.Rows, left.Columns);
            result.ProcessFunctionOverData
                ((i, j) => result.Values[i, j] = left.Values[i, j] * right);

            return result;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows || left.Columns != right.Columns)
            {
                throw new InvalidOperationException();
            }

            Matrix result = new Matrix(left.Rows, left.Columns);

            result.ProcessFunctionOverData((i, j) =>
                result.Values[i, j] = left.Values[i, j] + right.Values[i, j]);

            return result;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows || left.Columns != right.Columns)
            {
                throw new InvalidOperationException();
            }

            Matrix result = new Matrix(left.Rows, left.Columns);

            result.ProcessFunctionOverData((i, j) =>
                result.Values[i, j] = left.Values[i, j] - right.Values[i, j]);

            return result;
        }

        public double this[int x, int y]
        {
            get
            {
                return Values[x, y];
            }
            set
            {
                Values[x, y] = value;
            }
        }
        #endregion

        #region Functions
        public void ProcessFunctionOverData(Action<int, int> func)
        {
            for (var i = 0; i < this.Rows; i++)
            {
                for (var j = 0; j < this.Columns; j++)
                {
                    func(i, j);
                }
            }
        }

        public Matrix CreateMatrixWithoutColumn(int column)
        {
            if (column < 0 || column >= this.Columns)
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(this.Rows, this.Columns - 1);
            result.ProcessFunctionOverData((i, j) =>
                result.Values[i, j] = j < column ? this.Values[i, j] : this.Values[i, j + 1]);
            return result;
        }

        public Matrix CreateMatrixWithoutRow(int row)
        {
            if (row < 0 || row >= this.Rows)
            {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(this.Rows - 1, this.Columns);
            result.ProcessFunctionOverData((i, j) =>
                result.Values[i, j] = i < row ? this.Values[i, j] : this.Values[i + 1, j]);
            return result;
        }

        public bool IsSquare => Rows == Columns;

        public double CalculateDeterminant()
        {
            if (!this.IsSquare)
            {
                throw new InvalidOperationException(
                    "determinant can be calculated only for square matrix");
            }
            if (this.Rows == 2)
            {
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            }
            double result = 0;
            for (var j = 0; j < this.Rows; j++)
            {
                result += (j % 2 == 1 ? 1 : -1) * this[1, j] *
                    this.CreateMatrixWithoutColumn(j).
                    CreateMatrixWithoutRow(1).CalculateDeterminant();
            }
            return result;
        }

        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write(string.Format("{0:0.00}", this[i, j]).ToString().PadRight(7));
                }
                Console.WriteLine();
            }
        }

        public object Clone()
        {
            Matrix result = new Matrix(Rows, Columns);
            result.ProcessFunctionOverData((i, j) => result[i, j] = Values[i, j]);
            return result;
        }

        #endregion
    }

}
