using EAITMApp.Application.UseCases.Commands.TaskCMD;
using MediatR;
using FluentValidation;
using EAITMApp.Application.Validators;
using EAITMApp.Infrastructure;
using EAITMApp.Domain.Common;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


// Add services.
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

//MediatR
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddTodoTaskCommand).Assembly));

// Register Repository
services.AddControllers();

// Register all Validators within the Application Assembly
services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();

// Enable automatic verification in Controllers
services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});

// Infrastructure DI
services.AddInfrastructure(configuration);


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
