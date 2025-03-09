using System.Reflection;
using Consul;
using CORE.Applications;
using CORE.Infrastructure.Repositories;
using CORE.Infrastructure.Shared;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(c =>
{
    c.Address = new Uri(builder.Configuration["Consul:Host"]);
}));

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

// Đăng ký Service vào Consul
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var consulClient = app.Services.GetRequiredService<IConsulClient>();

var registration = new AgentServiceRegistration
{
    ID = builder.Configuration["Consul:ServiceId"],
    Name = builder.Configuration["Consul:ServiceName"],
    Address = "localhost",  // Nếu deploy bằng Docker, đổi thành tên container
    Port = int.Parse(builder.Configuration["Consul:ServicePort"])
};

consulClient.Agent.ServiceRegister(registration).Wait();
lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(builder.Configuration["Consul:ServiceId"]).Wait();
});

app.Run();
