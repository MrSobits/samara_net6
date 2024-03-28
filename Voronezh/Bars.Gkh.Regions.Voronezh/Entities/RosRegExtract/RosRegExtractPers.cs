using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractPers : PersistentObject
    {
        //Person
        public virtual string Pers_Code_SP { get; set; }
        public virtual string Pers_Content { get; set; }
        public virtual string Pers_FIO_Surname { get; set; }
        public virtual string Pers_FIO_First { get; set; }
        public virtual string Pers_FIO_Patronymic { get; set; }
        public virtual string Pers_DateBirth { get; set; }
        public virtual string Pers_Place_Birth { get; set; }
        public virtual string Pers_Citizen { get; set; }
        public virtual string Pers_Sex { get; set; }
        public virtual string Pers_DocContent { get; set; }
        public virtual string Pers_DocType_Document { get; set; }
        public virtual string Pers_DocName { get; set; }
        public virtual string Pers_DocSeries { get; set; }
        public virtual string Pers_DocNumber { get; set; }
        public virtual string Pers_DocDate { get; set; }
        public virtual string Pers_SNILS { get; set; }
        //Person->Location
        public virtual string Pers_Loc_ID_Address { get; set; }
        public virtual string Pers_Loc_Content { get; set; }
        public virtual string Pers_Loc_CountryCode { get; set; }
        public virtual string Pers_Loc_CountryName { get; set; }
        public virtual string Pers_Loc_RegionCode { get; set; }
        public virtual string Pers_Loc_RegionName { get; set; }
        public virtual string Pers_Loc_Code_OKATO { get; set; }
        public virtual string Pers_Loc_Code_KLADR { get; set; }
        public virtual string Pers_Loc_DistrictName { get; set; }
        public virtual string Pers_Loc_Urban_DistrictName { get; set; }
        public virtual string Pers_Loc_LocalityName { get; set; }
        public virtual string Pers_Loc_StreetName { get; set; }
        public virtual string Pers_Loc_Level1Name { get; set; }
        public virtual string Pers_Loc_Level2Name { get; set; }
        public virtual string Pers_Loc_Level3Name { get; set; }
        public virtual string Pers_Loc_ApartmentName { get; set; }
        public virtual string Pers_Loc_Other { get; set; }
        //Person->FactLocation
        public virtual string Pers_Floc_ID_Address { get; set; }
        public virtual string Pers_Floc_Content { get; set; }
        public virtual string Pers_Floc_CountryCode { get; set; }
        public virtual string Pers_Floc_CountryName { get; set; }
        public virtual string Pers_Floc_RegionCode { get; set; }
        public virtual string Pers_Floc_RegionName { get; set; }
        public virtual string Pers_Floc_Code_OKATO { get; set; }
        public virtual string Pers_Floc_Code_KLADR { get; set; }
        public virtual string Pers_Floc_DistrictName { get; set; }
        public virtual string Pers_Floc_Urban_DistrictName { get; set; }
        public virtual string Pers_Floc_FlocalityName { get; set; }
        public virtual string Pers_Floc_StreetName { get; set; }
        public virtual string Pers_Floc_Level1Name { get; set; }
        public virtual string Pers_Floc_Level2Name { get; set; }
        public virtual string Pers_Floc_Level3Name { get; set; }
        public virtual string Pers_Floc_ApartmentName { get; set; }
        public virtual string Pers_Floc_Other { get; set; }
    }
}
