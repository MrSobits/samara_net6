/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Фото акта обследования ГЖИ"
///     /// </summary>
///     public class ActSurveyPhotoMap : BaseEntityMap<ActSurveyPhoto>
///     {
///         public ActSurveyPhotoMap()
///             : base("GJI_ACTSURVEY_PHOTO")
///         {
///             Map(x => x.ImageDate, "IMAGE_DATE");
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.IsPrint, "IS_PRINT").Not.Nullable();
///             Map(x => x.Group, "IMAGE_GROUP").Not.Nullable().CustomType<ImageGroupSurveyGji>();
/// 
///             References(x => x.ActSurvey, "ACTSURVEY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "фото в акте обследования"</summary>
    public class ActSurveyPhotoMap : BaseEntityMap<ActSurveyPhoto>
    {
        
        public ActSurveyPhotoMap() : 
                base("фото в акте обследования", "GJI_ACTSURVEY_PHOTO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ImageDate, "Дата изображения").Column("IMAGE_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.IsPrint, "Выводить на печать?").Column("IS_PRINT").NotNull();
            Property(x => x.Group, "Группа").Column("IMAGE_GROUP").NotNull();
            Reference(x => x.ActSurvey, "Акт обследования").Column("ACTSURVEY_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
