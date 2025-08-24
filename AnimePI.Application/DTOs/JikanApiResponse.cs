using System.Text.Json.Serialization;

namespace AnimePI.Application.DTOs;

public class JikanApiResponse<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; } = default!;

    [JsonPropertyName("pagination")]
    public JikanPagination? Pagination { get; set; }
}

public class JikanPagination
{
    [JsonPropertyName("last_visible_page")]
    public int LastVisiblePage { get; set; }

    [JsonPropertyName("has_next_page")]
    public bool HasNextPage { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }
}

public class JikanAnimeDto
{
    [JsonPropertyName("mal_id")]
    public int MalId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("title_english")]
    public string? TitleEnglish { get; set; }

    [JsonPropertyName("title_japanese")]
    public string? TitleJapanese { get; set; }

    [JsonPropertyName("synopsis")]
    public string? Synopsis { get; set; }

    [JsonPropertyName("images")]
    public JikanImages? Images { get; set; }

    [JsonPropertyName("trailer")]
    public JikanTrailer? Trailer { get; set; }

    [JsonPropertyName("score")]
    public double? Score { get; set; }

    [JsonPropertyName("rank")]
    public int? Rank { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("episodes")]
    public int? Episodes { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("genres")]
    public List<JikanGenre>? Genres { get; set; }
}

public class JikanImages
{
    [JsonPropertyName("jpg")]
    public JikanImageUrls? Jpg { get; set; }
}

public class JikanImageUrls
{
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

}

public class JikanTrailer
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

public class JikanGenre
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
