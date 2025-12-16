using EAITMApp.Application.DependencyInjection;
using EAITMApp.Infrastructure.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using EAITMApp.Api.Middlewares;


// Serializer dedicated to standardizing the method of storing and reading Guid values in MongoDB.
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// ==========================================================
// 1. SERVICES REGISTRATION (CENTRALIZED)
// ==========================================================

// Application DI
services.AddApplicationServices();
// Infrastructure DI
services.AddInfrastructure(configuration);

// ==========================================================
// 2. API/Controller Configuration
// ==========================================================

// Add services.
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Enable automatic verification in Controllers
services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    // ≈–« ·„ ‰ﬁ„ »≈Ìﬁ«› Â–«° ”Ì „ «· ⁄«„· „⁄ «·√Œÿ«¡ „— Ì‰.
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();
app.MapControllers();

// regester middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
