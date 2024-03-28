namespace Bars.Gkh.Integration.DataFetcher
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;

    using Bars.B4.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class WebClientFetcher
    {
        public dynamic GetData(HttpQueryBuilder builder)
        {
            var result = GetResult(builder);

            return result.IsEmpty() ? new JObject[0] : JsonConvert.DeserializeObject(result);
        }

        public T GetData<T>(HttpQueryBuilder builder)
        {
            var result = GetResult(builder);

            return result.IsEmpty() ? default(T) : JsonConvert.DeserializeObject<T>(result);
        }

        private string GetResult(HttpQueryBuilder builder)
        {
            string result;

            try
            {
                var request = (HttpWebRequest) WebRequest.Create(builder.ToString());
                request.Method = "GET";
                request.Timeout = Timeout.Infinite;
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (WebException)
            {
                if (builder.Address.IsEmpty())
                {
                    throw new WebException("Адрес системы не указан");
                }

                throw new WebException("Система интерграции по адресу: '{0}' не доступна".FormatUsing(builder.Address));
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }
    }
}