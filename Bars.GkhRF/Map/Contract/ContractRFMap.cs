/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Договор рег. фонда"
///     /// </summary>
///     public class ContractRfMap : BaseGkhEntityMap<ContractRf>
///     {
///         public ContractRfMap() : base("RF_CONTRACT")
///         {
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DateBegin, "DATE_BEGIN");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             References(x => x.ManagingOrganization, "MANAG_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Договор рег. фонда"</summary>
    public class ContractRfMap : BaseEntityMap<ContractRf>
    {
        
        public ContractRfMap() : 
                base("Договор рег. фонда", "RF_CONTRACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DateBegin, "Дата начала действия").Column("DATE_BEGIN");
            Property(x => x.DateEnd, "Дата окончания действия").Column("DATE_END");
            Property(x => x.TerminationContractNum, "Нлмер расторжения договора").Column("TERMINATION_CONTRACT_NUM").Length(50); 
            Property(x => x.TerminationContractDate, "Дата расторжения договора").Column("TERMINATION_CONTRACT_DATE");
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAG_ORG_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.TerminationContractFile, "Файл расторжения договора").Column("TERMINATION_CONTRACT_FILE_ID").Fetch();
        }
    }
}
