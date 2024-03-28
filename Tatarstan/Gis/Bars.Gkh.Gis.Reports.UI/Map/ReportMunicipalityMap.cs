namespace Bars.Gkh.Gis.Reports.UI.Map
{
    using B4.Modules.Dapper.BaseMap;
    using DapperExtensions.Mapper;
    using Entities;

    public class ReportMunicipalityMap : BaseObjectMap<ReportMunicipality>
    {
        public ReportMunicipalityMap()
            : base("REPORT_COLLECTION")
        {
            Map(x => x.Area.Id).Column("RAJON_CODE").Key(KeyType.Identity);
            Map(x => x.Name).Column("VILL");
        }
    }
}
