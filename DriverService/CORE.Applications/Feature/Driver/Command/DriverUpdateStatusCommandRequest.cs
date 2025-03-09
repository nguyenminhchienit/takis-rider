using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Driver.Interfaces;
using CORE.Infrastructure.Shared.Models.Driver.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Driver.Command
{
    public class DriverUpdateStatusCommandRequest :  DriverUpdateStatusModel,IRequest<ResponseCus<bool>>
    {
        public class QueryHandler : IRequestHandler<DriverUpdateStatusCommandRequest, ResponseCus<bool>>
        {

            private readonly IDriverCommandRepository driverCommandRepository;

            public QueryHandler(IDriverCommandRepository _driverCommandRepository)
            {
                driverCommandRepository = _driverCommandRepository;
            }
            public async Task<ResponseCus<bool>> Handle(DriverUpdateStatusCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await driverCommandRepository.UpdateDriverStatusAsync(request.driverId, request.Status);
                    return new ResponseCus<bool> (result );
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<bool>(ex.Message));
                }
            }
        }
    }
}
