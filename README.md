# EAITMApp

**An app for managing personal or team tasks that combines artificial intelligence and emotional understanding to prioritize tasks, offer smart suggestions, and help users improve their productivity based on their mood.**



# **Architecture**

Domain: Core entities (TodoTask, User) \& business rules.
Application: Use Cases, MediatR Handlers, Validators (FluentValidation).
Infrastructure: Repositories (InMemory, Postgres, MongoDB), Encryption \& SecureMemory services, Storage settings.
API: Controllers \& Endpoints interacting with Application layer.

## Multi-Storage Support

InMemory (for fast development/testing)
Postgres (EF Core)
MongoDB (NoSQL)
Multi-store mode: store the same data across multiple sources

## Security \& Encryption

Argon2EncryptionService for sensitive data
ISecureMemoryService for in-memory security
Singleton registration ensures performance \& consistency

## Features

CRUD operations for TodoTask via Handlers and MediatR
Unified GUID for compatibility across all storage types
Flexible DI with Factory pattern for repository selection
Console logging for Postgres actions
Ready for production with MongoDB \& Postgres

## Getting Started

Configure appsettings.json for database connections and storage settings.
Run API via Visual Studio or dotnet run.
Test Endpoints using Postman or any HTTP client.

## Notes

Project is in early stages; future updates will include advanced authentication, logging, and UI.
Multi-storage design allows easy extension to new database providers.

