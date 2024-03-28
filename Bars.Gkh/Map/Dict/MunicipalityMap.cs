/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Муниципальное образование"
///     /// </summary>
///     public class MunicipalityMap : BaseGkhEntityMap<Municipality>
///     {
///         public MunicipalityMap()
///             : base("GKH_DICT_MUNICIPALITY")
///         {
///             Map(x => x.Code, "CODE").Length(30);
///             Map(x => x.FiasId, "FIAS_ID").Length(36);
///             Map(x => x.Group, "GRP").Length(30);
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Okato, "OKATO").Length(30);
///             Map(x => x.Description, "DESCRIPTION").Length(300);
///             Map(x => x.FederalNumber, "FEDERALNUMBER").Length(30);
///             Map(x => x.Cut, "CUT").Length(10);
/// 
///             Map(x => x.RegionName, "REGION_NAME").Length(200);
/// 
///             Map(x => x.MapGuid, "MAP_GUID").Length(36);
///             Map(x => x.PolygonPointsArray, "POLYGON_POINTS").Length(5000);
///             Map(x => x.CheckCertificateValidity, "CHECK_CERTIFICATE").Not.Nullable();
///             Map(x => x.Oktmo, "OKTMO");
///             Map(x => x.IsOld, "IS_OLD");
/// 
///             Map(x => x.Level, "TYPE_MO").Not.Nullable().CustomType<TypeMunicipality>();
///             References(x => x.ParentMo, "PARENT_MO_ID").Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Муниципальное образование"</summary>
    public class MunicipalityMap : BaseImportableEntityMap<Municipality>
    {
        
        public MunicipalityMap() : 
                base("Муниципальное образование", "GKH_DICT_MUNICIPALITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Code, "Код").Column("CODE").Length(30);
            Property(x => x.FiasId, "Guid ФИАС").Column("FIAS_ID").Length(36);
            Property(x => x.Group, "Группа").Column("GRP").Length(30);
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Okato, "ОКАТО").Column("OKATO").Length(30);
            Property(x => x.Description, "Описание, комментарий").Column("DESCRIPTION").Length(300);
            Property(x => x.FederalNumber, "Федеральный номер").Column("FEDERALNUMBER").Length(30);
            Property(x => x.Cut, "Сокращение").Column("CUT").Length(10);
            Property(x => x.RegionName, "Наименование региона").Column("REGION_NAME").Length(200);
            Property(x => x.MapGuid, "Гуид, полученный из карты").Column("MAP_GUID").Length(36);
            Property(x => x.PolygonPointsArray, "Массив координат полигона").Column("POLYGON_POINTS").Length(5000);
            Property(x => x.CheckCertificateValidity, "Проверять сертификат пользователей, отоносящихся к данному МО, при полдписывании").Column("CHECK_CERTIFICATE").NotNull();
            Property(x => x.Oktmo, "ОКТМО").Column("OKTMO");
            Property(x => x.IsOld, "Признак МО, было заведено до импорта 2х уровневого справочника").Column("IS_OLD");
            Property(x => x.Level, "Уровень").Column("TYPE_MO").NotNull();
            //Property(x => x.TorId, "Идентификатор в ТОР").Column("TOR_ID");
            Property(x => x.CodeGji, "Код ГЖИ").Column("CODE_GJI");
            Reference(x => x.ParentMo, "Муниципальный район").Column("PARENT_MO_ID").Fetch();
        }
    }
}
