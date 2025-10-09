using EAITMApp.Api.Enums;
using EAITMApp.Application.Interfaces;
using EAITMApp.Application.UseCases.Commands.TaskCMD;
using EAITMApp.Infrastructure.Configurations;
using EAITMApp.Infrastructure.Data;
using EAITMApp.Infrastructure.Repositories;
using EAITMApp.Infrastructure.Repositories.TaskRepo;
using EAITMApp.Infrastructure.Repositories.UserRepo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FluentValidation;
using EAITMApp.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddTodoTaskCommand).Assembly));

// MongoDb settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


// Read repository type
var repoType = Enum.Parse<RepositoryType>(builder.Configuration["RepositoryType"] ?? "InMemory");

if(repoType  == RepositoryType.Mongo)
{
    builder.Services.AddSingleton<ITodoTaskRepository>(sp =>
    {
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        return new MongoTodoTaskRepository(settings);
    });

    // Register IUserRepository if you want Mongo later
    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
}
else if (repoType == RepositoryType.Postgres)
{
    builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

    builder.Services.AddScoped<ITodoTaskRepository, PostgresTodoTaskRepository>();
    builder.Services.AddScoped<IUserRepository, InMemoryUserRepository>();
}
else
{
    builder.Services.AddSingleton<ITodoTaskRepository, InMemoryTodoTaskRepository>();
    builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
}

Console.WriteLine($"[DEBUG] Repository type from config: {repoType}");

// Register Repository
builder.Services.AddControllers();

// Register all Validators within the Application Assembly
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

// Enable automatic verification in Controllers
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

var app = builder.Build();

app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
