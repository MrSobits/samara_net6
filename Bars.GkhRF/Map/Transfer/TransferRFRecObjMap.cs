/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhRf.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Объект записи перечисления средств рег. фонда"
///     /// </summary>
///     public class TransferRfRecObjMap : BaseEntityMap<TransferRfRecObj>
///     {
///         public TransferRfRecObjMap() : base("RF_TRANSFER_REC_OBJ")
///         {
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.TransferRfRecord, "TRANSFER_RF_RECORD_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Объект записи перечисления средств рег. фонда"</summary>
    public class TransferRfRecObjMap : BaseEntityMap<TransferRfRecObj>
    {
        
        public TransferRfRecObjMap() : 
                base("Объект записи перечисления средств рег. фонда", "RF_TRANSFER_REC_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Sum, "Сумма").Column("SUM");
            Reference(x => x.TransferRfRecord, "Запись договора рег. фонда").Column("TRANSFER_RF_RECORD_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").Fetch();
        }
    }
}
