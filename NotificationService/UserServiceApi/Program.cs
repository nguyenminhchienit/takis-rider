using System.Reflection;
using CORE.Applications;
using CORE.Infrastructure.Repositories;
using CORE.Infrastructure.Shared;
using CORE.Infrastructure.Shared.ConfigDB.MongoDB;


var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var services = builder.Services;
var configuration = builder.Configuration;

services.AddRegisterSharedServices(configuration);
services.AddRegisterRepositories(configuration);
services.AddCoreApplication(configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
