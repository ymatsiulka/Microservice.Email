using FluentEmail.Core.Models;

namespace Microservice.Email.Core.Factories.Interfaces;

public interface IAddressFactory
{
    Address CreateAddress(string address);
}