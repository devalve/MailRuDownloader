using Newtonsoft.Json;

namespace MailRuDownloader.Models;

public class AjaxVideoWrapper
{
    public string? Type { get; set; } // "AjaxResponse"
    public string? Status { get; set; } // "OK"
    public VideoResponseData? Payload { get; set; }
}

public class VideoResponseData
{
    [JsonProperty("isutf")]
    public int IsUtf { get; set; }

    [JsonProperty("charset")]
    public string Charset { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("items")]
    public List<VideoItem> Items { get; set; }
}

public class VideoItem
{
    [JsonProperty(nameof(CanWrite))]
    public int CanWrite { get; set; }

    [JsonProperty(nameof(ItemId))]
    public string ItemId { get; set; }

    [JsonProperty(nameof(ImageUrlP))]
    public string ImageUrlP { get; set; }

    [JsonProperty("AddTime_y")]
    public string AddTimeY { get; set; }

    [JsonProperty(nameof(ImageUrlI))]
    public string ImageUrlI { get; set; }

    [JsonProperty(nameof(FiledUrl))]
    public string FiledUrl { get; set; }

    [JsonProperty(nameof(UserName))]
    public string UserName { get; set; }

    [JsonProperty(nameof(VideoUrl))]
    public string VideoUrl { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty(nameof(Album))]
    public string Album { get; set; }

    [JsonProperty(nameof(FormattedTime))]
    public string FormattedTime { get; set; }

    [JsonProperty(nameof(DurationFormat))]
    public string DurationFormat { get; set; }

    [JsonProperty("HDexist")]
    public int HdExist { get; set; }

    [JsonProperty(nameof(Empty))]
    public string Empty { get; set; }

    [JsonProperty(nameof(DurationMicro))]
    public string DurationMicro { get; set; }

    [JsonProperty(nameof(PreviewCount))]
    public int PreviewCount { get; set; }

    [JsonProperty(nameof(ID))]
    public int ID { get; set; }

    [JsonProperty(nameof(AddDate))]
    public string AddDate { get; set; }

    [JsonProperty(nameof(Time))]
    public long Time { get; set; }

    [JsonProperty(nameof(UserEmail))]
    public string UserEmail { get; set; }

    [JsonProperty(nameof(UserDir))]
    public string UserDir { get; set; }

    [JsonProperty(nameof(MetaUrl))]
    public string MetaUrl { get; set; }

    [JsonProperty(nameof(UserID))]
    public long UserID { get; set; }

    [JsonProperty("AddTime_d")]
    public string AddTimeD { get; set; }

    [JsonProperty(nameof(AddTime))]
    public long AddTime { get; set; }

    [JsonProperty(nameof(Title))]
    public string Title { get; set; }

    [JsonProperty(nameof(PreviewNum))]
    public int PreviewNum { get; set; }

    [JsonProperty(nameof(PreviewUrlPrefix))]
    public string PreviewUrlPrefix { get; set; }

    [JsonProperty(nameof(PreviewUrl))]
    public string PreviewUrl { get; set; }

    [JsonProperty(nameof(Adult))]
    public int Adult { get; set; }

    [JsonProperty(nameof(Duration))]
    public int Duration { get; set; }

    [JsonProperty(nameof(UrlHtml))]
    public string UrlHtml { get; set; }

    [JsonProperty("AddTime_m")]
    public string AddTimeM { get; set; }

    [JsonProperty(nameof(IsLegal))]
    public int IsLegal { get; set; }

    [JsonProperty("hasLongPreview")]
    public int HasLongPreview { get; set; }

    [JsonProperty(nameof(CanEdit))]
    public int CanEdit { get; set; }

    [JsonProperty(nameof(ViewCount))]
    public int ViewCount { get; set; }

    [JsonProperty(nameof(ExternalID))]
    public string ExternalID { get; set; }

    [JsonProperty("FiledUrl_Text_HTML")]
    public string FiledUrlTextHtml { get; set; }

    [JsonProperty(nameof(AddDateMicro))]
    public string AddDateMicro { get; set; }
}
