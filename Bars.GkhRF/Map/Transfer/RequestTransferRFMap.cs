/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
///     using Bars.GkhRf.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Заявка на перечисление средств"
///     /// </summary>
///     public class RequestTransferRfMap : BaseGkhEntityMap<RequestTransferRf>
///     {
///         public RequestTransferRfMap() : base("RF_REQUEST_TRANSFER")
///         {
///             Map(x => x.TypeProgramRequest, "TYPE_PROGRAM_REQUEST").Not.Nullable().CustomType<TypeProgramRequest>();
///             Map(x => x.Perfomer, "PERFOMER_NAME").Length(300);
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DateFrom, "DATE_FROM");
/// 
///             References(x => x.File, "FILE_ID").Fetch.Join();
/// 
///             References(x => x.ContractRf, "CONTRACT_RF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ProgramCr, "PROGRAM_CR_ID").Fetch.Join();
///             References(x => x.ContragentBank, "CONTRAGENT_BANK_ID").Fetch.Join();
///             References(x => x.ManagingOrganization, "MANAGING_ORGANIZATION_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Заявка на перечисление средств"</summary>
    public class RequestTransferRfMap : BaseEntityMap<RequestTransferRf>
    {
        
        public RequestTransferRfMap() : 
                base("Заявка на перечисление средств", "RF_REQUEST_TRANSFER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeProgramRequest, "Тип программы заявки перечисления рег.фонда").Column("TYPE_PROGRAM_REQUEST").NotNull();
            Property(x => x.Perfomer, "Исполнитель").Column("PERFOMER_NAME").Length(300);
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.ContractRf, "Договор рег. фонда").Column("CONTRACT_RF_ID").NotNull().Fetch();
            Reference(x => x.ProgramCr, "Программа кап. ремонта").Column("PROGRAM_CR_ID").Fetch();
            Reference(x => x.ContragentBank, "Банк контрагента").Column("CONTRAGENT_BANK_ID").Fetch();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAGING_ORGANIZATION_ID").Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
