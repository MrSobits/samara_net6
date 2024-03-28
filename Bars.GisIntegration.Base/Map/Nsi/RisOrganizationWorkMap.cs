namespace Bars.GisIntegration.Base.Map.Nsi
{
    using System.Collections.Generic;
    
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.GisIntegration.Base.Map;
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг сущности RisOrganizationWork
    /// </summary>
    public class RisOrganizationWorkMap : BaseRisEntityMap<RisOrganizationWork>
    {
        public RisOrganizationWorkMap()
            : base("Bars.Gkh.Ris.Entities.Nsi.RisOrganizationWork", "RIS_ORGANIZATION_WORK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(100);
            this.Property(x => x.ServiceTypeCode, "ServiceTypeCode").Column("SERVICE_TYPE_CODE").Length(20);
            this.Property(x => x.ServiceTypeGuid, "ServiceTypeGuid").Column("SERVICE_TYPE_GUID").Length(40);
            this.Property(x => x.RequiredServices, "RequiredServices").Column("REQUIRED_SERVICES");
            this.Property(x => x.Okei, "Okei").Column("OKEI").Length(3);
            this.Property(x => x.StringDimensionUnit, "StringDimensionUnit").Column("STRING_DIMENSION_UNIT").Length(100);
        }
    }

    public class RisOrganizationWorkNHibernateMapping : ClassMapping<RisOrganizationWork>
    {
        public RisOrganizationWorkNHibernateMapping()
        {
            this.Property(
                x => x.RequiredServices,
                mapper =>
                {
                    mapper.Type<ImprovedBinaryJsonType<List<RequiredService>>>();
                });
        }
    }
}