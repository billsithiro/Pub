using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Pub
{
    // cretae a class to perform multiple linear regression with matricies
    public class MLR
    {      
        // create a function to get the beta coefficients of a multiple linear regression 
        public static double[] Calculate(double[,] x, double[] y)
        {
            // create a matrix holding only one column containing ones
            double[,] ones = Matrix.Ones(x.GetLength(0), 1);
            // insert the ones into the input matrix
            double[,] X = Matrix.Concatenate(ones, x);
            // create a matrix to hold the transpose of the X
            double[,] Xt = Matrix.Transpose(X);
            // create a matrix to hold the product of the Xt and X
            double[,] XtX = Matrix.Multiply(Xt, X);
            // create a matrix to hold the inverse of the XtX
            double[,] XtXInv = Matrix.Inverse(XtX);
            // create a matrix to hold the product of the Xt and y
            double[] Xty = Matrix.Multiply(Xt, y);
            // create a vector to hold the product of the XtXInv and Xty
            double[] betas = Matrix.Multiply(XtXInv, Xty);
            // return the coefficients a.k.a. betas
            return betas;
        }

        // create a function to predict yhat
        public static double[] Predict(double[,] x, double[] coefficients)
        {
            // create a matrix holding only one column containing ones
            double[,] ones = Matrix.Ones(x.GetLength(0), 1);
            // insert the ones into the input matrix
            double[,] X = Matrix.Concatenate(ones, x);
            // create a vector to hold the predictions
            double[] yhat = Matrix.Multiply(X, coefficients);
            // return the predictions
            return yhat;
        }
    }
}
