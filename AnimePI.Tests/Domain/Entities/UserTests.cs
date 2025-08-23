using AnimePI.Domain.Aggregates.UserAggregate;
using AnimePI.Domain.Core;


namespace AnimePI.Tests.Domain.Entities;

[TestFixture]
public class UserTests
{
    private User _user;
    private object nome;
    private string sobrenome;
    private UserName _userName;
    private Email _email;
    private string mail;
    private int idAnime;
    private string tituloAnime;
    private string urlAnime;

    [SetUp]
    public void Setup()
    {
        _userName = new UserName("Jader", "Cardoso");
        _email = new Email("jader@cardoso.com");
        _user = new User(_userName, _email);

        nome = "Jader";
        sobrenome = "Cardoso";
        mail = "jader@cardoso.com";

        idAnime = 1;
        tituloAnime = "Anime 1";
        urlAnime = "https://image.com/image.jpg";
    }
    [Test]
    public void Constructor_ShouldSetProps()
    {
        Assert.That(_userName, Is.EqualTo(_user.UserName));
        Assert.That(_email, Is.EqualTo(_user.Email));
        Assert.IsNull(_user.Favorites);
    }

    [Test]
    public void CreateUser_shouldCreate_WithValidData()
    {

        Assert.NotNull(_user);
        Assert.That(_userName, Is.EqualTo(_user.UserName));
        Assert.That(nome, Is.EqualTo(_user.UserName.FirstName));
        Assert.That(sobrenome, Is.EqualTo(_user.UserName.Surname));
        Assert.That(mail, Is.EqualTo(_user.Email.Mail));
    }

    [Test]
    public void AddFavorite_ShouldAddFavorite_WithValidData()
    {
        _user.AddFavorite(idAnime, tituloAnime, urlAnime);

        Assert.That(1, Is.EqualTo(_user.GetFavorites().Count));
        Assert.That(1, Is.EqualTo(_user.GetFavorites()[0].AnimeId));
    }

    [Test]
    public void AddFirstFavorite_ToUser_ShoudBeFalid()
    {
        _user.AddFavorite(1, "Anime 1", "https://image.com/image.jpg");

        Assert.IsNotNull(_user.Favorites);
        Assert.That(1, Is.EqualTo(_user.GetFavorites().Count));
        Assert.That(_user.Id, Is.EqualTo(_user.Favorites.UserId));

    }


}
