using CORE.Applications.MessageQueue;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class MainEnable2FARequest : Enable2FARequest, IRequest<ResponseCus<bool>>
    {

        public class QueryHandler : IRequestHandler<MainEnable2FARequest, ResponseCus<bool>>
        {
            private readonly IMainUserCommand mainUserCommand;

            public QueryHandler(IMainUserCommand _mainUserCommand)
            {
                mainUserCommand = _mainUserCommand;
            }
            public async Task<ResponseCus<bool>> Handle(MainEnable2FARequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserCommand.EnableTwoFactor(request).ConfigureAwait(false);
             
                    return new ResponseCus<bool>(result);
                }
                catch (Exception ex)
                {
                    return await Task.FromResult(new ResponseCus<bool>(ex.Message));

                }
            }
        }
    }
}
