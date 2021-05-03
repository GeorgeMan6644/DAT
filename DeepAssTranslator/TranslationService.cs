using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeepAssTranslator
{
    public static class TranslationService
    {
        public static async Task TranslateTextsAsync(List<Translation> textsForTranslation)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                List<Translation> waitingTextsForTranslation = textsForTranslation;

                while (waitingTextsForTranslation.Any())
                {
                    Console.Write('.');
                    List<Translation> takenTextsForTranslation = waitingTextsForTranslation.Take(50).ToList();

                    using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), Configuration.GetApiEndpoint()))
                    {
                        List<string> contentList = new List<string>();
                        contentList.Add($"auth_key={Configuration.AuthenticationKey}");

                        foreach (Translation translation in takenTextsForTranslation)
                        {
                            contentList.Add($"text={translation.TextForTranslate}");
                        }

                        contentList.Add($"target_lang={Configuration.TargetLang}");
                        if (!string.IsNullOrEmpty(Configuration.SourceLang))
                        {
                            contentList.Add($"source_lang={Configuration.SourceLang}");
                        }

                        contentList.Add("tag_handling=xml");
                        contentList.Add("ignore_tags=ctrl1");

                        request.Content = new StringContent(string.Join("&", contentList));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                        HttpResponseMessage _response = await httpClient.SendAsync(request);
                        string content = await _response.Content.ReadAsStringAsync();
                        DeepLTranslation response = JsonSerializer.Deserialize<DeepLTranslation>(content);

                        int i = 0;
                        foreach (Translation translation in takenTextsForTranslation)
                        {
                            translation.TranslatedText = response.translations[i++].text;
                        }

                        waitingTextsForTranslation = waitingTextsForTranslation.Skip(50).ToList();
                    }
                }

                Console.WriteLine();
            };
        }

        public class DeepLTranslation
        {
            public Translation[] translations { get; set; }

            public class Translation
            {
                public string detected_source_language { get; set; }
                public string text { get; set; }
            }
        }
    }
}
