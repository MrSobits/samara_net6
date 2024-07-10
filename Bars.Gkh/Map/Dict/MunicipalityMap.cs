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
            Reference(x => x.ParentMo, "Муниципальный район").Column("PARENT_MO_ID").Fetch();
        }
    }
}
