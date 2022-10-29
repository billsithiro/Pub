public class SimpleLinearRegression
{
    // create a function to calculate the linear regression
    public static double[] Coefficients(double[] x, double[] y)
    {
        double[] result = new double[2];
        double xSum = x.Sum();
        double ySum = y.Sum();
        double x2Sum = x.Select(d => d * d).Sum();
        double xySum = x.Select((t, i) => t * y[i]).Sum();
        double count = x.Length;
        double m = (count * xySum - xSum * ySum) / (count * x2Sum - xSum * xSum);
        double b = (ySum - m * xSum) / count;

        result[0] = m;
        result[1] = b;

        return result;
    }
}

public class SimpleLinearRegressionEx
{
    // create a function to calculate the mean of a list of numbers
    public double Mean(List<double> values)
    {
        double sum = 0;
        foreach (double value in values)
        {
            sum += value;
        }
        return sum / values.Count;
    }

    // create a function to calculate the standard deviation of a list of numbers
    public double StandardDeviation(List<double> values)
    {
        double mean = Mean(values);
        double sum = 0;
        foreach (double value in values)
        {
            sum += Math.Pow(value - mean, 2);
        }
        return Math.Sqrt(sum / (values.Count - 1));
    }

    // create a function to calculate the covariance between two lists of numbers
    public double Covariance(List<double> values1, List<double> values2)
    {
        double mean1 = Mean(values1);
        double mean2 = Mean(values2);
        double sum = 0;
        for (int i = 0; i < values1.Count; i++)
        {
            sum += (values1[i] - mean1) * (values2[i] - mean2);
        }
        return sum / (values1.Count - 1);
    }

    // create a function to calculate the coefficients of a linear regression
    public double[] Coefficients(List<double> values1, List<double> values2)
    {
        double[] coefficients = new double[2];
        coefficients[1] = Covariance(values1, values2) / Math.Pow(StandardDeviation(values1), 2);
        coefficients[0] = Mean(values2) - coefficients[1] * Mean(values1);
        return coefficients;
    }

    // create a function to calculate the predicted values of a linear regression
    public List<double> Predict(List<double> values1, double[] coefficients)
    {
        List<double> predictions = new List<double>();
        foreach (double value in values1)
        {
            predictions.Add(coefficients[0] + coefficients[1] * value);
        }
        return predictions;
    }

    // create a function to calculate the root mean squared error of a linear regression
    public double RMSE(List<double> values1, List<double> values2)
    {
        double sum = 0;
        for (int i = 0; i < values1.Count; i++)
        {
            double difference = values1[i] - values2[i];
            sum += Math.Pow(difference, 2);
        }
        return Math.Sqrt(sum / values1.Count);
    }

    // create a function to calculate the coefficient of determination of a linear regression
    public double R2(List<double> values1, List<double> values2)
    {
        double mean = Mean(values2);
        double sum1 = 0;
        double sum2 = 0;
        for (int i = 0; i < values1.Count; i++)
        {
            sum1 += Math.Pow(values1[i] - values2[i], 2);
            sum2 += Math.Pow(values2[i] - mean, 2);
        }
        return 1 - sum1 / sum2;
    }

    // create a function to calculate the adjusted coefficient of determination of a linear regression
    public double AdjustedR2(List<double> values1, List<double> values2, int numberOfPredictors)
    {
        double r2 = R2(values1, values2);
        return 1 - (1 - r2) * (values1.Count - 1) / (values1.Count - numberOfPredictors - 1);
    }

    // create a function to calculate the F-statistic of a linear regression
    public double FStatistic(List<double> values1, List<double> values2, int numberOfPredictors)
    {
        double r2 = R2(values1, values2);
        return r2 / (1 - r2) * (values1.Count - numberOfPredictors - 1) / numberOfPredictors;
    }

    // create a function to calculate the p-value of a linear regression
    public double PValue(List<double> values1, List<double> values2, int numberOfPredictors)
    {
        double f = FStatistic(values1, values2, numberOfPredictors);
        return 1 - FTest(f, numberOfPredictors, values1.Count - numberOfPredictors - 1);
    }

