/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount;
/// 
///     public class PeriodSummaryBalanceChangeMap : BaseImportableEntityMap<PeriodSummaryBalanceChange>
///     {
///         public PeriodSummaryBalanceChangeMap()
///             : base("REGOP_SUMMARY_SALDO_CHANGE")
///         {
///             Map(x => x.CurrentValue, "CURRENT_VAL");
///             Map(x => x.NewValue, "NEW_VAL");
///             Map(x => x.Reason, "REASON");
///             Map(x => x.TransferGuid, "GUID");
///             Map(x => x.OperationDate, "OPER_DATE");
/// 
///             References(x => x.Document, "DOC_ID", ReferenceMapConfig.Fetch);
///             References(x => x.PeriodSummary, "SUMMARY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Изменение исходящего сальдо счета"</summary>
    public class PeriodSummaryBalanceChangeMap : BaseImportableEntityMap<PeriodSummaryBalanceChange>
    {
        
        public PeriodSummaryBalanceChangeMap() : 
                base("Изменение исходящего сальдо счета", "REGOP_SUMMARY_SALDO_CHANGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PeriodSummary, "Ситуация за период по лицевому счету").Column("SUMMARY_ID").NotNull().Fetch();
            Property(x => x.OperationDate, "Дата операции").Column("OPER_DATE");
            Property(x => x.CurrentValue, "Значение исходящего сальдо перед изменением").Column("CURRENT_VAL");
            Property(x => x.NewValue, "Новое значение исходящего сальдо").Column("NEW_VAL");
            Reference(x => x.Document, "Документ-основание для изменения").Column("DOC_ID").Fetch();
            Property(x => x.Reason, "Причина изменения").Column("REASON").Length(250);
            Property(x => x.TransferGuid, "Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer").Column("GUID").Length(250);
        }
    }
}
