﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Models.User.Response;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class MainUserAuthCommandRequest : UserLoginRequest, IRequest<ResponseCus<AuthResponse>>
    {
        public class QueryHandler : IRequestHandler<MainUserAuthCommandRequest, ResponseCus<AuthResponse>>
        {
            private readonly IMainUserCommand mainUserCommand;

            public QueryHandler(IMainUserCommand _mainUserCommand)
            {
                mainUserCommand = _mainUserCommand;
            }
            public async Task<ResponseCus<AuthResponse>> Handle(MainUserAuthCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserCommand.AuthenticateAsync(request).ConfigureAwait(false);
                    return new ResponseCus<AuthResponse>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<AuthResponse>(ex.Message));
                }
            }
        }
    }
}
