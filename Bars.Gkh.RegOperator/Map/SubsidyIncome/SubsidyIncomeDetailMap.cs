/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     /// <summary>
///     /// мапинг для Реестра субсидий
///     /// </summary>
///     public class SubsidyIncomeDetailMap : BaseImportableEntityMap<SubsidyIncomeDetail>
///     {
///         public SubsidyIncomeDetailMap()
///             : base("REGOP_SUBSIDY_INC_DET")
///         {
///             Map(x => x.DateReceipt, "DATE_RECEIPT");
///             Map(x => x.RealObjId, "IMP_REAL_OBJ_ID");
///             Map(x => x.RealObjAddress, "REAL_OBJ_ADR");
///             Map(x => x.TypeSubsidyDistr, "SUBSIDY_DISTR_TYPE");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.IsConfirmed, "IS_CONFIRMED", true, false);
///             References(x => x.SubsidyIncome, "SUBSIDY_INCOME_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Запись реестр субсидий"</summary>
    public class SubsidyIncomeDetailMap : BaseImportableEntityMap<SubsidyIncomeDetail>
    {
        
        public SubsidyIncomeDetailMap() : 
                base("Запись реестр субсидий", "REGOP_SUBSIDY_INC_DET")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SubsidyIncome, "SubsidyIncome").Column("SUBSIDY_INCOME_ID").Fetch();
            Property(x => x.RealObjId, "ID дома в файле").Column("IMP_REAL_OBJ_ID");
            Property(x => x.RealObjAddress, "Адрес в файле").Column("REAL_OBJ_ADR").Length(250);
            Reference(x => x.RealityObject, "Жилой дом").Column("REAL_OBJ_ID").Fetch();
            Property(x => x.TypeSubsidyDistr, "Тип субсидии").Column("SUBSIDY_DISTR_TYPE").Length(250);
            Property(x => x.Sum, "Сумма").Column("SUM");
            Property(x => x.DateReceipt, "Дата поступления").Column("DATE_RECEIPT");
            Property(x => x.IsConfirmed, "Определен/Не определен").Column("IS_CONFIRMED").DefaultValue(false).NotNull();
        }
    }
}
