using HttpUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Translation.OpenAI
{
    public class OpenAiCompatibleTranslator
    {
        private ILog _Logger;
        private HttpReader _HttpReader;

        public OpenAiCompatibleTranslator(ILog logger)
        {
            _Logger = logger;
            _HttpReader = new HttpReader(new Translation.HttpUtils.HttpILogWrapper(_Logger));
            _HttpReader.ContentType = "application/json";
        }

        public string Translate(string sentence, string inLang, string outLang)
        {
            try
            {
                if (string.IsNullOrEmpty(GlobalTranslationSettings.OpenAI_ApiKey))
                {
                    _Logger.WriteLog("OpenAI API Key is missing.");
                    return "Error: OpenAI API Key missing.";
                }

                _HttpReader.OptionalHeaders["Authorization"] = $"Bearer {GlobalTranslationSettings.OpenAI_ApiKey}";

                var payload = new
                {
                    model = GlobalTranslationSettings.OpenAI_Model,
                    messages = new[]
                    {
                        new { role = "system", content = $"You are a professional translator. Translate the following text from {inLang} to {outLang}. Only return the translated text, nothing else." },
                        new { role = "user", content = sentence }
                    },
                    temperature = 0.3
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                var endpoint = GlobalTranslationSettings.OpenAI_Endpoint;

                // Using the overload that supports data body
                var requestResult = _HttpReader.RequestWebData(endpoint, HttpMethods.POST, jsonPayload, false);

                if (requestResult.IsSuccessful)
                {
                    var response = JsonConvert.DeserializeObject<dynamic>(requestResult.Body);
                    string translatedText = response.choices[0].message.content;
                    return translatedText.Trim();
                }
                else
                {
                    _Logger.WriteLog($"OpenAI Error: {requestResult.Body ?? requestResult.InnerException?.Message}");
                    return $"Error: {requestResult.Body ?? "Unknown Error"}";
                }
            }
            catch (Exception e)
            {
                _Logger.WriteLog(e.ToString());
                return $"Error: {e.Message}";
            }
        }
    }
}
