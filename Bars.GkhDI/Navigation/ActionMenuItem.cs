namespace Bars.GkhDi.Navigation
{
    using Newtonsoft.Json;

    public class ActionMenuItem
    {
        public ActionMenuItem(string text, string type, string percent)
        {
            Text = text;
            Type = type;
            Percent = percent;
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("percent")]
        public string Percent { get; set; }
    }
}