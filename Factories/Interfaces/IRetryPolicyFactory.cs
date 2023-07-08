using Polly;

namespace Microservice.Email.Factories.Interfaces;

public interface IRetryPolicyFactory
{
    IAsyncPolicy<T> GetPolicy<T>();
}