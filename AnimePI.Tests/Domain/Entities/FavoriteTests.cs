using AnimePI.Domain.Aggregates.FavoriteAggregate;

namespace AnimePI.Tests.Domain.Entities;

[TestFixture]
public class FavoriteTests
{
    private Favorite _favorite;
    private int idAnime;
    private string tituloAnime;
    private string urlAnime;
    private Guid _userId;

    [SetUp]
    public void Setup()
    {
        _userId = Guid.NewGuid();
        _favorite = new Favorite(_userId);

        idAnime = 1;
        tituloAnime = "Anime 1";
        urlAnime = "https://image.com/image.jpg";
    }

    [Test]
    public void Constructor_ShouldInitializeWithUserId()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_userId, Is.EqualTo(_favorite.UserId));
            Assert.That(_favorite.Animes, Is.Not.Null);
        });
        Assert.That(_favorite.Animes, Is.Empty);
    }

    [Test]
    public void AddFavorite_ToUser_WithValidData()
    {

        _favorite.AddAnime(idAnime, tituloAnime, urlAnime);

        Assert.Multiple(() =>
        {
            Assert.That(idAnime, Is.EqualTo(_favorite.Animes.Count));
            Assert.That(idAnime, Is.EqualTo(_favorite.Animes.First().AnimeId));
            Assert.That(tituloAnime, Is.EqualTo(_favorite.Animes.First().AnimeTitle));
            Assert.That(urlAnime, Is.EqualTo(_favorite.Animes.First().AnimeImageUrl));
        });
    }



    [Test]
    public void RemoveFavorite_FromUser_ShouldBeValid()
    {
        _favorite.AddAnime(idAnime, tituloAnime, urlAnime);
        _favorite.RemoveAnime(1);
        Assert.That(_favorite.Animes, Is.Empty);

    }


}
