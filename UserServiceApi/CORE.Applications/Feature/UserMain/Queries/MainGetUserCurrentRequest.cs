using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Applications.Feature.UserMain.Queries
{
    public class MainGetUserCurrentRequest : IRequest<ResponseCus<UserRequest>>
    {
        public class QueryHandler : IRequestHandler<MainGetUserCurrentRequest, ResponseCus<UserRequest>>
        {
            private readonly IMainUserQuery mainUserQuery;

            public QueryHandler(IMainUserQuery _mainUserQuery)
            {
                mainUserQuery = _mainUserQuery;
            }
            public async Task<ResponseCus<UserRequest>> Handle(MainGetUserCurrentRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserQuery.GetUserCurrent().ConfigureAwait(false);

                    return new ResponseCus<UserRequest>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<UserRequest>(ex.Message));

                }
            }
        }
    }
}
