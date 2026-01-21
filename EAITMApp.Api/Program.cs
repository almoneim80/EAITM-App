using EAITMApp.Application.DependencyInjection;
using EAITMApp.Infrastructure.DependencyInjection;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using EAITMApp.Api.Middlewares;
using EAITMApp.SharedKernel.Common;
using EAITMApp.Infrastructure.Errors.Policies;


// Serializer dedicated to standardizing the method of storing and reading Guid values in MongoDB.
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var appEnv = builder.Environment.IsDevelopment() ? AppEnvironment.Development :
             builder.Environment.IsProduction() ? AppEnvironment.Production :
                                                  AppEnvironment.Testing;
builder.Services.AddSingleton(new ErrorExposurePolicyFactory(appEnv));
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
app.UseMiddleware<CorrelationMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
