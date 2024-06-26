﻿using ArchitectProg.FunctionalExtensions.Contracts;
using Microservice.Email.Core.Contracts.Common;

namespace Microservice.Email.Core.Contracts.Responses;

public sealed class EmailResponse
{
    public int Id { get; set; }
    public required Sender Sender { get; init; }
    public required RecipientResponse[] Recipients { get; init; }
    public required DateTimeOffset SentDate { get; init; }
    public required EnumItem EmailStatus { get; init; }
}