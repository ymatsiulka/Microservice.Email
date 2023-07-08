using FluentEmail.Core.Models;

namespace Microservice.Email.Factories.Interfaces;

public interface IAddressFactory
{
    Address CreateAddress(string address);
}