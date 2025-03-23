using CORE.Applications.MessageQueue;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Models.User.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class MainVerify2FARequest : Verify2FAModel,IRequest<ResponseCus<AuthResponse>>
    {
        public class QueryHandler : IRequestHandler<MainVerify2FARequest, ResponseCus<AuthResponse>>
        {
            private readonly IMainUserCommand mainUserCommand;

            public QueryHandler(IMainUserCommand _mainUserCommand)
            {
                mainUserCommand = _mainUserCommand;
            }
            public async Task<ResponseCus<AuthResponse>> Handle(MainVerify2FARequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserCommand.VerifyTwoFactor(request).ConfigureAwait(false);                    
                    return new ResponseCus<AuthResponse>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<AuthResponse>(ex.Message));

                }
            }
        }
    }
}
