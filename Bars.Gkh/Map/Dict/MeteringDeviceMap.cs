/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Прибор учета"
///     /// </summary>
///     public class MeteringDeviceMap : BaseGkhEntityMap<MeteringDevice>
///     {
///         public MeteringDeviceMap()
///             : base("GKH_DICT_METERING_DEVICE")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(1000);
///             Map(x => x.AccuracyClass, "ACCURACY_CLASS").Length(30);
///             Map(x => x.TypeAccounting, "TYPE_ACCOUNTING").Not.Nullable().CustomType<TypeAccounting>();
///             Map(x => x.ReplacementCost, "REPLACEMENT_COST");
///             Map(x => x.LifeTime, "LIFETIME");
///             Map(x => x.Group, "DEVICE_GROUP").CustomType<MeteringDeviceGroup>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Прибор учета"</summary>
    public class MeteringDeviceMap : BaseImportableEntityMap<MeteringDevice>
    {
        
        public MeteringDeviceMap() : 
                base("Прибор учета", "GKH_DICT_METERING_DEVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            Property(x => x.AccuracyClass, "Класс точности").Column("ACCURACY_CLASS").Length(30);
            Property(x => x.TypeAccounting, "Тип учета").Column("TYPE_ACCOUNTING").NotNull();
            Property(x => x.ReplacementCost, "Стоимость замены").Column("REPLACEMENT_COST");
            Property(x => x.LifeTime, "Срок эксплуатации").Column("LIFETIME");
            Property(x => x.Group, "Гpуппа").Column("DEVICE_GROUP");
        }
    }
}
