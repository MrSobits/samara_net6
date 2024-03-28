namespace Bars.GkhGji.Regions.Tomsk.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

    public class TomskAppealCitsMap : JoinedSubClassMap<TomskAppealCits>
    {
        public TomskAppealCitsMap()
            : base("TomskAppealCits", "TOMSK_GJI_APPEAL_CITIZENS")
        {
        }

        protected override void Map()
        {
            Property(x => x.Comment, "COMMENT").Column("COMMENT").Length(2000);
        }
    }
}