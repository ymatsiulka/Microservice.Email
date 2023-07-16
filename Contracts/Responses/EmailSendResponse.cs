using ArchitectProg.FunctionalExtensions.Contracts;
using Microservice.Email.Contracts.Common;

namespace Microservice.Email.Contracts.Responses;

public sealed class EmailSendResponse
{
    public int Id { get; set; }
    public required Sender Sender { get; init; }
    public required string[] Recipients { get; init; }
    public required DateTimeOffset SentDate { get; init; }
    public required EnumItem EmailStatus { get; init; }
}