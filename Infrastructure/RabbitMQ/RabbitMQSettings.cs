﻿namespace Microservice.Email.Infrastructure.RabbitMQ;

public class RabbitMQSettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}
