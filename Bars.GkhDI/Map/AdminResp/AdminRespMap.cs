/// <mapping-converter-backup>
/// using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     public class AdminRespMap : BaseDocumentMap<AdminResp>
///     {
///         public AdminRespMap()
///             : base("DI_ADMIN_RESP")
///         {
///             Map(x => x.AmountViolation, "AMOUNT_VIOLATION");
///             Map(x => x.SumPenalty, "SUM_PENALTY");
///             Map(x => x.DatePaymentPenalty, "DATE_PAYMENT_PENALTY");
///             Map(x => x.DateImpositionPenalty, "DATE_IMPOSITION_PENALTY");
///             Map(x => x.TypeViolation, "TYPE_VIOLATION").Length(2000);
///             Map(x => x.Actions, "ACTIONS").Length(2000);
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.SupervisoryOrg, "SUPERVISORY_ORG_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.AdminResp"</summary>
    public class AdminRespMap : BaseImportableEntityMap<AdminResp>
    {
        
        public AdminRespMap() : 
                base("Bars.GkhDi.Entities.AdminResp", "DI_ADMIN_RESP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.AmountViolation, "AmountViolation").Column("AMOUNT_VIOLATION");
            Property(x => x.SumPenalty, "SumPenalty").Column("SUM_PENALTY");
            Property(x => x.DatePaymentPenalty, "DatePaymentPenalty").Column("DATE_PAYMENT_PENALTY");
            Property(x => x.DateImpositionPenalty, "DateImpositionPenalty").Column("DATE_IMPOSITION_PENALTY");
            Property(x => x.TypeViolation, "TypeViolation").Column("TYPE_VIOLATION").Length(2000);
            Property(x => x.Actions, "Actions").Column("ACTIONS").Length(2000);
            Property(x => x.TypePerson, "TypePerson").Column("TYPE_PERSON").NotNull();
            Property(x => x.Fio, "Fio").Column("FIO").Length(300);
            Property(x => x.Position, "Position").Column("PSITION").Length(300);

            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.SupervisoryOrg, "SupervisoryOrg").Column("SUPERVISORY_ORG_ID").Fetch();
        }
    }
}
