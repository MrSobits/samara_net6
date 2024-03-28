namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section17
    {
        public string CodeErc { get; set; }

        public string ServiceDeviceCode { get; set; }

        public string ServiceDevice => GetServiceCode(ServiceDeviceCode);

        public string NumDevice { get; set; }

        public string UnitsDeviceCode { get; set; }

        public string UnitsDevice => GetUnitsCode(UnitsDeviceCode);

        public DateTime? InstallDateDevice { get; set; }

        public DateTime? CheckDateDevice { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            this.CodeErc = parts[0];
            this.ServiceDeviceCode = parts[1];
            this.NumDevice = parts[2];
            this.UnitsDeviceCode = parts[3];
            this.InstallDateDevice = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDateTime().To<DateTime?>() : null;
            this.CheckDateDevice = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDateTime().To<DateTime?>() : null;
        }

        private static string GetServiceCode(string codeForFile)
        {
            switch (codeForFile)
            {
                case "1":
                    return "3";
                case "2":
                    return "6";
                case "4":
                    return "1";
                case "5":
                    return "2";
                case "6":
                    return "4";
                case "19":
                    return "5";
            }

            return string.Empty;
        }

        private static string GetUnitsCode(string codeForFile)
        {
            switch (codeForFile)
            {
                case "1":
                    return "26";
                case "3":
                    return "23";
                case "5":
                    return "22";
                case "8":
                    return "4";
                case "9":
                    return "23";
            }

            return string.Empty;
        }
    }
}