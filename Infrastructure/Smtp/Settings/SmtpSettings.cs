﻿namespace Microservice.Email.Infrastructure.Smtp.Settings;

public sealed class SmtpSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public bool EnableSsl { get; init; }
    public bool UseDefaultCredentials { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}
