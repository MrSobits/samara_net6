using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractOrg : PersistentObject
    {        
        //Organization
        public virtual string Org_Code_SP { get; set; }
        public virtual string Org_Content { get; set; }
        public virtual string Org_Code_OPF { get; set; }
        public virtual string Org_Name { get; set; }
        public virtual string Org_Inn { get; set; }
        public virtual string Org_Code_OGRN { get; set; }
        public virtual string Org_RegDate { get; set; }
        public virtual string Org_AgencyRegistration { get; set; }
        public virtual string Org_Code_CPP { get; set; }
        //Organization->Location
        public virtual string Org_Loc_ID_Address { get; set; }
        public virtual string Org_Loc_Content { get; set; }
        public virtual string Org_Loc_RegionCode { get; set; }
        public virtual string Org_Loc_RegionName { get; set; }
        public virtual string Org_Loc_Code_OKATO { get; set; }
        public virtual string Org_Loc_Code_KLADR { get; set; }
        public virtual string Org_Loc_CityName { get; set; }
        public virtual string Org_Loc_StreetName { get; set; }
        public virtual string Org_Loc_Level1Name { get; set; }
        //Organization->FactLocation
        public virtual string Org_FLoc_ID_Address { get; set; }
        public virtual string Org_FLoc_Content { get; set; }
        public virtual string Org_FLoc_RegionCode { get; set; }
        public virtual string Org_FLoc_RegionName { get; set; }
        public virtual string Org_FLoc_Code_OKATO { get; set; }
        public virtual string Org_FLoc_Code_KLADR { get; set; }
        public virtual string Org_FLoc_CityName { get; set; }
        public virtual string Org_FLoc_StreetName { get; set; }
        public virtual string Org_FLoc_Level1Name { get; set; }
    }
}
