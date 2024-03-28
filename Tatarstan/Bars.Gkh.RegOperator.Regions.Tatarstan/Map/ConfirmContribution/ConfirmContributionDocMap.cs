/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Учет платежных документов по начислениям и оплатам на КР"
///     /// </summary>
///     public class ConfirmContributionDocMap : BaseEntityMap<ConfirmContributionDoc>
///     {
///         public ConfirmContributionDocMap()
///             : base("REGOP_CONFCONTRIB_DOC")
///         {
///             References(x => x.ConfirmContribution, "CONFIRMCONTRIB_ID", ReferenceMapConfig.NotNull);
///             References(x => x.RealityObject, "REALOBJ_ID");
///             References(x => x.Scan, "SCAN_ID", ReferenceMapConfig.Fetch);
/// 
///             Map(x => x.DocumentNum, "DOCUMENT_NUM", false, 300);
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.TransferDate, "TRANSFER_DATE");
///             Map(x => x.Amount, "AMOUNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.ConfirmContributionDoc"</summary>
    public class ConfirmContributionDocMap : BaseEntityMap<ConfirmContributionDoc>
    {
        
        public ConfirmContributionDocMap() : 
                base("Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.ConfirmContributionDoc", "REGOP_CONFCONTRIB_DOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ConfirmContribution, "ConfirmContribution").Column("CONFIRMCONTRIB_ID").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALOBJ_ID");
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.TransferDate, "TransferDate").Column("TRANSFER_DATE");
            Reference(x => x.Scan, "Scan").Column("SCAN_ID").Fetch();
            Property(x => x.Amount, "Amount").Column("AMOUNT");
        }
    }
}
