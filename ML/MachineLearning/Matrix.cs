using System;

namespace MachineLearning
{
    // create a class to perform matrix operations
    public class Matrix
    {
        // create a method to concatenate two matricies
        public static double[,] Concatenate(double[,] a, double[,] b)
        {
            // create a matrix to hold the concatenated matrix
            var c = new double[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            // loop through the rows of the first matrix
            for (int i = 0; i < a.GetLength(0); i++)
            {
                // loop through the columns of the first matrix
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    // set the value of the concatenated matrix
                    c[i, j] = a[i, j];
                }
            }
            // loop through the rows of the second matrix
            for (int i = 0; i < b.GetLength(0); i++)
            {
                // loop through the columns of the second matrix
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    // set the value of the concatenated matrix
                    c[i, j + a.GetLength(1)] = b[i, j];
                }
            }
            // return the concatenated matrix
            return c;
        }

        // create a function to delete a column from a matrix
        public static double[,] DeleteColumn(double[,] a, int column)
        {
            // create a matrix to hold the new matrix
            var b = new double[a.GetLength(0), a.GetLength(1) - 1];
            // loop through the rows of the matrix
            for (int i = 0; i < a.GetLength(0); i++)
            {
                // loop through the columns of the matrix
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    // if the column is not the one to be deleted
                    if (j != column)
                    {
                        // set the value of the new matrix
                        b[i, j < column ? j : j - 1] = a[i, j];
                    }
                }
            }
            // return the new matrix
            return b;
        }

        // create a function to extract a column from a matrix
        public static double[] ExtractColumn(double[,] a, int column)
        {
            // create a vector to hold the column
            var b = new double[a.GetLength(0)];
            // loop through the rows of the matrix
            for (int i = 0; i < a.GetLength(0); i++)
            {
                // set the value of the vector
                b[i] = a[i, column];
            }
            // return the vector
            return b;
        }

        // create a function to extract multiple columns from a matrix
        public static double[,] ExtractColumns(double[,] a, params int[] columns)
        {
            // create a matrix to hold the columns
            var b = new double[a.GetLength(0), columns.Length];

            // loop through the columns of the matrix
            for (int j = 0; j < columns.Length; j++)
            {
                int column = columns[j];
                // loop through the rows of the matrix
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    // set the value of the matrix
                    b[i, j] = a[i, column];
                }
            }

            // return the matrix
            return b;
        }

        // create a function to extract a row from a matrix
        public static double[] ExtractRow(double[,] a, int row)
        {
            // create a vector to hold the row
            var b = new double[a.GetLength(1)];
            // loop through the columns of the matrix
            for (int i = 0; i < a.GetLength(1); i++)
            {
                // set the value of the vector
                b[i] = a[row, i];
            }
            // return the vector
            return b;
        }

        // create a function to create a matrix from a vector
        public static double[,] FromVector(double[] a)
        {
            // create a matrix to hold the vector
            var b = new double[a.GetLength(0), 1];
            // loop through the vector
            for (int i = 0; i < a.GetLength(0); i++)
            {
                // set the value of the matrix
                b[i, 0] = a[i];
            }
            // return the matrix
            return b;
        }

        // create a method to multiply two matrices
        public static double[,] Multiply(double[,] a, double[,] b)
        {
            // get the number of rows in the first matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the first matrix
            int aCols = a.GetLength(1);
            // get the number of rows in the second matrix
            int bRows = b.GetLength(0);
            // get the number of columns in the second matrix
            int bCols = b.GetLength(1);

            // check if the number of columns in the first matrix is equal to the number of rows in the second matrix
            if (aCols != bRows)
            {
                // if not, throw an exception
                throw new Exception("The number of columns in the first matrix must be equal to the number of rows in the second matrix.");
            }

            // create a new matrix to hold the result
            double[,] result = new double[aRows, bCols];

            // loop through the rows in the first matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the second matrix
                for (int j = 0; j < bCols; j++)
                {
                    // loop through the rows in the second matrix
                    for (int k = 0; k < bRows; k++)
                    {
                        // multiply the values in the first matrix by the values in the second matrix
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            }

            // return the result
            return result;
        }

        // create a method to transpose a matrix
        public static double[,] Transpose(double[,] a)
        {

            // get the number of rows in the matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the matrix
            int aCols = a.GetLength(1);

            // create a new matrix to hold the result
            double[,] result = new double[aCols, aRows];

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // transpose the values in the matrix
                    result[j, i] = a[i, j];
                }
            }

            // return the result
            return result;
        }

        // create a method to subtract two matrices
        public static double[,] Subtract(double[,] a, double[,] b)
        {

            // get the number of rows in the first matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the first matrix
            int aCols = a.GetLength(1);
            // get the number of rows in the second matrix
            int bRows = b.GetLength(0);
            // get the number of columns in the second matrix
            int bCols = b.GetLength(1);

            // check if the number of rows in the first matrix is equal to the number of rows in the second matrix
            if (aRows != bRows)
            {
                // if not, throw an exception
                throw new Exception("The number of rows in the first matrix must be equal to the number of rows in the second matrix.");
            }

            // check if the number of columns in the first matrix is equal to the number of columns in the second matrix
            if (aCols != bCols)
            {
                // if not, throw an exception
                throw new Exception("The number of columns in the first matrix must be equal to the number of columns in the second matrix.");
            }

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // loop through the rows in the first matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the first matrix
                for (int j = 0; j < aCols; j++)
                {
                    // subtract the values in the second matrix from the values in the first matrix
                    result[i, j] = a[i, j] - b[i, j];
                }
            }

            // return the result
            return result;
        }

        // create a method to add two matrices
        public static double[,] Add(double[,] a, double[,] b)
        {
            // get the number of rows in the first matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the first matrix
            int aCols = a.GetLength(1);
            // get the number of rows in the second matrix
            int bRows = b.GetLength(0);
            // get the number of columns in the second matrix
            int bCols = b.GetLength(1);

            // check if the number of rows in the first matrix is equal to the number of rows in the second matrix
            if (aRows != bRows)
            {
                // if not, throw an exception
                throw new Exception("The number of rows in the first matrix must be equal to the number of rows in the second matrix.");
            }

            // check if the number of columns in the first matrix is equal to the number of columns in the second matrix
            if (aCols != bCols)
            {
                // if not, throw an exception
                throw new Exception("The number of columns in the first matrix must be equal to the number of columns in the second matrix.");
            }

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // loop through the rows in the first matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the first matrix
                for (int j = 0; j < aCols; j++)
                {
                    // add the values in the first matrix to the values in the second matrix
                    result[i, j] = a[i, j] + b[i, j];
                }
            }

            // return the result
            return result;
        }

        // create a method to multiply a matrix by an array
        public static double[] Multiply(double[,] a, double[] b)
        {

            // get the number of rows in the matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the matrix
            int aCols = a.GetLength(1);

            // check if the number of columns in the matrix is equal to the number of elements in the array
            if (aCols != b.Length)
            {
                // if not, throw an exception
                throw new Exception("The number of columns in the matrix must be equal to the number of elements in the array.");
            }

            // create a new array to hold the result
            double[] result = new double[aRows];

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // multiply the values in the matrix by the values in the array
                    result[i] += a[i, j] * b[j];
                }
            }

            // return the result
            return result;
        }

        // create a method to multiply a matrix by a scalar
        public static double[,] Multiply(double[,] a, double b)
        {

            // get the number of rows in the matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the matrix
            int aCols = a.GetLength(1);

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // multiply the values in the matrix by the scalar
                    result[i, j] = a[i, j] * b;
                }
            }

            // return the result
            return result;
        }

        // create a method to inverse a matrix
        public static double[,] Inverse(double[,] a)
        {
            int rows = a.GetLength(0);
            int cols = a.GetLength(1);
            if (rows != cols)
                throw new Exception("Matrix must be square");

            double[,] ret = new double[rows, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    ret[i, j] = (i == j) ? 1 : 0;
                }
            }
            for (int i = 0; i < rows; i++)
            {
                double pivot = a[i, i];
                if (pivot == 0)
                {
                    for (int j = i + 1; j < rows; j++)
                    {
                        if (a[j, i] != 0)
                        {
                            for (int k = 0; k < rows; k++)
                            {
                                double t = a[i, k];
                                a[i, k] = a[j, k];
                                a[j, k] = t;
                                t = ret[i, k];
                                ret[i, k] = ret[j, k];
                                ret[j, k] = t;
                            }
                            pivot = a[i, i];
                            break;
                        }
                    }
                }

                if (pivot == 0)
                    throw new Exception("Matrix is singular");

                for (int j = 0; j < rows; j++)
                {
                    a[i, j] /= pivot;
                    ret[i, j] /= pivot;
                }

                for (int j = 0; j < rows; j++)
                {
                    if (i != j)
                    {
                        double multiplier = a[j, i];
                        for (int k = 0; k < rows; k++)
                        {
                            a[j, k] -= multiplier * a[i, k];
                            ret[j, k] -= multiplier * ret[i, k];
                        }
                    }
                }
            }

            return ret;
        }

        // create a method to create an identity matrix
        public static double[,] Identity(int aRows, int aCols)
        {

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // check if the current row is equal to the current column
                    if (i == j)
                    {
                        // if so, set the value to one
                        result[i, j] = 1;
                    }
                    else
                    {
                        // if not, set the value to zero
                        result[i, j] = 0;
                    }
                }
            }

            // return the result
            return result;
        }

        // create a method to create a matrix filled with zeros
        public static double[,] Zeros(int aRows, int aCols)
        {

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // return the result
            return result;
        }

        // create a method to create a matrix filled with ones
        public static double[,] Ones(int aRows, int aCols)
        {

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // set the value to one
                    result[i, j] = 1;
                }
            }

            // return the result
            return result;
        }

        // create a method to create a matrix filled with random values
        public static double[,] Random(int aRows, int aCols)
        {

            // create a new matrix to hold the result
            double[,] result = new double[aRows, aCols];

            // create a new random number generator
            Random random = new Random();

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // set the value to a random number between zero and one
                    result[i, j] = random.NextDouble();
                }
            }

            // return the result
            return result;
        }

        // create a method to print a matrix
        public static void Print(double[,] a)
        {

            // get the number of rows in the matrix
            int aRows = a.GetLength(0);
            // get the number of columns in the matrix
            int aCols = a.GetLength(1);

            // loop through the rows in the matrix
            for (int i = 0; i < aRows; i++)
            {
                // create a new string to hold the row
                string row = "";
                // loop through the columns in the matrix
                for (int j = 0; j < aCols; j++)
                {
                    // add the value in the matrix to the row
                    row += a[i, j] + " ";
                }
                // print the row
                Console.WriteLine(row);
            }
        }
    }
}
