using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Tests.Domain.Entities;

[TestFixture]
public class FavoriteTests
{
    private Favorite _favorite;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _userId = Guid.NewGuid();
        _favorite = new Favorite(_userId);
    }

    [Test]
    public void Constructor_ShouldInitializeWithUserId()
    {
        Assert.That(_userId, Is.EqualTo(_favorite.UserId));
        Assert.IsNotNull(_favorite.Animes);
        Assert.IsEmpty(_favorite.Animes);
    }

    [Test]
    public void AddFavorite_ToUser_WithValidData()
    {
        var animeId = 1;
        var animeTitle = "teste";
        var animeImageUrl = "https://teste.com/teste.jpg";

        _favorite.AddAnime(animeId, animeTitle, animeImageUrl);

        Assert.That(1, Is.EqualTo(_favorite.Animes.Count));
        Assert.That(animeId, Is.EqualTo(_favorite.Animes.First().AnimeId));
        Assert.That(animeTitle, Is.EqualTo(_favorite.Animes.First().AnimeTitle));
        Assert.That(animeImageUrl, Is.EqualTo(_favorite.Animes.First().AnimeImageUrl));
    }


 
    [Test]
    public void RemoveFavorite_FromUser_ShouldBeValid()
    {
        _favorite.AddAnime(1, "teste", "https://teste.com/teste.jpg");
        _favorite.RemoveAnime(1);
        Assert.IsEmpty(_favorite.Animes);

    }


}
