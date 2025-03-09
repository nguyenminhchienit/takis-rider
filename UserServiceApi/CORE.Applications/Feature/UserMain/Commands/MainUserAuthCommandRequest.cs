using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class MainUserAuthCommandRequest : UserLoginRequest, IRequest<ResponseCus<string>>
    {
        public class QueryHandler : IRequestHandler<MainUserAuthCommandRequest, ResponseCus<string>>
        {
            private readonly IMainUserCommand mainUserCommand;

            public QueryHandler(IMainUserCommand _mainUserCommand)
            {
                mainUserCommand = _mainUserCommand;
            }
            public async Task<ResponseCus<string>> Handle(MainUserAuthCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserCommand.AuthenticateAsync(request).ConfigureAwait(false);
                    return new ResponseCus<string>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<string>(ex.Message));
                }
            }
        }
    }
}
