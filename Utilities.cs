using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Pub
{
    public static class Utilities
    {
        public static double[,] ReadCSV(string path)
        {
            var lines = File.ReadAllLines(path);
            var matrix = new double[lines.Length, lines[0].Split(',').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var values = line.Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    matrix[i, j] = double.Parse(values[j]);
                }
            }
            return matrix;
        }


        // create a function to calculate moving average
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
    }        

    // create class to perform classic multiplicative time series
    public class TimeSeries
    {
        public double[] Values { get; set; }
        public double[] Seasonal { get; set; }
        public double[] Trend { get; set; }
        public double[] Residual { get; set; }
        public double[] Forecast { get; set; }

        public TimeSeries(double[] values, int seasonality)
        {
            Values = values;
            Seasonal = new double[seasonality];
            Trend = new double[values.Length];
            Residual = new double[values.Length];
            Forecast = new double[values.Length];
        }

        public void Decompose()
        {
            var seasonality = Seasonal.Length;
            var seasons = Values.Length / seasonality;
            var seasonals = new double[seasons, seasonality];
            for (int i = 0; i < seasons; i++)
            {
                for (int j = 0; j < seasonality; j++)
                {
                    seasonals[i, j] = Values[i * seasonality + j];
                }
            }
            for (int i = 0; i < seasonality; i++)
            {
                Seasonal[i] = seasonals.Cast<double>().Average();
            }
            for (int i = 0; i < Values.Length; i++)
            {
                Trend[i] = Values[i] / Seasonal[i % seasonality];
            }
            var trend = Trend.Average();
            for (int i = 0; i < Values.Length; i++)
            {
                Trend[i] /= trend;
            }
            for (int i = 0; i < Values.Length; i++)
            {
                Residual[i] = Values[i] / (Seasonal[i % seasonality] * Trend[i]);
            }
        }

        public void ForecastNext(int steps)
        {
            var seasonality = Seasonal.Length;
            var trend = Trend.Average();
            for (int i = 0; i < steps; i++)
            {
                Forecast[i] = Seasonal[(Values.Length + i) % seasonality] * Trend[Values.Length - 1] * trend;
            }
        }   

        // create a function to calculate the mean absolute percentage error
        public double MAPE()
        {
            var sum = 0.0;
            for (int i = 0; i < Values.Length; i++)
            {
                sum += Math.Abs((Values[i] - Forecast[i]) / Values[i]);
            }
            return sum / Values.Length;
        }

        // create a function to calculate the mean absolute error
        public double MAE()
        {
            var sum = 0.0;
            for (int i = 0; i < Values.Length; i++)
            {
                sum += Math.Abs(Values[i] - Forecast[i]);
            }
            return sum / Values.Length;
        }

        // create a function to calculate the mean squared error
        public double MSE()
        {
            var sum = 0.0;
            for (int i = 0; i < Values.Length; i++)
            {
                sum += Math.Pow(Values[i] - Forecast[i], 2);
            }
            return sum / Values.Length;
        }    

        // create a function to calculate the root mean squared error
        public double RMSE()
        {
            return Math.Sqrt(MSE());
        }

        // create a function to calculate the mean absolute scaled error
        public double MASE()
        {
            var sum = 0.0;
            for (int i = 0; i < Values.Length; i++)
            {
                sum += Math.Abs(Values[i] - Forecast[i]) / Math.Abs(Values[i] - Values[i - 1]);
            }
            return sum / Values.Length;
        }

        // create a function to calculate the mean absolute percentage error
        public double MAPE(double[] values)
        {
            var sum = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += Math.Abs((Values[i] - values[i]) / Values[i]);
            }
            return sum / values.Length;
        }

        // create a function to calculate the mean absolute error
        public double MAE(double[] values)
        {
            var sum = 0.0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += Math.Abs(Values[i] - values[i]);
            }
            return sum / values.Length;
        } 
    }   
}