    // create a function to calculate the F-test of a linear regression
    public double FTest(double f, int df1, int df2)
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
    public double TStatistic(double coefficient, double standardError)
    {
        return coefficient / standardError;
    }

    // create a function to calculate the p-value of a linear regression
    public double PValue(double t, int df)
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
    public double StandardError(List<double> values1, List<double> values2)
    {
        double sum = 0;
        for (int i = 0; i < values1.Count; i++)
        {
            double difference = values1[i] - values2[i];
            sum += Math.Pow(difference, 2);
        }
        return Math.Sqrt(sum / (values1.Count - 2));
    }

    // create a function to calculate the standard error of a coefficient of a linear regression
    public double StandardError(List<double> values1, List<double> values2, int index)
    {
        double standardError = StandardError(values1, values2);
        double sum = 0;
        foreach (double value in values1)
        {
            sum += Math.Pow(value, 2);
        }
        return standardError / Math.Sqrt(sum / (values1.Count - 1));
    }

    // create a function to calculate the confidence interval of a coefficient of a linear regression
    public double[] ConfidenceInterval(double coefficient, double standardError, double alpha)
    {
        double[] interval = new double[2];
        //double t = TStatistic(coefficient, standardError);
        //double p = PValue(t, values1.Count - 2);
        double z = NormalDistribution.InverseCumulativeDistribution(1 - alpha / 2);
        interval[0] = coefficient - z * standardError;
        interval[1] = coefficient + z * standardError;
        return interval;
    }
}

// create a class to calculate the normal distribution of a linear regression  
public class NormalDistribution
{

    // create a function to calculate the inverse cumulative distribution of a normal distribution
    public static double InverseCumulativeDistribution(double p)
    {
        if (p <= 0 || p >= 1)
        {
            throw new ArgumentOutOfRangeException("p", p, "Argument out of range.");
        }
        if (p == 0.5)
        {
            return 0;
        }
        if (p < 0.5)
        {
            return -InverseCumulativeDistributionCore(2 * p);
        }
        return InverseCumulativeDistributionCore(2 * (1 - p));
    }

    // create a function to calculate the inverse cumulative distribution of a normal distribution
    private static double InverseCumulativeDistributionCore(double p)
    {
        double[] a = new double[] { -3.969683028665376e+01,  2.209460984245205e+02,
                                            -2.759285104469687e+02,  1.383577518672690e+02,
                                            -3.066479806614716e+01,  2.506628277459239e+00 };
        double[] b = new double[] { -5.447609879822406e+01,  1.615858368580409e+02,
                                            -1.556989798598866e+02,  6.680131188771972e+01,
                                            -1.328068155288572e+01 };
        double[] c = new double[] { -7.784894002430293e-03, -3.223964580411365e-01,
                                            -2.400758277161838e+00, -2.549732539343734e+00,
                                             4.374664141464968e+00,  2.938163982698783e+00 };
        double[] d = new double[] {  7.784695709041462e-03,  3.224671290700398e-01,
                                             2.445134137142996e+00,  3.754408661907416e+00 };
        double q = p - 0.5;
        if (Math.Abs(q) <= 0.425)
        {
            double r = 0.180625 - q * q;
            return q * (((((((a[0] * r + a[1]) * r + a[2]) * r + a[3]) * r + a[4]) * r + a[5]) * r + 1)
            / ((((((b[0] * r + b[1]) * r + b[2]) * r + b[3]) * r + b[4]) * r + 1) * r + 1));
        }
        double r2 = p;
        if (q > 0)
        {
            r2 = 1 - p;
        }
        r2 = Math.Log(-Math.Log(r2));
        double r3 = c[0];
        for (int i = 1; i < 6; i++)
        {
            r3 = r3 * r2 + c[i];
        }
        double r4 = d[0];
        for (int i = 1; i < 4; i++)
        {
            r4 = r4 * r2 + d[i];
        }
        double r5 = r3 / r4;
        if (q < 0)
        {
            r5 = -r5;
        }
        return r5;
    }
}

