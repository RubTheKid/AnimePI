using AnimePI.Application.Services;
using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;
using Moq;

namespace AnimePI.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _mockRepo;
    private UserService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IUserRepository>();
        _service = new UserService(_mockRepo.Object);
    }

    [Test]
    public async Task GetAllUsers_ShouldReturnUsers()
    {
        var expectedUsers = new List<User>
        {
            new User(new UserName("Teste", "123"), new Email("teste@123.com")),
            new User(new UserName("Fulano", "Tal"), new Email("fulano@tal.com"))
        };

        _mockRepo.Setup(r => r.GetAllUsersAsync())
            .ReturnsAsync(expectedUsers);

        var result = await _service.GetAllUsersAsync();

        Assert.That(result, Is.EqualTo(expectedUsers));
        _mockRepo.Verify(r => r.GetAllUsersAsync(), Times.Once);
    }

    [Test]
    public async Task GetUserByIdAsync_ShouldReturnUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser =  new User(new UserName("Teste", "123"), new Email("teste@123.com"));

        _mockRepo.Setup(r => r.GetUserAsync(userId))
            .ReturnsAsync(expectedUser);

        var result = await _service.GetUserByIdAsync(userId);

        Assert.That(result, Is.EqualTo(expectedUser));
        _mockRepo.Verify(r => r.GetUserAsync(userId), Times.Once);
    }

    [Test]
    public async Task CreateUser_ShouldCreateAndReturnUser()
    {
        var firstName = "Teste";
        var surname = "Silva";
        var email = "teste@silva.com";
        var expectedUser = new User(new UserName(firstName, surname), new Email(email));

        _mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser);

        var result = await _service.CreateUserAsync(firstName, surname, email);

        Assert.That(result, Is.EqualTo(expectedUser));
        _mockRepo.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }

}