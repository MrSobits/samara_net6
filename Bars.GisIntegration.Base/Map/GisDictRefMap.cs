namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>Маппинг для "Bars.GisIntegration.Base.Entities.GisDictRef"</summary>
    public class GisDictRefMap : BaseEntityMap<GisDictRef>
    {
        /// <summary>
        /// Конструктор типа GisDictRefMap
        /// </summary>
        public GisDictRefMap() : base("Bars.GisIntegration.Base.Entities.GisDictRef", "GI_INTEGR_REF_DICT")
        {
        }
        
        protected override void Map()
        {
            //this.Property(x => x.ClassName, "ClassName").Column("CLASS_NAME").Length(1000);
            this.Property(x => x.GkhId, "GkhId").Column("GKH_REC_ID");
            this.Property(x => x.GkhName, "GkhName").Column("GKH_REC_NAME").Length(1000);
            this.Property(x => x.GisCode, "GisCode").Column("GIS_REC_ID").Length(10);
            this.Property(x => x.GisGuid, "GisGuid").Column("GIS_REC_GUID").Length(50);
            this.Property(x => x.GisName, "GisName").Column("GIS_REC_NAME").Length(1000);
            this.Reference(x => x.Dict, "Dict").Column("DICT_ID").NotNull().Fetch();
        }
    }
}