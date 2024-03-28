namespace Bars.GkhDi.Import.Data
{
    public class Section8
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronomyc { get; set; }

        public string Type { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            Surname = parts[0];
            Name = parts[1];
            Patronomyc = parts[2];
            Type = parts[3];
            Phone = parts[4];
            Email = parts[5];
        }
    }
}

