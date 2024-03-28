namespace Bars.GkhDi.Import.Data
{
    using Bars.B4.Utils;

    public class Section19
    {
        public string CodeErc { get; set; }

        public string CodeCommunalPay { get; set; }

        public string Code
        {
            get { return Section19.GetCode(this.CodeCommunalPay); }
        }

        public decimal? MeterBegin { get; set; }

        public decimal? MeterEnd { get; set; }

        public decimal? ConsVol { get; set; }

        public decimal? AssessedCons { get; set; }

        public decimal? PaidCons { get; set; }

        public decimal? DebtCons { get; set; }

        public decimal? AssessedSupp { get; set; }

        public decimal? PaidSupp { get; set; }

        public decimal? DebtSupp { get; set; }

        public decimal? ConsPenaltySum { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            this.CodeErc = parts[0];
            this.CodeCommunalPay = parts[1];
            this.MeterBegin = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToDecimal().To<decimal?>() : null;
            this.MeterEnd = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDecimal().To<decimal?>() : null;
            this.ConsVol = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDecimal().To<decimal?>() : null;
            this.AssessedCons = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDecimal().To<decimal?>() : null;
            this.PaidCons = !string.IsNullOrEmpty(parts[6]) ? parts[6].ToDecimal().To<decimal?>() : null;
            this.DebtCons = !string.IsNullOrEmpty(parts[7]) ? parts[7].ToDecimal().To<decimal?>() : null;
            this.AssessedSupp = !string.IsNullOrEmpty(parts[8]) ? parts[8].ToDecimal().To<decimal?>() : null;
            this.PaidSupp = !string.IsNullOrEmpty(parts[9]) ? parts[9].ToDecimal().To<decimal?>() : null;
            this.DebtSupp = !string.IsNullOrEmpty(parts[10]) ? parts[10].ToDecimal().To<decimal?>() : null;
            this.ConsPenaltySum = !string.IsNullOrEmpty(parts[11]) ? parts[11].ToDecimal().To<decimal?>() : null;
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