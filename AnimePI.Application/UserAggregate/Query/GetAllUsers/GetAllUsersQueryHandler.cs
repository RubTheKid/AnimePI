using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetAllUsersResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<GetAllUsersResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(user => new GetAllUsersResponse(
            user.Id,
            user.UserName.FullName,
            user.Email.Mail
        )).ToList();
    }
}
