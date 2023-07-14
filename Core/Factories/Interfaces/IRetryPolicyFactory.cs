using Polly;

namespace Microservice.Email.Core.Factories.Interfaces
{
    public interface IRetryPolicyFactory
    {
        IAsyncPolicy<T> GetPolicy<T>();
    }
}