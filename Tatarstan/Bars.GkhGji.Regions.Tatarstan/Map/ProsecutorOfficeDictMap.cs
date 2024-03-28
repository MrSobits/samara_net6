namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class ProsecutorOfficeDictMap : BaseEntityMap<ProsecutorOfficeDict>
    {
        public ProsecutorOfficeDictMap() : 
            base("Справочник прокуратур", "GJI_DICT_PROSECUTOR_OFFICE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Type, "Тип прокуратуры").Column("TYPE");
            this.Property(x => x.Code, "Код прокуратуры").Column("CODE").NotNull();
            this.Property(x => x.ErknmCode, "Код ЕРКНМ").Column("ERKNM_CODE").Length(10);
            this.Property(x => x.Name, "Наименование прокуратуры").Column("NAME").Length(1024).NotNull();
            this.Property(x => x.FederalDistrictCode, "Код федерального округа").Column("FEDERAL_DISTRICT_CODE").NotNull();
            this.Property(x => x.FederalDistrictName, "Наименование федерального округа").Column("FEDERAL_DISTRICT_NAME").Length(300);
            this.Property(x => x.FederalCenterCode, "Код федерального центра").Column("FEDERAL_CENTER_CODE").NotNull();
            this.Property(x => x.FederalCenterName, "Наименование федерального центра").Column("FEDERAL_CENTER_NAME").Length(300);
            this.Property(x => x.DistrictCode, "Код района по версии ЕРП").Column("DISTRICT_CODE").Length(200);
            this.Reference(x => x.Parent, "Идентификатор родительской записи").Column("PARENT_ID");
            this.Property(x => x.OkatoTer, "Код региона (ОКАТО)").Column("OKATO_TER").Length(2);
            this.Property(x => x.OkatoKod1, "Код района (ОКАТО)").Column("OKATO_KOD1").Length(3);
            this.Property(x => x.OkatoKod2, "Код рабочего поселка/сельсовета (ОКАТО)").Column("OKATO_KOD2").Length(3);
            this.Property(x => x.OkatoKod3, "Код населенного пункта (ОКАТО)").Column("OKATO_KOD3").Length(3);
            this.Property(x => x.UseDefault, "Использовать по умолчанию").Column("USE_DEFAULT").NotNull().DefaultValue(false);
        }
    }
}