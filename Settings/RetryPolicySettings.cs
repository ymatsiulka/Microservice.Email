namespace Microservice.Email.Settings;

public class RetryPolicySettings
{
    public required int RetriesCount { get; init; }
    public required int MedianRetryDelay { get; init; }
}