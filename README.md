# Microservice.Email

## Introduction
Microservice.Email is designed to send sanitised templated emails and personalized emails.<br>

## Features
- Sending email templates using Scriban to render body of message.
- Metrics Collection: The application can efficiently collect various metrics.
- Graph Visualization: It offers a built-in graphing feature to visualize the collected data in a user-friendly manner.
- Versatile Data Transfer: Supports sending data seamlessly over REST, gRPC, and RabbitMQ event bus, providing flexibility in data delivery.

## Setup
1. Open root folder of project 
2. Run commands in console:
    ```
    docker-compose up
    dotnet build
    dotnet run
    ```
3. Open a web browser and use this link: http://localhost:5236/swagger/index.html

## Database
Todo

## Dependencies
| Dependency    | URL                               |
|---------------|-----------------------------------|
| Prometheus    | http://localhost:9090				|
| Grafana       | http://localhost:3000				|
| MailHog       | http://localhost:1025             |
| Postgres      | http://localhost:5432             |
| RabbitMQ      | http://localhost:15672            |

## The list of used technologies
- FluentEmail
- HtmlSanitizer
- PostgreSQL
- EF Core
- Prometheus
- Grafana
- MailHog
- RabbitMQ
- gRpc
- Scriban

## Maintainers
- [ymatsiulka](https://github.com/ymatsiulka)
- [architect-prog](https://github.com/architect-prog)