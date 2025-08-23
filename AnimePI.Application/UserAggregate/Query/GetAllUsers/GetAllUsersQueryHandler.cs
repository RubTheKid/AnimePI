using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllUsersAsync();
    }
}
