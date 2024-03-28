/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhRf.Entities;
/// 
///     public class ViewTransferRfRecordMap : PersistentObjectMap<ViewTransferRfRecord>
///     {
///         public ViewTransferRfRecordMap()
///             : base("VIEW_RF_TRANSFER_RECORD")
///         {
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.DocumentName, "DOCUMENT_NAME");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.TransferDate, "TRANSFER_DATE");
///             Map(x => x.CountRecords, "COUNT");
///             Map(x => x.SumRecords, "SUM");
/// 
///             References(x => x.TransferRf, "TRANSFER_RF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Вьюха на запись договора рег. фонда"</summary>
    public class ViewTransferRfRecordMap : PersistentObjectMap<ViewTransferRfRecord>
    {
        
        public ViewTransferRfRecordMap() : 
                base("Вьюха на запись договора рег. фонда", "VIEW_RF_TRANSFER_RECORD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM");
            Property(x => x.TransferDate, "Дата перечисления").Column("TRANSFER_DATE");
            Property(x => x.CountRecords, "Количество").Column("COUNT");
            Property(x => x.SumRecords, "Сумма").Column("SUM");
            Reference(x => x.TransferRf, "Перечисление рег. фонда").Column("TRANSFER_RF_ID").NotNull().Fetch();
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
