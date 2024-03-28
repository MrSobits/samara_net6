namespace Bars.GkhDi.Import.Data
{
    public class Section0
    {
        public string VersionFormat { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            VersionFormat = parts[3];
        }
    }
}
