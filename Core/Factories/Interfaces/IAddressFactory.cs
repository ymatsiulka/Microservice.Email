using FluentEmail.Core.Models;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IAddressFactory
{
    Address Create(string address);
    Address[] Create(IEnumerable<string> addresses);
}