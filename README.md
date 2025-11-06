# EAITMApp
EAITMApp is a .NET-based Todo App built with Clean/Onion Architecture, 
supporting multi-storage, secure encryption, and modular, testable design.

## Architecture
Domain: Core entities (TodoTask, User) & business rules.
Application: Use Cases, MediatR Handlers, Validators (FluentValidation).
Infrastructure: Repositories (InMemory, Postgres, MongoDB), Encryption & SecureMemory services, Storage settings.
API: Controllers & Endpoints interacting with Application layer.

## Multi-Storage Support
InMemory (for fast development/testing)
Postgres (EF Core)
MongoDB (NoSQL)
Multi-store mode: store the same data across multiple sources

## Security & Encryption
Argon2EncryptionService for sensitive data
ISecureMemoryService for in-memory security
Singleton registration ensures performance & consistency

## Features
CRUD operations for TodoTask via Handlers and MediatR
Unified GUID for compatibility across all storage types
Flexible DI with Factory pattern for repository selection
Console logging for Postgres actions
Ready for production with MongoDB & Postgres

## Getting Started
Configure appsettings.json for database connections and storage settings.
Run API via Visual Studio or dotnet run.
Test Endpoints using Postman or any HTTP client.

## Notes
Project is in early stages; future updates will include advanced authentication, logging, and UI.
Multi-storage design allows easy extension to new database providers.