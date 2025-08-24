using AnimePI.Domain.Core;

namespace AnimePI.Domain.Aggregates.AnimeAggregate;

public class Anime : BaseEntity
{
    public int MalId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string TitleEnglish { get; set; } = string.Empty;
    public string TitleJapanese { get; set; } = string.Empty;
    public string Synopsis { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string TrailerUrl { get; set; } = string.Empty;
    public double Score { get; set; }
    public int Rank { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Episodes { get; set; }
    public int Year { get; set; }
    public List<string> Genres { get; set; } = new List<string>();

    protected Anime() { }

    public Anime(int malId, string title)
    {
        MalId = malId;
        Title = title;
    }

    public void UpdateFromExternalData(
        string titleEnglish,
        string titleJapanese,
        string synopsis,
        string imageUrl,
        string trailerUrl,
        double score,
        int rank,
        string status,
        string type,
        int episodes,
        int year,
        List<string> genres
        )
    {
        TitleEnglish = titleEnglish;
        TitleJapanese = titleJapanese;
        Synopsis = synopsis;
        ImageUrl = imageUrl;
        TrailerUrl = trailerUrl;
        Score = score;
        Rank = rank;
        Status = status;
        Type = type;
        Episodes = episodes;
        Year = year;
        Genres = genres;
        DateUpdated = DateTime.UtcNow;
    }
}