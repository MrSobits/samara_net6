namespace Bars.GkhDi.Import.Data
{
    using Bars.B4.Utils;

    public class Section16
    {
        public string CodeErc { get; set; }

        public decimal? ChargeSumManProfit { get; set; }

        public int? ConstrYear { get; set; }

        public int? FloorMax { get; set; }

        public int? FloorMin { get; set; }

        public int? EntranceNum { get; set; }

        public int? ElevNum { get; set; }

        public int? FlatNum { get; set; }

        public int? NonResidNum { get; set; }

        public decimal? ResidSq { get; set; }

        public decimal? NonResidSq { get; set; }

        public decimal? CommunSq { get; set; }

        public string WallMaterialCode { get; set; }

        public string WallMaterial => GetCode(this.WallMaterialCode);

        public int? ChuteNum { get; set; }

        public decimal? UserPrepay { get; set; }

        public decimal? UserDebt { get; set; }

        public decimal? RoutineMaintenCharge { get; set; }

        public decimal? MaintenCharge { get; set; }

        public decimal? RoutineCharge { get; set; }

        public decimal? ManagCharge { get; set; }

        public decimal? GetChargeAll { get; set; }

        public decimal? GetChargeRent { get; set; }

        public decimal? Budget { get; set; }

        public decimal? UserPrepayEnd { get; set; }

        public decimal? UserDebtEnd { get; set; }

        public int? ClaimNum { get; set; }

        public int? StatisfClaimNum { get; set; }

        public decimal? ClaimRecalcSum { get; set; }

        public decimal? UserCommPrepayBegin { get; set; }

        public decimal? UserCommDebtBegin { get; set; }

        public decimal? UserCommPrepayEnd { get; set; }

        public decimal? UserCommDebtEnd { get; set; }

        public decimal? ComRecalcSum { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            this.CodeErc = parts[0];
            this.ChargeSumManProfit = !string.IsNullOrEmpty(parts[1]) ? parts[1].ToDecimal().To<decimal?>() : null;
            this.ConstrYear = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToInt().To<int?>() : null;
            this.FloorMax = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToInt().To<int?>() : null;
            this.FloorMin = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToInt().To<int?>() : null;
            this.EntranceNum = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToInt().To<int?>() : null;
            this.ElevNum = !string.IsNullOrEmpty(parts[6]) ? parts[6].ToInt().To<int?>() : null;
            this.FlatNum = !string.IsNullOrEmpty(parts[7]) ? parts[7].ToInt().To<int?>() : null;
            this.NonResidNum = !string.IsNullOrEmpty(parts[8]) ? parts[8].ToInt().To<int?>() : null;
            this.ResidSq = !string.IsNullOrEmpty(parts[9]) ? parts[9].ToDecimal().To<decimal?>() : null;
            this.NonResidSq = !string.IsNullOrEmpty(parts[10]) ? parts[10].ToDecimal().To<decimal?>() : null;
            this.CommunSq = !string.IsNullOrEmpty(parts[11]) ? parts[11].ToDecimal().To<decimal?>() : null;
            this.WallMaterialCode = parts[12];
            this.ChuteNum = !string.IsNullOrEmpty(parts[13]) ? parts[13].ToInt().To<int?>() : null;
            this.UserPrepay = !string.IsNullOrEmpty(parts[14]) ? parts[14].ToDecimal().To<decimal?>() : null;
            this.UserDebt = !string.IsNullOrEmpty(parts[15]) ? parts[15].ToDecimal().To<decimal?>() : null;
            this.RoutineMaintenCharge = !string.IsNullOrEmpty(parts[16]) ? parts[16].ToDecimal().To<decimal?>() : null;
            this.MaintenCharge = !string.IsNullOrEmpty(parts[17]) ? parts[17].ToDecimal().To<decimal?>() : null;
            this.RoutineCharge = !string.IsNullOrEmpty(parts[18]) ? parts[18].ToDecimal().To<decimal?>() : null;
            this.ManagCharge = !string.IsNullOrEmpty(parts[19]) ? parts[19].ToDecimal().To<decimal?>() : null;
            this.GetChargeAll = !string.IsNullOrEmpty(parts[20]) ? parts[20].ToDecimal().To<decimal?>() : null;
            this.GetChargeRent = !string.IsNullOrEmpty(parts[21]) ? parts[21].ToDecimal().To<decimal?>() : null;
            this.Budget = !string.IsNullOrEmpty(parts[22]) ? parts[22].ToDecimal().To<decimal?>() : null;
            this.UserPrepayEnd = !string.IsNullOrEmpty(parts[23]) ? parts[23].ToDecimal().To<decimal?>() : null;
            this.UserDebtEnd = !string.IsNullOrEmpty(parts[24]) ? parts[24].ToDecimal().To<decimal?>() : null;
            this.ClaimNum = !string.IsNullOrEmpty(parts[25]) ? parts[25].ToInt().To<int?>() : null;
            this.StatisfClaimNum = !string.IsNullOrEmpty(parts[26]) ? parts[26].ToInt().To<int?>() : null;
            this.ClaimRecalcSum = !string.IsNullOrEmpty(parts[27]) ? parts[27].ToDecimal().To<decimal?>() : null;
            this.UserCommPrepayBegin = !string.IsNullOrEmpty(parts[28]) ? parts[28].ToDecimal().To<decimal?>() : null;
            this.UserCommDebtBegin = !string.IsNullOrEmpty(parts[29]) ? parts[29].ToDecimal().To<decimal?>() : null;
            this.UserCommPrepayEnd = !string.IsNullOrEmpty(parts[30]) ? parts[30].ToDecimal().To<decimal?>() : null;
            this.UserCommDebtEnd = !string.IsNullOrEmpty(parts[31]) ? parts[31].ToDecimal().To<decimal?>() : null;
            this.ComRecalcSum = !string.IsNullOrEmpty(parts[32]) ? parts[32].ToDecimal().To<decimal?>() : null;
        }

        private static string GetCode(string codeForFile)
        {
            switch (codeForFile)
            {
                case "1":
                    return "2";
                case "2":
                    return "1";
                case "3":
                    return "4";
                case "4":
                    return "3";
            }

            return string.Empty;
        }
    }
}