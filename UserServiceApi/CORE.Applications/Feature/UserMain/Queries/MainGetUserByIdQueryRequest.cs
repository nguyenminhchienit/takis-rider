using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.UserMain.Queries
{
    public class MainGetUserByIdQueryRequest : IRequest<ResponseCus<UserRequest>>
    {

        public string UserId { get; set; }

        public MainGetUserByIdQueryRequest(string userId)
        {
            UserId = userId;
        }
        public class QueryHandler : IRequestHandler<MainGetUserByIdQueryRequest, ResponseCus<UserRequest>>
        {

            private readonly IMainUserQuery mainUserQuery;

            public QueryHandler(IMainUserQuery _mainUserQuery)
            {
                mainUserQuery = _mainUserQuery;
            }
            public async Task<ResponseCus<UserRequest>> Handle(MainGetUserByIdQueryRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserQuery.GetUserByIdAsync(request.UserId);
                    return new ResponseCus<UserRequest>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<UserRequest>(ex.Message));
                }
            }
        }
    }
}
