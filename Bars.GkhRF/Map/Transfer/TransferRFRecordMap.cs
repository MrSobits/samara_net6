/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Запись перечисление средств рег. фонда"
///     /// </summary>
///     public class TransferRfRecordMap : BaseDocumentMap<TransferRfRecord>
///     {
///         public TransferRfRecordMap() : base("RF_TRANSFER_RECORD")
///         {
///             Map(x => x.TransferDate, "TRANSFER_DATE");
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.TransferRf, "TRANSFER_RF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Запись перечисление средств рег. фонда"</summary>
    public class TransferRfRecordMap : BaseEntityMap<TransferRfRecord>
    {
        
        public TransferRfRecordMap() : 
                base("Запись перечисление средств рег. фонда", "RF_TRANSFER_RECORD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentName, "DocumentName").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM");
            Property(x => x.TransferDate, "Дата перечисления").Column("TRANSFER_DATE");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.IsCalculation, "IsCalculation").Column("IS_CALCULATING").NotNull();

            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
            Reference(x => x.TransferRf, "Перечисление средств рег. фонда").Column("TRANSFER_RF_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
