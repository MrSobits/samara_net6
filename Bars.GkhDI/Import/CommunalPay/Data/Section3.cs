namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section3
    {
        public string CodeErc { get; set; }

        public string CodeCommunalPay { get; set; }

        public string Code
        {
            get { return Section3.GetCode(this.CodeCommunalPay); }
        }

        public decimal? Tariff { get; set; }

        public string UnitMeasure { get; set; }

        public decimal? VolumeFull { get; set; }

        public string Provider { get; set; }

        public string ProviderInn { get; set; }

        public string ProviderKpp { get; set; }

        public DateTime? TariffBegin { get; set; }

        public string DogNum { get; set; }

        public DateTime? DogDate { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            this.CodeErc = parts[0];

            this.CodeCommunalPay = parts[1];
            this.Tariff = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToDecimal().To<decimal?>() : null;
            this.UnitMeasure = parts[3];
            this.VolumeFull = !string.IsNullOrEmpty(parts[6]) ? parts[6].ToDouble().To<decimal?>() : null;
            this.Provider = parts[12];
            this.ProviderInn = parts[13];
            this.ProviderKpp = parts[14];
            this.DogNum = parts[15];
            this.DogDate = !string.IsNullOrEmpty(parts[16]) ? parts[16].To<DateTime?>() : null;
            this.TariffBegin = !string.IsNullOrEmpty(parts[17]) ? parts[17].To<DateTime?>() : null;
        }

        private static string GetCode(string codeForFile)
        {
            switch (codeForFile)
            {
                case "205":
                    return "1";
                case "17":
                    return "2";
                case "18":
                    return "6";
                case "19":
                    return "7";
                case "2":
                    return "8";
                case "22":
                    return "9";
                case "209":
                    return "10";
                case "21":
                    return "11";
                case "23":
                    return "12";
                case "20":
                    return "13";
                case "16":
                    return "14";
                case "206":
                    return "16";
                case "6":
                    return "17";
                case "9":
                    return "18";
                case "7":
                    return "19";
                case "25":
                    return "20";
                case "10":
                    return "21";
                case "8":
                    return "22";
                case "244":
                    return "23";
                case "245":
                    return "24";
                case "394":
                    return "25";
                case "266":
                    return "26";
                case "243":
                    return "27";
                case "5":
                    return "28";
                case "24":
                    return "29";
                case "26":
                    return "30";
                case "14":
                    return "33";
                case "-":
                    return "34";
            }

            return string.Empty;
        }
    }
}