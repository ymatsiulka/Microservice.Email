using System.Net.Mail;
using Microservice.Email.Core.Factories.Interfaces;
using Microservice.Email.Core.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace Microservice.Email.Core.Factories
{
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
                .Handle<SmtpException>()
                .Or<SmtpFailedRecipientException>()
                .Or<SmtpFailedRecipientsException>()
                .WaitAndRetryAsync(retryDelays);

            return result;
        }
    }
}