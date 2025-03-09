
using CORE.Infrastructure.Repositories.Location.Interfaces;
using CORE.Infrastructure.Shared.Models.Location.Request;
using MediatR;

namespace CORE.Applications.Feature.Location.Commands
{
    public class UpdateDriverLocationCommandRequest: DriverLocation,IRequest<Unit>
    {
        public class QueryHandler : IRequestHandler<UpdateDriverLocationCommandRequest, Unit>
        {
            private readonly ILocationCommandRepository _repository;

            public QueryHandler(ILocationCommandRepository repository)
            {
                _repository = repository;
            }
            public async Task<Unit> Handle(UpdateDriverLocationCommandRequest request, CancellationToken cancellationToken)
            {
                return Unit.Value;
            }
        }
    }
}
