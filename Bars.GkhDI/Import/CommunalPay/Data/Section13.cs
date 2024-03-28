namespace Bars.GkhDi.Import.Data
{
    using Bars.B4.Utils;

    public class Section13
    {
        public decimal? IncomeManagCategory { get; set; }

        public decimal? IncomeCommunal { get; set; }

        public decimal? DebtPopulationEndManagCategory { get; set; }

        public decimal? DebtPopulationStartManagCategory { get; set; }

        public decimal? DebtPopulationEndCommunal { get; set; }

        public decimal? DebtPopulationStartCommunal { get; set; }

        public decimal? PaidByMeteringDevice { get; set; }

        public decimal? PaidByGeneralNeeds { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            IncomeManagCategory = !string.IsNullOrEmpty(parts[0]) ? parts[0].ToDecimal().To<decimal?>() : null;
            IncomeCommunal = !string.IsNullOrEmpty(parts[1]) ? parts[1].ToDecimal().To<decimal?>() : null;
            DebtPopulationEndManagCategory = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToDecimal().To<decimal?>() : null;
            DebtPopulationStartManagCategory = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDecimal().To<decimal?>() : null;
            DebtPopulationEndCommunal = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDecimal().To<decimal?>() : null;
            DebtPopulationStartCommunal = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDecimal().To<decimal?>() : null;
            PaidByMeteringDevice = !string.IsNullOrEmpty(parts[6]) ? parts[6].ToDecimal().To<decimal?>() : null;
            PaidByGeneralNeeds = !string.IsNullOrEmpty(parts[7]) ? parts[7].ToDecimal().To<decimal?>() : null;
        }
    }
}

