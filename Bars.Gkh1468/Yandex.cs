namespace Bars.Gkh1468
{
    using System.Net;
    using System.Text;

    using Newtonsoft.Json.Linq;

    public class Yandex
    {
        private static string geocodeFormat = "http://geocode-maps.yandex.ru/1.x/?geocode={0}&format=json";

        public static string GetCoordinates(string address)
        {
            var requestStr = string.Format(geocodeFormat, address);
            string response;

            using (var cl = new WebClient())
            {
                cl.Encoding = Encoding.UTF8;
                response = cl.DownloadString(requestStr);
            }

            var jObj = JObject.Parse(response);

            var members = jObj.Try("response").Try("GeoObjectCollection").Try("featureMember") as JArray;

            if (members != null)
            {
                var member = members.Last;
                var point = member.Try("GeoObject").Try("Point").Try("pos") as JValue;

                if (point != null && point.Value != null)
                {
                    return point.Value.ToString();
                }
            }

            return string.Empty;
        }
    }

    public static class JTokenExtensions
    {
        public static JToken Try(this JToken token, string member)
        {
            return (token != null && token[member] != null) ? token[member] : new JObject();
        }
    }
}