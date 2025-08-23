using AnimePI.Application.Services;
using AnimePI.Application.UserAggregate.Command.DeleteUser;
using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Aggregates.UserAggregate.Interfaces;
using AnimePI.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace AnimePI.Tests.Repository;

[TestFixture]
public class UserRepositoryTests
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
    public async Task GetAllUsers_ShouldReturnAll()
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
    public async Task GetUserById_ShouldreturnUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new User(new UserName("Teste", "Fulano"), new Email("teste@teste.com"));

        _mockRepo.Setup(r => r.GetUserAsync(userId))
            .ReturnsAsync(expectedUser);

        var result = await _service.GetUserByIdAsync(userId);

        Assert.That(result, Is.EqualTo(expectedUser));
        _mockRepo.Verify(r => r.GetUserAsync(userId), Times.Once);
    }


    [Test]
    public async Task CreateUser_ShouldCreate_AndReturn()
    {
        var firstName = "Teste";
        var surname = "Silva";
        var email = "teste@silva.com";
        var expectedUser = new User(new UserName(firstName, surname), new Email(email));

        _mockRepo.Setup(r => r.CreateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(expectedUser);

        var result = await _service.CreateUserAsync(firstName, surname, email);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.UserName.FirstName, Is.EqualTo(firstName));
            Assert.That(result.UserName.Surname, Is.EqualTo(surname));
            Assert.That(result.Email.Mail, Is.EqualTo(email));
        });

        _mockRepo.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public async Task DeleteUser_ShouldReturnTrue()
    {
        var userId = Guid.NewGuid();
        _mockRepo.Setup(r => r.DeleteUserAsync(userId))
            .ReturnsAsync(true);

        var result = await _service.DeleteUserAsync(userId);

        Assert.That(result, Is.True);
        _mockRepo.Verify(r => r.DeleteUserAsync(userId), Times.Once);
    }

    [Test]
    public async Task UpdateUser_ShouldUpdateAndReturn()
    {
        var userId = Guid.NewGuid();
        var firstName = "Teste";
        var surname = "123";
        var email = "teste@teste.com";
        var existingUser = new User(new UserName("Teste", "123"), new Email("teste@teste.com"));
        var updatedUser = new User(new UserName(firstName, surname), new Email(email));

        _mockRepo.Setup(r => r.GetUserAsync(userId))
            .ReturnsAsync(existingUser);
        _mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<User>()))
            .ReturnsAsync(updatedUser);

        var result = await _service.UpdateUserAsync(userId, firstName, surname, email);

        Assert.That(result, Is.EqualTo(updatedUser));
        _mockRepo.Verify(r => r.GetUserAsync(userId), Times.Once);
        _mockRepo.Verify(r => r.UpdateUserAsync(It.IsAny<User>()), Times.Once);
    }
}
