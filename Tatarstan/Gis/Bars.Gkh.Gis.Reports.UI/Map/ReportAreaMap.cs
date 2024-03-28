namespace Bars.Gkh.Gis.Reports.UI.Map
{
    using B4.Modules.Dapper.BaseMap;
    using DapperExtensions.Mapper;
    using Entities;

    public class ReportAreaMap : BaseObjectMap<ReportArea>
    {
        public ReportAreaMap() : base("SR_RAJON")
        {
            Map(x => x.Id).Column("KOD_RAJ").Key(KeyType.Identity);
            Map(x => x.Name).Column("RAJON");
        }
    }
}
