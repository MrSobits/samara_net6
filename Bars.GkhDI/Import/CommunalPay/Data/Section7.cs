namespace Bars.GkhDi.Import.Data
{
    public class Section7
    {
        public string DocumentType { get; set; }

        public string File { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            DocumentType = parts[0];
            File = parts[1];
        }
    }
}

