namespace Microservice.Email.Core.Settings;

public sealed class RetryPolicySettings
{
    public required int RetriesCount { get; init; }
    public required int MedianRetryDelay { get; init; }
}