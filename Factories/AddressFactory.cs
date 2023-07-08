using ArchitectProg.FunctionalExtensions.Extensions;
using FluentEmail.Core.Models;
using Microservice.Email.Factories.Interfaces;

namespace Microservice.Email.Factories;

public sealed class AddressFactory : IAddressFactory
{
    public Address CreateAddress(string address)
    {
        if (address.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(address));

        var result = new Address
        {
            EmailAddress = address
        };

        return result;
    }
}