using MailRuDownloader.Helpers;
using MailRuDownloader.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

var videoLinks = new List<VideoInfoToDb>();

var fileContent = File.ReadAllText("links.txt");
var linkInfo = JsonConvert.DeserializeObject<List<LinkInfo>>(fileContent);

string BASE_METAINFO_URL = linkInfo.Where(i => i.Name == "meta").First().Url;
string TOTAL_ITEMS_URL = linkInfo.Where(i => i.Name == "total").First().Url;

if (string.IsNullOrWhiteSpace(TOTAL_ITEMS_URL) || string.IsNullOrWhiteSpace(BASE_METAINFO_URL))
{
    Console.WriteLine("Сначала заполните total и meta в файле links.txt");
    return;
}
const string CACHE_FILE = "videos.json";

#region Сбор инфы и старт

string? archiveCookie, videoCookie, filter;

GetUserData(out archiveCookie, out videoCookie, out filter);

var handler = new HttpClientHandler
{
    UseCookies = false,
    MaxConnectionsPerServer = 50,
    AutomaticDecompression = DecompressionMethods.All
};
using var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(10) };

var wrapper = new AjaxVideoWrapper();

await StartProgramAsync(videoLinks, TOTAL_ITEMS_URL, CACHE_FILE, archiveCookie, videoCookie, filter, client);

#endregion

#region Приватные методы

async Task ConvertToWavAsync(IEnumerable<VideoInfoToDb> videoLinks)
{
    foreach (var video in videoLinks)
    {
        var tempFileName = Path.GetTempFileName() + VideoFormats.MP4;
        var wavFileName = Path.Combine("output", $"{CommonHelper.SanitizeFileName(video.Title)}{VideoFormats.WAV}");

        Directory.CreateDirectory("output");

        var request = new HttpRequestMessage(HttpMethod.Get, "https:" + video.Url);
        request.Headers.Add("Cookie", videoCookie);

        var response = await client.SendAsync(request);
        var videoStream = await response.Content.ReadAsStreamAsync();

        await using (var fileStream = File.Create(tempFileName))
        {
            await videoStream.CopyToAsync(fileStream);
        }

        VideoConverterHelper.ConvertToWav(tempFileName, wavFileName);
    }
}

void SaveToJson(IEnumerable<VideoInfoToDb> videoLinks)
{
    // Сохранение в JSON
    try
    {
        // Извлечение expire_at из URL
        foreach (var video in videoLinks)
        {
            var expireAt = ExtractExpireAt(video.Url);
            video.ExpiredAt = expireAt;
        }

        var json = JsonConvert.SerializeObject(videoLinks, Formatting.Indented);
        File.WriteAllText(CACHE_FILE, json);

        Console.WriteLine($"Сохранено в файл: {Path.GetFullPath(CACHE_FILE)}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при сохранении JSON: {ex.Message}");
    }
}

DateTime ExtractExpireAt(string url)
{
    // Извлекаем значение expire_at из URL
    var queryParams = System.Web.HttpUtility.ParseQueryString(url);
    var expireAtValue = queryParams["expire_at"];

    if (long.TryParse(expireAtValue, out long expireAtUnix))
    {
        // Конвертируем Unix-время в DateTime
        return DateTimeOffset.FromUnixTimeSeconds(expireAtUnix).DateTime.ToLocalTime();
    }

    // Если значение не найдено или не корректное, возвращаем минимальную дату
    return DateTime.MinValue;
}


async Task GoToServerAsync(
    int total,
    List<VideoInfoToDb> videoLinks)
{
    try
    {
        Console.WriteLine("Запуск цикла загрузки видео...");

        var counter = 1;
        while (videoLinks.Count != total)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BASE_METAINFO_URL}/{counter}");
            request.Headers.Add("Cookie", archiveCookie);

            try
            {
                var response = await client.SendAsync(request);
                var videoInfo = JsonConvert.DeserializeObject<VideoInfo>(await response.Content.ReadAsStringAsync());

                if (videoInfo is null || videoInfo.Videos is null || videoInfo.Videos.Count < 1)
                {
                    counter++;
                    Console.WriteLine($"Видео {counter} не найдено или пустое, продолжаем...");
                    continue;
                }

                // Если видео есть, добавляем в список
                var videoInfoToDb =
                    videoInfo.Videos
                        .Where(v => !string.IsNullOrWhiteSpace(v.Url))
                        .Select(v => new VideoInfoToDb
                        {
                            Title = videoInfo.Meta.Title,
                            Url = v.Url
                        })
                        .FirstOrDefault();

                if (!string.IsNullOrEmpty(videoInfoToDb.Url))
                {
                    videoLinks.Add(videoInfoToDb);
                    Console.WriteLine($"Добавлено видео {counter}. Ссылка: {videoInfoToDb.Url}, название: {videoInfoToDb.Title}");
                }

                counter++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке видео {counter}: {ex.Message}");
                counter++;
            }
        }

        SaveToJson(videoLinks);

        Console.WriteLine($"Все видео успешно загружены. Начинается конвертация в {VideoFormats.MP3}");
        await ConvertToWavAsync(videoLinks.Where(i => i.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase)));
        Console.WriteLine($"Все видео успешно конвертированы в {VideoFormats.MP3} и загружены в папку 'output/'.");

        return;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ошибка: {e.Message}");
        return;
    }
}

