using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Models.Location.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.Location.Queries
{
    public class GetDriverLocationQueryRequest : IRequest<ResponseCus<DriverLocation>>  
    {

        public string driverId { get; set; } = string.Empty;

        public GetDriverLocationQueryRequest(string id)
        {
            driverId = id;
        }
        public class QueryHandler : IRequestHandler<GetDriverLocationQueryRequest, ResponseCus<DriverLocation>>
        {

            private readonly ILocationQueryRepository repository;


            public QueryHandler(ILocationQueryRepository _repository)
            {
                repository = _repository;
            }
            public async Task<ResponseCus<DriverLocation>> Handle(GetDriverLocationQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await repository.GetDriverLocationAsync(request.driverId);
                    return new ResponseCus<DriverLocation>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<DriverLocation>(ex.Message));
                }
            }
        }
    }
}
