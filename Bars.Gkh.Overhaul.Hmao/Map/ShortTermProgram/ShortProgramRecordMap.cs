/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class ShortProgramRecordMap : BaseImportableEntityMap<ShortProgramRecord>
///     {
///         public ShortProgramRecordMap()
///             : base("OVRHL_SHORT_PROG_REC")
///         {
///             Map(x => x.OwnerSumForCr, "OWNER_SUM_CR", true, 0);
///             Map(x => x.BudgetRegion, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipality, "BUDGET_MU", true, 0);
///             Map(x => x.BudgetFcr, "BUDGET_FSR", true, 0);
///             Map(x => x.BudgetOtherSource, "BUDGET_OTHER_SRC", true, 0);
///             Map(x => x.Difitsit, "DIFITSIT", true, 0);
///             Map(x => x.Year, "YEAR", true, 0);
/// 
///             References(x => x.Stage2, "STAGE2_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Запись краткосрочной программы в которой бует подробно разбиение из каких Бюджетов будет финансирвоатся выполнение работ по всему ООИ"</summary>
    public class ShortProgramRecordMap : BaseImportableEntityMap<ShortProgramRecord>
    {
        
        public ShortProgramRecordMap() : 
                base("Запись краткосрочной программы в которой бует подробно разбиение из каких Бюджето" +
                        "в будет финансирвоатся выполнение работ по всему ООИ", "OVRHL_SHORT_PROG_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Ссылка на Дом").Column("REALITY_OBJECT_ID").Fetch();
            Reference(x => x.Stage2, "Ссылка на Корректировку ДПКР (это считается связь с долгосрочной программой)").Column("STAGE2_ID").Fetch();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.OwnerSumForCr, "Из Средств собственников на кап. ремонт").Column("OWNER_SUM_CR").NotNull();
            Property(x => x.BudgetFcr, "Из Бюджета ФСР").Column("BUDGET_FSR").NotNull();
            Property(x => x.BudgetOtherSource, "Из Бюджета других источников").Column("BUDGET_OTHER_SRC").NotNull();
            Property(x => x.BudgetRegion, "Из Бюджета региона").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "Из Бюджета МО").Column("BUDGET_MU").NotNull();
            Property(x => x.Difitsit, "Дифицит для конкретного Этого ООИ, который считается по формуле = DpkrCorrection." +
                    "Sum - OwnerSumForCR - BudgetFcr - BudgetOtherSource").Column("DIFITSIT").NotNull();
        }
    }
}
