using System;
using Extreme.Mathematics;
using RabbitMQ.Client.Framing;

namespace PiCalcServer.Services
{
    public class PiCalcService : IPiCalcService
    {
        public BigFloat Calculate(int precision, Func<bool> stopFunc)
        {
            BigInteger l = 13591409;
            BigInteger x = 1;

            var goal = AccuracyGoal.Relative(precision + 10);

            BigFloat.DefaultAccuracyGoal = goal;
            BigFloat.DefaultRoundingMode = RoundingMode.TowardsNearest;

            var m = (BigFloat) 1;
            long k = 6;

            var sum = (BigFloat) 13591409;

            for (var index = 1; index < int.MaxValue; index++)
            {
                //if (stopFunc())
                //{
                //    return -1;
                //}

                l += 545140134;
                x *= -262537412640768000;

                m = m * (BigInteger.Pow(k, 3) - 16 * k) /
                    BigInteger.Pow(index, 3);

                BigFloat c;
                sum += (c = (m * l / x));

                k += 12;

                var correctionDigits = -c.GetDecimalDigits();

                if (correctionDigits >= precision)
                {
                    break;
                }
            }

            var alpha = 426880 * BigFloat.Sqrt(10005, goal, RoundingMode.TowardsNearest);

            var pi = alpha / sum;

            //if (stopFunc())
            //{
            //    return -1;
            //}

            return pi.RestrictPrecision(AccuracyGoal.Absolute(precision), new RoundingMode());
        }
    }
}