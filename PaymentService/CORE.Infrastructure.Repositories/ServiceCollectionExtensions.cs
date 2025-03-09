
using CORE.Infrastructure.Repositories.Payment.Commands;
using CORE.Infrastructure.Repositories.Payment.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CORE.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentCommandRepository, PaymentCommnadRepository>();
            services.AddHttpClient();

        }
    }
}
