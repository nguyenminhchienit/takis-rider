using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CORE.Applications.MessageQueue;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class UserRegisterMakeDriverCommandRequest : UserRegisterMakeDriverModel,IRequest<ResponseCus<bool>>
    {
        public class QueryHandler : IRequestHandler<UserRegisterMakeDriverCommandRequest, ResponseCus<bool>>
        {
            private readonly IMainUserCommand mainUserCommand;
            private readonly UserProducer userProducer;

            public QueryHandler(IMainUserCommand _mainUserCommand, UserProducer _userProducer)
            {
                mainUserCommand = _mainUserCommand;
                userProducer = _userProducer;
            }
            public async Task<ResponseCus<bool>> Handle(UserRegisterMakeDriverCommandRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await mainUserCommand.RegisterMakeDriverAsync(request);
                    userProducer.SendUserRegisteredEvent(request);
                    return new ResponseCus<bool>(result);
                }
                catch (Exception ex) {
                    return await Task.FromResult(new ResponseCus<bool>(ex.Message));
                }
            }
        }
    }
}
