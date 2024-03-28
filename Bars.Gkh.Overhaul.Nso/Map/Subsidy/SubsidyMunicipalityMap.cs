/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class SubsidyMunicipalityMap : BaseEntityMap<SubsidyMunicipality>
///     {
///         public SubsidyMunicipalityMap()
///             : base("OVRHL_SUBSIDY_MU")
///         {
///             Map(x => x.StartTarif, "START_TARIF", true, 0);
///             Map(x => x.CoefGrowthTarif, "COEF_GROWTH_TARIF", true, 0);
///             Map(x => x.CoefSumRisk, "COEF_SUM_RISK", true, 0);
///             Map(x => x.DateReturnLoan, "DATE_RETURN_LOAN", true, 0);
///             Map(x => x.CoefAvgInflationPerYear, "COEF_AVG_INFL", true, 0);
///             Map(x => x.ConsiderInflation, "CONSIDER_INFL", true, false);
///             Map(x => x.CalculationCompleted, "CALC_COMPLETED", true, false);
///             Map(x => x.DpkrCorrected, "DPKR_CORRECTED", true, false);
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.SubsidyMunicipality"</summary>
    public class SubsidyMunicipalityMap : BaseEntityMap<SubsidyMunicipality>
    {
        
        public SubsidyMunicipalityMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.SubsidyMunicipality", "OVRHL_SUBSIDY_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Property(x => x.StartTarif, "StartTarif").Column("START_TARIF").NotNull();
            Property(x => x.CoefGrowthTarif, "CoefGrowthTarif").Column("COEF_GROWTH_TARIF").NotNull();
            Property(x => x.CoefAvgInflationPerYear, "CoefAvgInflationPerYear").Column("COEF_AVG_INFL").NotNull();
            Property(x => x.ConsiderInflation, "ConsiderInflation").Column("CONSIDER_INFL").DefaultValue(false).NotNull();
            Property(x => x.CoefSumRisk, "CoefSumRisk").Column("COEF_SUM_RISK").NotNull();
            Property(x => x.DateReturnLoan, "DateReturnLoan").Column("DATE_RETURN_LOAN").NotNull();
            Property(x => x.CalculationCompleted, "CalculationCompleted").Column("CALC_COMPLETED").DefaultValue(false).NotNull();
            Property(x => x.DpkrCorrected, "DpkrCorrected").Column("DPKR_CORRECTED").DefaultValue(false).NotNull();
        }
    }
}
