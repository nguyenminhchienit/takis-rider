using System.Reflection;
using Consul;
using CORE.Applications;
using CORE.Infrastructure.Repositories;
using CORE.Infrastructure.Shared;


var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
builder.Services.AddControllers();

builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri(configuration["Consul:Host"]);
}));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var services = builder.Services;


services.AddRegisterSharedServices(configuration);
services.AddRegisterRepositories(configuration);
services.AddCoreApplication(configuration);

var app = builder.Build();



// Đăng ký Service Discovery vào Consul
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var consulClient = app.Services.GetRequiredService<IConsulClient>();



var registration = new AgentServiceRegistration
{
    ID = configuration["Consul:ServiceId"],
    Name = configuration["Consul:ServiceName"],
    Address = "localhost",
    Port = int.Parse(configuration["Consul:ServicePort"])
};

consulClient.Agent.ServiceRegister(registration).Wait();
lifetime.ApplicationStopping.Register(() =>
{
    consulClient.Agent.ServiceDeregister(configuration["Consul:ServiceId"]).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
