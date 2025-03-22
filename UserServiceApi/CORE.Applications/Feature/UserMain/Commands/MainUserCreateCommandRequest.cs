
using CORE.Applications.MessageQueue;
using CORE.Infrastructure.Repositories.User.Interfaces;
using CORE.Infrastructure.Shared.Models.User.Request;
using CORE.Infrastructure.Shared.Response;
using MediatR;

namespace CORE.Applications.Feature.UserMain.Commands
{
    public class MainUserCreateCommandRequest : RegisterModel, IRequest<ResponseCus<UserRequest>>
    {
        public class QueryHandler : IRequestHandler<MainUserCreateCommandRequest, ResponseCus<UserRequest>>
        {

            private readonly IMainUserCommand mainUserCommand;
            private readonly UserProducer userProducer;

            public QueryHandler(IMainUserCommand _mainUserCommand, UserProducer _userProducer)
            {
                mainUserCommand = _mainUserCommand;
                userProducer = _userProducer;   
            }
            public async Task<ResponseCus<UserRequest>> Handle(MainUserCreateCommandRequest request, CancellationToken cancellationToken)
            {

                try
                {
                    var result = await mainUserCommand.CreateUserAsync(request).ConfigureAwait(false);
                    //if (request.IsDriver)
                    //{
                    //    userProducer.SendUserRegisteredEvent(request);
                    //}
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
