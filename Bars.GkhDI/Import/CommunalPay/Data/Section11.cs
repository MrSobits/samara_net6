namespace Bars.GkhDi.Import.Data
{
    public class Section11
    {
        public string CodeService { get; set; }

        public string NameService { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            CodeService = parts[0];
            NameService = parts[2];
        }
    }
}

