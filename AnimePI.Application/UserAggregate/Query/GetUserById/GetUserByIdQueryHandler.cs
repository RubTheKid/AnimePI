using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserAsync(request.Id);
    }
}
