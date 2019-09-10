using System;

namespace PiCalcContract
{
    public class CustomContext
    {
        public ulong DeliveryTag { get; set; }
        public Guid GlobalRequestId { get; set; }
    }
}