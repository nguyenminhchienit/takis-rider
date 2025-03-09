using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Repositories.Payment.Interfaces
{
    public interface IPaymentCommandRepository
    {
        Task<string> CreateVietQRPaymentAsync(Guid rideId, string userId, decimal amount);
    }
}
