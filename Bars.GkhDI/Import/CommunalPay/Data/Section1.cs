namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section1
    {
        public string Inn { get; set; }

        public string Kpp { get; set; }

        public string Opf { get; set; }

        public string Name { get; set; }

        public string JuralAddressCode { get; set; }

        public string NumJural { get; set; }

        public string KorpusJural { get; set; }

        public string FactAddressCode { get; set; }

        public string NumFact { get; set; }

        public string KorpusFact { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Ogrn { get; set; }

        public int? OgrnYear { get; set; }

        public string OgrnAgency { get; set; }

        public string ManagerPostCode { get; set; }

        public string ManagerPost
        {
            get { return GetCode(this.ManagerPostCode); }
        }

        public string ManagerFamily { get; set; }

        public string ManagerName { get; set; }

        public string ManagerPatronomyc { get; set; }

        public string ManagerPhone { get; set; }

        public string ManagerEmail { get; set; }

        public DateTime? ManagerDate { get; set; }

        public string ControlType { get; set; }

        public string RukFamily { get; set; }

        public string RukName { get; set; }

        public string RukPatronomyc { get; set; }

        public string OfficialSite { get; set; }

        public string WorkTimeBlock { get; set; }

        public string DispWorkTimeBlock { get; set; }

        public string PriemWorkTimeBlock { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            Inn = parts[0];
            Kpp = parts[1];
            Opf = parts[2];
            Name = parts[3];
            JuralAddressCode = parts[4];
            NumJural = parts[5];
            KorpusJural = parts[6];
            FactAddressCode = parts[7];
            NumFact = parts[8];
            KorpusFact = parts[9];
            Email = parts[10];
            Phone = parts[11];
            Ogrn = parts[12];
            OgrnYear = !string.IsNullOrEmpty(parts[13]) ? parts[13].ToLong().To<int?>() : null;
            OgrnAgency = parts[14];
            ManagerPostCode = parts[15];
            ManagerFamily = parts[16];
            ManagerName = parts[17];
            ManagerPatronomyc = parts[18];
            ManagerPhone = parts[19];
            ManagerEmail = parts[20];
            ManagerDate = !string.IsNullOrEmpty(parts[21]) ? parts[21].To<DateTime?>() : null;
            ControlType = parts[22];
            RukFamily = parts[26];
            RukName = parts[27];
            RukPatronomyc = parts[28];
            OfficialSite = parts[29];
            WorkTimeBlock = parts[30];
            DispWorkTimeBlock = parts[31];
            PriemWorkTimeBlock = parts[33];
        }

        private static string GetCode(string codeForFile)
        {
            switch (codeForFile)
            {
                case "1":
                    return "3";
                case "2":
                    return "1";
                case "3":
                    return "2";
                case "4":
                    return "4";
                case "5":
                    return "5";
                case "6":
                    return "6";
            }

            return string.Empty;
        }
    }
}

