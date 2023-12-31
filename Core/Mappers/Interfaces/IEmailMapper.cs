﻿using ArchitectProg.Kernel.Extensions.Mappers;
using Microservice.Email.Core.Contracts.Responses;
using Microservice.Email.Domain.Entities;

namespace Microservice.Email.Core.Mappers.Interfaces;

public interface IEmailMapper : IMapper<EmailEntity, EmailResponse>
{
}