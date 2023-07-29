﻿namespace Microservice.Email.Infrastructure.RabbitMQ;

public sealed class ExchangeSettings
{
    public required string Name { get; init; }
    public required List<QueueSettings> Queues { get; init; }
}
