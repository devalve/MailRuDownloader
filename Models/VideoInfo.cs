using Newtonsoft.Json;

namespace MailRuDownloader.Models;

public class VideoInfo
{
    [JsonProperty("provider")]
    public string? Provider { get; set; }

    [JsonProperty("multiOverlay")]
    public int? MultiOverlay { get; set; }

    [JsonProperty("skipOverlay")]
    public bool? SkipOverlay { get; set; }

    [JsonProperty("isChannel")]
    public bool? IsChannel { get; set; }

    [JsonProperty("author")]
    public Author? Author { get; set; }

    [JsonProperty("meta")]
    public Meta? Meta { get; set; }

    [JsonProperty("overlayTime")]
    public int? OverlayTime { get; set; }

    [JsonProperty("relatedHost")]
    public object? RelatedHost { get; set; }

    [JsonProperty("adSlot")]
    public int? AdSlot { get; set; }

    [JsonProperty("isCommunity")]
    public bool? IsCommunity { get; set; }

    [JsonProperty("encoding")]
    public bool? Encoding { get; set; }

    [JsonProperty("cluster")]
    public Cluster? Cluster { get; set; }

    [JsonProperty("targetParent")]
    public bool? TargetParent { get; set; }

    [JsonProperty("flags")]
    public int? Flags { get; set; }

    [JsonProperty("admanUrl")]
    public string? AdmanUrl { get; set; }

    [JsonProperty("version")]
    public int? Version { get; set; }

    [JsonProperty("skipAd")]
    public bool? SkipAd { get; set; }

    [JsonProperty("isPrivate")]
    public bool? IsPrivate { get; set; }

    [JsonProperty("region")]
    public int? Region { get; set; }

    [JsonProperty("savePoint")]
    public int? SavePoint { get; set; }

    [JsonProperty("service")]
    public string? Service { get; set; }

    [JsonProperty("spAccess")]
    public object? SpAccess { get; set; }

    [JsonProperty("sitezone")]
    public object? Sitezone { get; set; }

    [JsonProperty("backscreenCounter")]
    public object? BackscreenCounter { get; set; }

    [JsonProperty("videos")]
    public List<VideoVariant>? Videos { get; set; }
}

public class Author
{
    [JsonProperty("profile")]
    public string? Profile { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public class Meta
{
    [JsonProperty("externalId")]
    public string? ExternalId { get; set; }

    [JsonProperty("accId")]
    public long? AccId { get; set; }

    [JsonProperty("duration")]
    public int? Duration { get; set; }

    [JsonProperty("viewsCount")]
    public int? ViewsCount { get; set; }

    [JsonProperty("itemId")]
    public int? ItemId { get; set; }

    [JsonProperty("timestamp")]
    public long? Timestamp { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("poster")]
    public string? Poster { get; set; }
}

public class Cluster
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("id")]
    public int? Id { get; set; }
}

public class VideoVariant
{
    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("seekSchema")]
    public int? SeekSchema { get; set; }

    [JsonProperty("key")]
    public string? Key { get; set; }
}

public class VideoInfoToDb
{
    [JsonProperty(nameof(Url))]
    public string Url { get; set; }

    [JsonProperty(nameof(Title))]
    public string Title { get; set; }

    [JsonProperty(nameof(ExpiredAt))]
    public DateTime ExpiredAt { get; set; }
}