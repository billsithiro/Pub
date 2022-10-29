public class SimpleLinearRegression
{
    // create a function to calculate the linear regression
    public static double[] Calculate(double[] x, double[] y)
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
