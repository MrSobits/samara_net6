namespace Bars.GkhDi.Navigation
{
    using Newtonsoft.Json;

    public class ManagingOrgDataMenuItem
    {
        public ManagingOrgDataMenuItem(string text, string controller, string percent)
        {
            Text = text;
            Controller = controller;
            Percent = percent;
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("controller")]
        public string Controller { get; set; }

        [JsonProperty("percent")]
        public string Percent { get; set; }
    }
}