async Task GoToFileAsync(
    List<VideoInfoToDb> videoLinks)
{
    try
    {
        var fileContent = File.ReadAllText(CACHE_FILE);
        var fromDb = JsonConvert.DeserializeObject<List<VideoInfoToDb>>(fileContent);

        if (fromDb is null ||
            fromDb is { Count: < 1 })
        {
            Console.WriteLine("Десериализация завершена с ошибками.");
            return;
        }

        if (fromDb.First().ExpiredAt <= DateTime.Now)
        {
            Console.WriteLine("У ссылок на видео истёк срок доступа, нужно обновить. Запустить повторное получение ссылок с сервера (y/n): ");

            if (Console.ReadLine() == "y")
            {
                File.Delete(CACHE_FILE);
                await GoToServerAsync(
                    wrapper.Payload.Total,
                    videoLinks);
            }
            else
            {
                return;
            }
        }

        videoLinks = [.. fromDb.Where(i => i.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase))];

        if (videoLinks.Count == 0)
        {
            Console.WriteLine("Не найдено ни одного подходящего файла.");
            return;
        }
        Console.WriteLine($"Загружено {videoLinks.Count} видео из локального файла.Начинается конвертация");

        await ConvertToWavAsync(videoLinks);
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Не удалось загрузить данные из файла: {ex.Message}");
        return;
    }
}

static void GetUserData(out string? archiveCookie, out string? videoCookie, out string? filter)
{
    archiveCookie = string.Empty;
    do
    {
        Console.Write("Введите строку кук для доступа к архиву: ");
        archiveCookie = Console.ReadLine();

        if (string.IsNullOrEmpty(archiveCookie))
            Console.WriteLine(Environment.NewLine + "Куки не могут быть пустыми.");

    } while (string.IsNullOrWhiteSpace(archiveCookie));

    videoCookie = string.Empty;
    do
    {
        Console.Write("Введите строку кук для доступа к конкретным видео: ");
        videoCookie = Console.ReadLine();

        if (string.IsNullOrEmpty(videoCookie))
            Console.WriteLine(Environment.NewLine + "Куки не могут быть пустыми.");

    } while (string.IsNullOrWhiteSpace(videoCookie));

    filter = string.Empty;
    do
    {
        Console.Write("Введите фразу/предложение/слово для поиска и загрузки:");
        filter = Console.ReadLine();

        if (string.IsNullOrEmpty(filter))
            Console.WriteLine(Environment.NewLine + "Ключевое слово не может быть пустым.");

    } while (string.IsNullOrWhiteSpace(filter));
}

async Task StartProgramAsync(
    List<VideoInfoToDb> videoLinks,
    string TOTAL_ITEMS_URL,
    string CACHE_FILE,
    string? archiveCookie,
    string? videoCookie,
    string? filter,
    HttpClient client)
{

    // Общее число записей
    try
    {
        Console.WriteLine("Отправка запроса на получение списка видео...");
        var request = new HttpRequestMessage(HttpMethod.Get, TOTAL_ITEMS_URL);
        request.Headers.Add("Cookie", archiveCookie);

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"❌ Ошибка HTTP {(int)response.StatusCode} {response.StatusCode}");
            Console.WriteLine($"📄 Текст ошибки: {errorContent}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Ответ от сервера получен.");

        var videoResponseData = JsonConvert.DeserializeObject<List<object>>(responseString);

        if (videoResponseData is null)
        {
            Console.WriteLine("Ошибка: ответ пришёл пустым");
            return;
        }

        wrapper = new AjaxVideoWrapper
        {
            Type = videoResponseData[0].ToString(),
            Status = videoResponseData[1].ToString(),
            Payload = ((JObject)videoResponseData[2]).ToObject<VideoResponseData>()
        };

        if (wrapper is { Payload: null })
        {
            Console.WriteLine($"Ошибка: не получилось сконвертировать пришедшую {nameof(VideoResponseData)}");
            return;
        }
        Console.WriteLine("Успешно сконвертировали данные ответа.");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Ошибка: {e.Message}");
        return;
    }

    // Попробуем прочитать из файла
    if (File.Exists(CACHE_FILE))
    {
        Console.Write("Обнаружен локальный json-файл videos.json. Загрузить его вместо обращения к серверу (y/n): ");

        if (Console.ReadLine()?.ToLower() == "y")
        {
            await GoToFileAsync(videoLinks);
            return;
        }
        else
        {
            File.Delete(CACHE_FILE);
            await GoToServerAsync(
                wrapper.Payload.Total,
                videoLinks);

            return;
        }
    }
    // Если нет - скачиваем все ссылки и названия в файл
    else
    {
        await GoToServerAsync(
            wrapper.Payload.Total,
            videoLinks);

        return;
    }
}
#endregion