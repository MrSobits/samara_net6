/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Фото-архив жилого дома"
///     /// </summary>
///     public class RealityObjectImageMap : BaseGkhEntityMap<RealityObjectImage>
///     {
///         public RealityObjectImageMap() : base("GKH_OBJ_IMAGE")
///         {
///             Map(x => x.ImagesGroup, "IMAGES_GROUP").Not.Nullable().CustomType<ImagesGroup>();
///             Map(x => x.DateImage, "DATE_IMAGE");
///             Map(x => x.Description, "DESCRIPTION").Length(300);
///             Map(x => x.Name, "NAME").Length(100);
///             Map(x => x.ToPrint, "TO_PRINT");
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Period, "PERIOD_ID").Fetch.Join();
///             References(x => x.WorkCr, "WORK_ID").Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Фото-архив жилого дома"</summary>
    public class RealityObjectImageMap : BaseImportableEntityMap<RealityObjectImage>
    {
        
        public RealityObjectImageMap() : 
                base("Фото-архив жилого дома", "GKH_OBJ_IMAGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ImagesGroup, "Группа").Column("IMAGES_GROUP").NotNull();
            Property(x => x.DateImage, "Дата изображения").Column("DATE_IMAGE");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(300);
            Property(x => x.Name, "Наименование").Column("NAME").Length(100);
            Property(x => x.ToPrint, "Выводить на печать").Column("TO_PRINT");
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Period, "Период").Column("PERIOD_ID").Fetch();
            Reference(x => x.WorkCr, "Вид работы").Column("WORK_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
