namespace Bars.GkhDi.Import.Data
{
    using Bars.B4.Utils;

    public class Section4
    {
        public string CodeErc { get; set; }

        public string CodeCommunalPay { get; set; }

        public string Code
        {
            get
            {
                return GetCode(CodeCommunalPay);
            }
        }

        public decimal? RecalculationSum { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            CodeErc = parts[0];
            CodeCommunalPay = parts[1];
            RecalculationSum = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDecimal().To<decimal?>() : null;
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

