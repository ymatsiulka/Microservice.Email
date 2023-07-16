using FluentEmail.Core.Models;
using Microservice.Email.Core.Factories.Interfaces;

namespace Microservice.Email.Core.Factories;

public sealed class AddressFactory : IAddressFactory
{
    public Address Create(string address)
    {
        var result = new Address
        {
            EmailAddress = address
        };

        return result;
    }

    public Address[] Create(IEnumerable<string> addresses)
    {
        var result = addresses.Select(Create).ToArray();
        return result;
    }
}