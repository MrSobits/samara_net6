namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractOrg"</summary>
    public class RosRegExtractOrgMap : PersistentObjectMap<RosRegExtractOrg>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractOrgMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractOrg")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Org_Code_SP, "Org_Code_SP").Column("Org_Code_SP");
            this.Property(x => x.Org_Content, "Org_Content").Column("Org_Content");
            this.Property(x => x.Org_Code_OPF, "Org_Code_OPF").Column("Org_Code_OPF");
            this.Property(x => x.Org_Name, "Org_Name").Column("Org_Name");
            this.Property(x => x.Org_Inn, "Org_Inn").Column("Org_Inn");
            this.Property(x => x.Org_Code_OGRN, "Org_Code_OGRN").Column("Org_Code_OGRN");
            this.Property(x => x.Org_RegDate, "Org_RegDate").Column("Org_RegDate");
            this.Property(x => x.Org_AgencyRegistration, "Org_AgencyRegistration").Column("Org_AgencyRegistration");
            this.Property(x => x.Org_Code_CPP, "Org_Code_CPP").Column("Org_Code_CPP");
            this.Property(x => x.Org_Loc_ID_Address, "Org_Loc_ID_Address").Column("Org_Loc_ID_Address");
            this.Property(x => x.Org_Loc_Content, "Org_Loc_Content").Column("Org_Loc_Content");
            this.Property(x => x.Org_Loc_RegionCode, "Org_Loc_RegionCode").Column("Org_Loc_RegionCode");
            this.Property(x => x.Org_Loc_RegionName, "Org_Loc_RegionName").Column("Org_Loc_RegionName");
            this.Property(x => x.Org_Loc_Code_OKATO, "Org_Loc_Code_OKATO").Column("Org_Loc_Code_OKATO");
            this.Property(x => x.Org_Loc_Code_KLADR, "Org_Loc_Code_KLADR").Column("Org_Loc_Code_KLADR");
            this.Property(x => x.Org_Loc_CityName, "Org_Loc_CityName").Column("Org_Loc_CityName");
            this.Property(x => x.Org_Loc_StreetName, "Org_Loc_StreetName").Column("Org_Loc_StreetName");
            this.Property(x => x.Org_Loc_Level1Name, "Org_Loc_Level1Name").Column("Org_Loc_Level1Name");
            this.Property(x => x.Org_FLoc_ID_Address, "Org_FLoc_ID_Address").Column("Org_FLoc_ID_Address");
            this.Property(x => x.Org_FLoc_Content, "Org_FLoc_Content").Column("Org_FLoc_Content");
            this.Property(x => x.Org_FLoc_RegionCode, "Org_FLoc_RegionCode").Column("Org_FLoc_RegionCode");
            this.Property(x => x.Org_FLoc_RegionName, "Org_FLoc_RegionName").Column("Org_FLoc_RegionName");
            this.Property(x => x.Org_FLoc_Code_OKATO, "Org_FLoc_Code_OKATO").Column("Org_FLoc_Code_OKATO");
            this.Property(x => x.Org_FLoc_Code_KLADR, "Org_FLoc_Code_KLADR").Column("Org_FLoc_Code_KLADR");
            this.Property(x => x.Org_FLoc_CityName, "Org_FLoc_CityName").Column("Org_FLoc_CityName");
            this.Property(x => x.Org_FLoc_StreetName, "Org_FLoc_StreetName").Column("Org_FLoc_StreetName");
            this.Property(x => x.Org_FLoc_Level1Name, "Org_FLoc_Level1Name").Column("Org_FLoc_Level1Name");
        }
    }
    public class RosRegExtractOrgNhMaping : ClassMapping<RosRegExtractOrg>
    {
        public RosRegExtractOrgNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
