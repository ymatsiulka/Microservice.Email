using Microservice.Email.Factories.Interfaces;
using Microservice.Email.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Microservice.Email.Factories;

public class RetryPolicyFactory : IRetryPolicyFactory
{
    private readonly RetryPolicySettings retrySettings;

    public RetryPolicyFactory(IOptions<RetryPolicySettings> retrySettings)
    {
        this.retrySettings = retrySettings.Value;
    }

    public IAsyncPolicy<T> GetPolicy<T>()
    {
        var retryDelays = Backoff.DecorrelatedJitterBackoffV2(
            TimeSpan.FromTicks(retrySettings.MedianRetryDelay),
            retrySettings.RetriesCount);

        var result = Policy<T>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryDelays);

        return result;
    }
}