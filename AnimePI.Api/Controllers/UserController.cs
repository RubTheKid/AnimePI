using AnimePI.Application.Queries;
using AnimePI.Application.UserAggregate.Command.CreateUser;
using AnimePI.Application.UserAggregate.Query.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AnimePI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/user/{id:guid}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery { Id = id };
                var user = await _mediator.Send(query);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var query = new GetAllUsersQuery();
                var users = await _mediator.Send(query);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
