

using System;
using Extreme.Mathematics;

namespace PiCalcServer.Services
{
    public interface IPiCalcService
    {
        BigFloat Calculate(int precision, Func<bool> stopFunc);
    }
}