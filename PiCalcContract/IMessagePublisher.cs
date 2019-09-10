using System;
using System.Threading.Tasks;
using RawRabbit.Operations.Publish.Context;

namespace PiCalcContract
{
    public interface IMessagePublisher<T>
    {
        Task Publish(T message, Action<IPublishContext> contextAction = null);
    }
}