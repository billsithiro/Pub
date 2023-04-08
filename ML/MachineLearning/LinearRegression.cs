// add common namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MachineLearning
{    
    // create a class to perform time series forecasting
    public class TSLR
    {
        // create a field to hold the seasonality averages
        public readonly double[] StAvg;
        // create a field to hold the seasonal component
        public readonly double[] St;
        // create a field to hold the iregularotary component
        public readonly double[] It;
        // create a field to hold the trend line component
        public readonly double[] Tt;
        // create a field to hold the time units
        public readonly double[] Tu;
        // create a field to hold the coefficients of the simple linear regression
        public readonly double[] Coefs;
        // create a field to hold the predictions of the inputed series
        public readonly double[] Predicted;        
        // create a field to hold the length of data
        public readonly int Length;
        // create a field to hold the seasonality of the time series
        public readonly int Seasonality;

        // create a function to calculate a moving average with given window
        public static double[] MA(double[] values, int window)
        {
            var result = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                var start = Math.Max(0, i - window + 1);
                var count = Math.Min(window, i + 1);
                var sum = 0.0;
                for (int j = start; j < start + count; j++)
                {
                    sum += values[j];
                }
                result[i] = sum / count;
            }
            return result;
        }

        // create a function to calculate time series data and return components to be used in forecasting
        public TSLR(double[] data, int seasonality = -1)
        {           
            // set the length of our data
            Length = data.Length;

            // set the seasonality of our time series
            Seasonality = seasonality == -1 ? (int)Math.Round(Utilities.FFT(data).Max()) : seasonality;

            // smooth out data by performing a moving average
            double[] ma = MA(data, Seasonality);
            // perform a centered moving average if the data is even
            double[] cma = MA(ma, Seasonality);

            // we will be using the classic time series multiplicative model to forecast future values
            // Yt = St*It*Tt

            // derived seasonality and irregularity combined component (StIt)
            IEnumerable<double> stit = data.Select((a, i) => a / cma[i]);

            // we will need to remove the irregularity so as we are left only with the St component
            // to achieve this we will use a table that contains the average of each seasonality value
            StAvg = new double[Seasonality];
            double[] counter = new double[Seasonality];
            for (int i = 0; i < Length; i++)
            {
                int idx = i % Seasonality;
                StAvg[idx] += data[idx] / cma[idx];
                counter[idx]++;
            }
            StAvg = StAvg.Where(a => a != 0).Select((a, i) => a / counter[i]).ToArray();

            // set the St and It (deseasonalized) components
            St = new double[Length];
            It = new double[Length];
            for (int i = 0; i < Length; i++)
            {
                int idx = i % Seasonality;
                St[i] = StAvg[idx];
                It[i] = data[i] / St[i];
            }

            // set the trend line Tt component by performing a simple linear regression  
            // x will be the time units and y will be the It component
            Tu = Enumerable.Range(1, Length).Select(a => (double)a).ToArray();
            Coefs = SLR.Fit(Tu, It);
            Tt = SLR.Predict(Tu, Coefs);

            // forecast the input data for testing the quality of the calulations
            Predicted = Tt.Select((a, i) => a * St[i]).ToArray();            
        }

        // create a function to perform a series of forecasts into the future
        public double[] Forecast(int steps)
        {
            // set the starting point of the forecast
            int start = (int)Tu.Last() + 1;
            if (start > Length)
                start = 0;
            // hold the results
            double[] results = new double[steps];
            // counter to helps us index the results
            int cnt = 0;
            // loop through steps and perform a forecast
            for (int i = start; i < start + steps; i++)
            {
                int idx = i % Seasonality;
                double st = StAvg[idx];
                double tt = Coefs[0] * i + Coefs[1];
                results[cnt++] = st * tt;
            }
            // return the results
            return results;
        }

    }

    // create a class to perform simple linear regression calculations
    public class SLR
    {
        // create a function to calculate the coefs of a linear regression (m and b)
        public static double[] Fit(double[] x, double[] y)
        {
            double[] result = new double[2];
            double xSum = x.Sum();
            double ySum = y.Sum();
            double x2Sum = x.Select(d => d * d).Sum();
            double xySum = x.Select((t, i) => t * y[i]).Sum();
            double count = x.Length;
            double m = (count * xySum - xSum * ySum) / (count * x2Sum - xSum * xSum);
            double b = (ySum - m * xSum) / count;

            result[0] = m; // coef
            result[1] = b; // intercept

            return result;
        }

        // create function to predict y given calculated coefs
        public static double Predict(double x, double[] coefs)
        {
            // y = mx+b;
            return coefs[0] * x + coefs[1];
        }

        // create function to predict a y vector given calculated coefs
        public static double[] Predict(double[] x, double[] coefs)
        {
            // create array to hold results
            double[] results = new double[x.Length];
            // loop through vector and results
            for (int i = 0; i < x.Length; i++)
                results[i] = Predict(x[i], coefs);
            // return the results
            return results;
        }
    }

    // create a class to perform simple linear regression calculations w/ statistics
    public class SSLR
    {
        // create a function to calculate residuals
        public static double[] Residuals(double[] x, double[] y, double[] coefs)
        {
            // create array to hold results
            double[] results = new double[x.Length];
            // loop through vector and results
            for (int i = 0; i < x.Length; i++)
                results[i] = y[i] - SLR.Predict(x[i], coefs);
            // return the results
            return results;
        }
        // create a function to calculate the mean of a list of numbers
        public static double Mean(double[] values)
        {
            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }
            return sum / values.Length;
        }

        // create a function to calculate the standard deviation of a list of numbers
        public static double StandardDeviation(double[] values)
        {
            double mean = Mean(values);
            double sum = 0;
            foreach (double value in values)
            {
                sum += Math.Pow(value - mean, 2);
            }
            return Math.Sqrt(sum / (values.Length - 1));
        }

        // create a function to calculate the covariance between two lists of numbers
        public static double Covariance(double[] values1, double[] values2)
        {
            double mean1 = Mean(values1);
            double mean2 = Mean(values2);
            double sum = 0;
            for (int i = 0; i < values1.Length; i++)
            {
                sum += (values1[i] - mean1) * (values2[i] - mean2);
            }
            return sum / (values1.Length - 1);
        }

        // create a function to calculate the coefficients of a linear regression (m and b)
        public static double[] Fit(double[] values1, double[] values2)
        {
            double[] coefficients = new double[2];
            coefficients[0] = Covariance(values1, values2) / Math.Pow(StandardDeviation(values1), 2);
            coefficients[1] = Mean(values2) - coefficients[0] * Mean(values1);
            return coefficients;
        }

        // create function to predict y given and array of coefs (m and b)
        public static double Predict(double x, double[] coefs)
        {
            // y = mx+b;
            return coefs[0] * x + coefs[1];
        }

        // create function to predict y given a series of values and an array of coefs (m and b)
        public static double[] Predict(double[] values, double[] coefs)
        {
            // create array to hold results
            double[] results = new double[values.Length];
            // loop through vector and results
            for (int i = 0; i < values.Length; i++)
                results[i] = Predict(values[i], coefs);
            // return the results
            return results;
        }

        // create a function to calculate the root mean squared error of a linear regression
        public static double RMSE(double[] values1, double[] values2)
        {
            double sum = 0;
            for (int i = 0; i < values1.Length; i++)
            {
                double difference = values1[i] - values2[i];
                sum += Math.Pow(difference, 2);
            }
            return Math.Sqrt(sum / values1.Length);
        }

        // create a function to calculate the coefficient of determination of a linear regression
        public static double R2(double[] values1, double[] values2)
        {
            double mean = Mean(values2);
            double sum1 = 0;
            double sum2 = 0;
            for (int i = 0; i < values1.Length; i++)
            {
                sum1 += Math.Pow(values1[i] - values2[i], 2);
                sum2 += Math.Pow(values2[i] - mean, 2);
            }
            return 1 - sum1 / sum2;
        }

        // create a function to calculate the adjusted coefficient of determination of a linear regression
        public static double AdjustedR2(double[] values1, double[] values2, int numberOfPredictors)
        {
            double r2 = R2(values1, values2);
            return 1 - (1 - r2) * (values1.Length - 1) / (values1.Length - numberOfPredictors - 1);
        }

        // create a function to calculate the F-statistic of a linear regression
        public static double FStatistic(double[] values1, double[] values2, int numberOfPredictors)
        {
            double r2 = R2(values1, values2);
            return r2 / (1 - r2) * (values1.Length - numberOfPredictors - 1) / numberOfPredictors;
        }

        // create a function to calculate the p-value of a linear regression
        public static double PValue(double[] values1, double[] values2, int numberOfPredictors)
        {
            double f = FStatistic(values1, values2, numberOfPredictors);
            return 1 - FTest(f, numberOfPredictors, values1.Length - numberOfPredictors - 1);
        }

        // create a function to calculate the F-test of a linear regression
        public static double FTest(double f, int df1, int df2)
        {
            double p = 0;
            double x = df2 / (df2 + df1 * f);
            for (int i = 1; i <= df2 / 2; i++)
            {
                p += Math.Pow(-1, i + 1) * Math.Pow(x, df2 / 2 - i) / (i * Math.Pow(1 - x, 2));
            }
            return 1 - 2 * p;
        }

        // create a function to calculate the t-statistic of a linear regression
        public static double TStatistic(double coefficient, double standardError)
        {
            return coefficient / standardError;
        }

        // create a function to calculate the p-value of a linear regression
        public static double PValue(double t, int df)
        {
            double p = 0;
            double x = df / (df + t * t);
            for (int i = 1; i <= df / 2; i++)
            {
                p += Math.Pow(-1, i + 1) * Math.Pow(x, df / 2 - i) / (i * Math.Pow(1 - x, 2));
            }
            return 1 - 2 * p;
        }

        // create a function to calculate the standard error of a linear regression
        public static double StandardError(double[] values1, double[] values2)
        {
            double sum = 0;
            for (int i = 0; i < values1.Length; i++)
            {
                double difference = values1[i] - values2[i];
                sum += Math.Pow(difference, 2);
            }
            return Math.Sqrt(sum / (values1.Length - 2));
        }

        // create a function to calculate the standard error of a coefficient of a linear regression
        public static double StandardError(double[] values1, double[] values2, int index)
        {
            double standardError = StandardError(values1, values2);
            double sum = 0;
            foreach (double value in values1)
            {
                sum += Math.Pow(value, 2);
            }
            return standardError / Math.Sqrt(sum / (values1.Length - 1));
        }

        // create a function to calculate the confidence interval of a coefficient of a linear regression
        public static double[] ConfidenceInterval(double coefficient, double standardError, double alpha)
        {
            double[] interval = new double[2];
            //double t = TStatistic(coefficient, standardError);
            //double p = PValue(t, values1.Count - 2);
            double z = NormalDistribution.InverseCumulativeDistribution(1 - alpha / 2);
            interval[0] = coefficient - z * standardError;
            interval[1] = coefficient + z * standardError;
            return interval;
        }

        // create a class to calculate the normal distribution of a linear regression  
        private class NormalDistribution
        {

            // create a function to calculate the inverse cumulative distribution of a normal distribution
            public static double InverseCumulativeDistribution(double p)
            {
                if (p <= 0 || p >= 1)
                    throw new ArgumentOutOfRangeException("p", p, "Argument out of range.");

                if (p == 0.5)
                    return 0;

                if (p < 0.5)
                    return -InverseCumulativeDistributionCore(2 * p);

                return InverseCumulativeDistributionCore(2 * (1 - p));
            }

            // create a function to calculate the inverse cumulative distribution of a normal distribution
            private static double InverseCumulativeDistributionCore(double p)
            {
                double[] a = new double[] { -3.969683028665376e+01, 2.209460984245205e+02, -2.759285104469687e+02, 1.383577518672690e+02, -3.066479806614716e+01, 2.506628277459239e+00 };
                double[] b = new double[] { -5.447609879822406e+01, 1.615858368580409e+02, -1.556989798598866e+02, 6.680131188771972e+01, -1.328068155288572e+01 };
                double[] c = new double[] { -7.784894002430293e-03, -3.223964580411365e-01, -2.400758277161838e+00, -2.549732539343734e+00, 4.374664141464968e+00, 2.938163982698783e+00 };
                double[] d = new double[] { 7.784695709041462e-03, 3.224671290700398e-01, 2.445134137142996e+00, 3.754408661907416e+00 };
                double q = p - 0.5;

                if (Math.Abs(q) <= 0.425)
                {
                    double r = 0.180625 - q * q;
                    return q * (((((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * r + 1)
                    / ((((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1) * r + 1));
                }

                double r2 = p;
                if (q > 0)
                    r2 = 1 - p;

                r2 = Math.Log(-Math.Log(r2));
                double r3 = c[0];
                for (int i = 1; i < 6; i++)
                    r3 = r3 * r2 + c[i];

                double r4 = d[0];
                for (int i = 1; i < 4; i++)
                    r4 = r4 * r2 + d[i];

                double r5 = r3 / r4;
                if (q < 0)
                    r5 = -r5;

                return r5;
            }
        }
    }
    
    // cretae a class to perform multiple linear regression with matricies
    public class MLR
    {
        // create a function to get the beta coefficients of a multiple linear regression by exluding dependant variable column from the input
        public static double[] Fit(double[,] x, int depCol = -1)
        {
            if (depCol == -1)
                depCol = x.GetLength(1) - 1;

            // create a vector to hold the dependency column
            double[] y = Matrix.ExtractColumn(x, depCol);
            // create a matrix with the removed dependency column
            double[,] X = Matrix.DeleteColumn(x, depCol);
            // return the calculation
            return Fit(X, y);
        }

        // create a function to get the beta coefficients of a multiple linear regression 
        public static double[] Fit(double[,] x, double[] y)
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

        // create a function to predict yhat from a matrix
        public static double[] Predict(double[,] x, double[] coeffs)
        {
            // create a matrix holding only one column containing ones
            double[,] ones = Matrix.Ones(x.GetLength(0), 1);
            // insert the ones into the input matrix
            double[,] X = Matrix.Concatenate(ones, x);
            // create a vector to hold the predictions
            double[] yhat = Matrix.Multiply(X, coeffs);
            // return the predictions
            return yhat;
        }

        // create a function to predict yhat from a vector of independant variables
        public static double Predict(double[] x, double[] coeffs)
        {
            // create a matrix from the vector and tranpsoe it
            double[,] X = Matrix.Transpose(Matrix.FromVector(x));
            // perform the prediction
            return Predict(X, coeffs)[0];            
        }
    }

    // create a class to perform a logistic regression
    public static class MLRB
    {                
        public static double[] Fit(double[,] x, double learningRate = 0.01, int iterations = 1000, double[] costs = null)
        {
            int samples = x.GetLength(0);
            int features = x.GetLength(1) - 1;
            double[] y = Matrix.ExtractColumn(x, features);
            double[] weights = new double[features + 1];

            for (int i = 0; i < iterations; i++)
            {
                double[] yPred = Predict(x, weights);
                if (costs != null)
                    costs[i] = Cost(y, yPred);
                var dw = new double[features];
                var db = 0.0;
                for (int j = 0; j < samples; j++)
                {
                    for (int k = 0; k < features; k++)
                    {
                        dw[k] += (yPred[j] - y[j]) * x[j, k];
                    }
                    db += (yPred[j] - y[j]);
                }
                for (int j = 0; j < features; j++)
                {
                    weights[j + 1] -= learningRate * dw[j] / samples;
                }
                weights[0] -= learningRate * db / samples; // Bias (stored in indicy 0)
            }

            return weights;
        }

        public static double[] Predict(double[,] x, double[] weights)
        {
            int samples = x.GetLength(0);
            double[] yPred = new double[samples];
            for (int i = 0; i < samples; i++)
            {
                double z = 0.0;
                for (int j = 0; j < weights.Length - 1; j++)
                {
                    z += weights[j + 1] * x[i, j];
                }
                z += weights[0]; // Bias (stored in indicy 0)
                yPred[i] = Sigmoid(z);
            }
            return yPred;
        }

        public static double Predict(double[] x, double[] weights)
        {            
            double[,] X = Matrix.Transpose(Matrix.FromVector(x));
            return Predict(X, weights)[0];
        }

        public static double Cost(double[] y, double[] yPred)
        {
            int samples = y.Length;
            double cost = 0.0;
            for (int i = 0; i < samples; i++)
            {
                cost += y[i] * Math.Log(yPred[i]) + (1 - y[i]) * Math.Log(1 - yPred[i]);
            }
            return -cost / samples;
        }

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }

    public static class MLRC
    {
        public static double[,] Fit(double[,] x, int numClasses, double learningRate = 0.01, int iterations = 1000, double[,] costs = null)
        {
            int samples = x.GetLength(0);
            int features = x.GetLength(1) - numClasses;
            double[,] y = Matrix.ExtractColumns(x, features, numClasses);
            double[,] weights = new double[features + 1, numClasses];

            for (int i = 0; i < iterations; i++)
            {
                double[,] yPred = Predict(x, weights);
                if (costs != null)
                    costs[i, 0] = Cost(y, yPred);
                double[,] dW = new double[features + 1, numClasses];
                for (int j = 0; j < samples; j++)
                {
                    for (int k = 0; k < numClasses; k++)
                    {
                        for (int l = 0; l < features + 1; l++)
                        {
                            dW[l, k] += (yPred[j, k] - y[j, k]) * x[j, l];
                        }
                    }
                }
                for (int j = 0; j < numClasses; j++)
                {
                    for (int k = 0; k < features + 1; k++)
                    {
                        weights[k, j] -= learningRate * dW[k, j] / samples;
                    }
                }
            }

            return weights;
        }

        public static double[,] Predict(double[,] x, double[,] weights)
        {
            int samples = x.GetLength(0);
            int numClasses = weights.GetLength(1);
            double[,] yPred = new double[samples, numClasses];
            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < numClasses; j++)
                {
                    double z = 0.0;
                    for (int k = 0; k < weights.GetLength(0); k++)
                    {
                        z += weights[k, j] * x[i, k];
                    }
                    yPred[i, j] = Sigmoid(z);
                }
            }
            return yPred;
        }

        public static double Cost(double[,] y, double[,] yPred)
        {
            int samples = y.GetLength(0);
            int numClasses = y.GetLength(1);
            double cost = 0.0;
            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < numClasses; j++)
                {
                    cost += y[i, j] * Math.Log(yPred[i, j]) + (1 - y[i, j]) * Math.Log(1 - yPred[i, j]);
                }
            }
            return -cost / samples;
        }

        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
    }

}

