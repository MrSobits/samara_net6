/// <mapping-converter-backup>
/// namespace Bars.B4.Modules.Analytics.Maps
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.Modules.Analytics.Entities;
/// 
///     /// <summary>
///     /// 
///     /// </summary>
///     public class DataSourceMap : BaseEntityMap<DataSource>
///     {
///         public DataSourceMap()
///             : base("AL_DATA_SOURCE")
///         {
///             References(x => x.Parent, "PARENT");
///             Map(x => x.Name, "NAME");
///             Map(x => x.OwnerType, "DATA_SOURCE_TYPE");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.ProviderKey, "PROVIDER_KEY");
///             Map(x => x.SystemFilterBytes, "SYS_FILTER_BYTES");
///             Map(x => x.DataFiletrBytes, "DATA_FILTER_BYTES");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.B4.Modules.Analytics.Map
{
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Mapping.Mappers;
    
    
    /// <summary>Маппинг для "Источник данных."</summary>
    public class DataSourceMap : BaseEntityMap<DataSource>
    {
        
        public DataSourceMap() : 
                base("Источник данных.", "AL_DATA_SOURCE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Название.").Column("NAME").Length(250);
            Property(x => x.OwnerType, "Тип источника данных.").Column("DATA_SOURCE_TYPE");
            Reference(x => x.Parent, "Parent").Column("PARENT");
            Property(x => x.Description, "Описание.").Column("DESCRIPTION").Length(250);
            Property(x => x.ProviderKey, "Ключ родительского поставщика данных").Column("PROVIDER_KEY").Length(250);
            Property(x => x.DataFiletrBytes, "Сериализованный для хранения в БД.").Column("DATA_FILTER_BYTES");
            Property(x => x.SystemFilterBytes, "Сериализованный для хранения в БД.").Column("SYS_FILTER_BYTES");
        }
    }
}
