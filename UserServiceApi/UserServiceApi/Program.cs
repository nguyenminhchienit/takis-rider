using System.Reflection;
using System.Text;
using Consul;
using CORE.Applications;
using CORE.Infrastructure.Repositories;
using CORE.Infrastructure.Shared;
using CORE.Infrastructure.Shared.ConfigDB.SQL;
using CORE.Infrastructure.Shared.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(c =>
{
    c.Address = new Uri(builder.Configuration["Consul:Host"]);
}));

// Tích hợp MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var services = builder.Services;
var configuration = builder.Configuration;

// Đăng ký các dịch vụ của ứng dụng
services.AddRegisterSharedServices(configuration);
services.AddRegisterRepositories(configuration);
services.AddCoreApplication(configuration);

// ✅ Cấu hình Identity (Tích hợp với Authentication)
services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<DbSqlContext>()
    .AddDefaultTokenProviders();

// 🔥 Thêm cấu hình Cookie để tránh xung đột với JWT Authentication
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/api/auth/login";
    options.AccessDeniedPath = "/api/auth/access-denied";
});

// ✅ Cấu hình Authentication với JWT
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };

    // Xử lý lỗi khi không có quyền
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Bạn chưa đăng nhập hoặc token không hợp lệ!\"}");
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"message\": \"Bạn không có quyền truy cập API này!\"}");
        }
    };
});

services.AddAuthorization();

// ✅ Thêm Swagger với hỗ trợ JWT Authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Thêm hỗ trợ JWT vào Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: Bearer {your_token_here}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ✅ Middleware quan trọng: Authentication trước Authorization
app.UseHttpsRedirection();
app.UseAuthentication();  // 🔥 Phải gọi trước
app.UseAuthorization();

// ✅ Cấu hình Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ✅ Đăng ký Service vào Consul
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
