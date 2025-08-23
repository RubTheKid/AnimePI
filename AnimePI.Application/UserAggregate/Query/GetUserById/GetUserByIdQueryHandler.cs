using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using MediatR;

namespace AnimePI.Application.UserAggregate.Query.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResponse?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByIdQueryResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(request.Id);

        if (user == null)
            return null;

        return new GetUserByIdQueryResponse(user.Id, user.Email, user.DateCreated, user.UserName, user.Favorites?.Animes);
    }
}
