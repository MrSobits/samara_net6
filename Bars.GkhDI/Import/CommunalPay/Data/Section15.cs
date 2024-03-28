namespace Bars.GkhDi.Import.Data
{
    using Bars.B4.Utils;

    public class Section15
    {
        public string KladrCode { get; set; }

        public string NumPost { get; set; }

        public string KorpusPost { get; set; }

        public string Fax { get; set; }

        public int? Staff { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            KladrCode = parts[0];
            NumPost = parts[1];
            KorpusPost = parts[2];
            Fax = parts[3];
            Staff = !string.IsNullOrEmpty(parts[4]) ? parts[4].To<int?>() : null;
        }
    }
}