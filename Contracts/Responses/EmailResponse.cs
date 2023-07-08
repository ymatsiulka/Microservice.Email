using ArchitectProg.FunctionalExtensions.Contracts;

namespace Microservice.Email.Contracts.Responses
{
    public sealed class EmailResponse
    {
        public int Id { get; set; }
        public string? Body { get; init; }
        public required string Subject { get; init; }
        public required string Sender { get; init; }
        public required string[] Recipients { get; init; }
        public required DateTimeOffset SentDate { get; init; }
        public required EnumItem EmailStatus { get; init; }
    }
}
