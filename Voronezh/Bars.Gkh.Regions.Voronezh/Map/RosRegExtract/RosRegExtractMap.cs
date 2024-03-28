namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtract"</summary>
    public class RosRegExtractMap : PersistentObjectMap<RosRegExtract>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtract")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.desc_id, "desc_id").Column("desc_id").Fetch();
            this.Reference(x => x.right_id, "right_id").Column("right_id").Fetch();
            /*this.Property(x => x.Desc_ID_Object, "Desc_ID_Object").Column("Desc_ID_Object");
            this.Property(x => x.Desc_CadastralNumber, "Desc_CadastralNumber").Column("Desc_CadastralNumber");
            this.Property(x => x.Desc_ObjectType, "Desc_ObjectType").Column("Desc_ObjectType");
            this.Property(x => x.Desc_ObjectTypeText, "Desc_ObjectTypeText").Column("Desc_ObjectTypeText");
            this.Property(x => x.Desc_ObjectTypeName, "Desc_ObjectTypeName").Column("Desc_ObjectTypeName");
            this.Property(x => x.Desc_AssignationCode, "Desc_AssignationCode").Column("Desc_AssignationCode");
            this.Property(x => x.Desc_AssignationCodeText, "Desc_AssignationCodeText").Column("Desc_AssignationCodeText");
            this.Property(x => x.Desc_Area, "Desc_Area").Column("Desc_Area");
            this.Property(x => x.Desc_AreaText, "Desc_AreaText").Column("Desc_AreaText");
            this.Property(x => x.Desc_AreaUnit, "Desc_AreaUnit").Column("Desc_AreaUnit");
            this.Property(x => x.Desc_Floor, "Desc_Floor").Column("Desc_Floor");
            this.Property(x => x.Desc_ID_Address, "Desc_ID_Address").Column("Desc_ID_Address");
            this.Property(x => x.Desc_AddressContent, "Desc_AddressContent").Column("Desc_AddressContent");
            this.Property(x => x.Desc_RegionCode, "Desc_RegionCode").Column("Desc_RegionCode");
            this.Property(x => x.Desc_RegionName, "Desc_RegionName").Column("Desc_RegionName");
            this.Property(x => x.Desc_OKATO, "Desc_OKATO").Column("Desc_OKATO");
            this.Property(x => x.Desc_KLADR, "Desc_KLADR").Column("Desc_KLADR");
            this.Property(x => x.Desc_CityName, "Desc_CityName").Column("Desc_CityName");
            this.Property(x => x.Desc_Urban_District, "Desc_Urban_District").Column("Desc_Urban_District");
            this.Property(x => x.Desc_Locality, "Desc_Locality").Column("Desc_Locality");
            this.Property(x => x.Desc_StreetName, "Desc_StreetName").Column("Desc_StreetName");
            this.Property(x => x.Desc_Level1Name, "Desc_Level1Name").Column("Desc_Level1Name");
            this.Property(x => x.Desc_Level2Name, "Desc_Level2Name").Column("Desc_Level2Name");
            this.Property(x => x.Desc_ApartmentName, "Desc_ApartmentName").Column("Desc_ApartmentName");
            this.Property(x => x.Pers_Code_SP, "Pers_Code_SP").Column("Pers_Code_SP");
            this.Property(x => x.Pers_Content, "Pers_Content").Column("Pers_Content");
            this.Property(x => x.Pers_FIO_Surname, "Pers_FIO_Surname").Column("Pers_FIO_Surname");
            this.Property(x => x.Pers_FIO_First, "Pers_FIO_First").Column("Pers_FIO_First");
            this.Property(x => x.Pers_FIO_Patronymic, "Pers_FIO_Patronymic").Column("Pers_FIO_Patronymic");
            this.Property(x => x.Pers_DateBirth, "Pers_DateBirth").Column("Pers_DateBirth");
            this.Property(x => x.Pers_Place_Birth, "Pers_Place_Birth").Column("Pers_Place_Birth");
            this.Property(x => x.Pers_Citizen, "Pers_Citizen").Column("Pers_Citizen");
            this.Property(x => x.Pers_Sex, "Pers_Sex").Column("Pers_Sex");
            this.Property(x => x.Pers_DocContent, "Pers_DocContent").Column("Pers_DocContent");
            this.Property(x => x.Pers_DocType_Document, "Pers_DocType_Document").Column("Pers_DocType_Document");
            this.Property(x => x.Pers_DocName, "Pers_DocName").Column("Pers_DocName");
            this.Property(x => x.Pers_DocSeries, "Pers_DocSeries").Column("Pers_DocSeries");
            this.Property(x => x.Pers_DocNumber, "Pers_DocNumber").Column("Pers_DocNumber");
            this.Property(x => x.Pers_DocDate, "Pers_DocDate").Column("Pers_DocDate");
            this.Property(x => x.Pers_Loc_ID_Address, "Pers_Loc_ID_Address").Column("Pers_Loc_ID_Address");
            this.Property(x => x.Pers_Loc_Content, "Pers_Loc_Content").Column("Pers_Loc_Content");
            this.Property(x => x.Pers_Loc_CountryCode, "Pers_Loc_CountryCode").Column("Pers_Loc_CountryCode");
            this.Property(x => x.Pers_Loc_CountryName, "Pers_Loc_CountryName").Column("Pers_Loc_CountryName");
            this.Property(x => x.Pers_Loc_RegionCode, "Pers_Loc_RegionCode").Column("Pers_Loc_RegionCode");
            this.Property(x => x.Pers_Loc_RegionName, "Pers_Loc_RegionName").Column("Pers_Loc_RegionName");
            this.Property(x => x.Pers_Loc_Code_OKATO, "Pers_Loc_Code_OKATO").Column("Pers_Loc_Code_OKATO");
            this.Property(x => x.Pers_Loc_Code_KLADR, "Pers_Loc_Code_KLADR").Column("Pers_Loc_Code_KLADR");
            this.Property(x => x.Pers_Loc_DistrictName, "Pers_Loc_DistrictName").Column("Pers_Loc_DistrictName");
            this.Property(x => x.Pers_Loc_Urban_DistrictName, "Pers_Loc_Urban_DistrictName").Column("Pers_Loc_Urban_DistrictName");
            this.Property(x => x.Pers_Loc_LocalityName, "Pers_Loc_LocalityName").Column("Pers_Loc_LocalityName");
            this.Property(x => x.Pers_Loc_StreetName, "Pers_Loc_StreetName").Column("Pers_Loc_StreetName");
            this.Property(x => x.Pers_Loc_Level1Name, "Pers_Loc_Level1Name").Column("Pers_Loc_Level1Name");
            this.Property(x => x.Pers_Loc_Level2Name, "Pers_Loc_Level2Name").Column("Pers_Loc_Level2Name");
            this.Property(x => x.Pers_Loc_Level3Name, "Pers_Loc_Level3Name").Column("Pers_Loc_Level3Name");
            this.Property(x => x.Pers_Loc_ApartmentName, "Pers_Loc_ApartmentName").Column("Pers_Loc_ApartmentName");
            this.Property(x => x.Pers_Loc_Other, "Pers_Loc_Other").Column("Pers_Loc_Other");
            this.Property(x => x.Pers_Floc_ID_Address, "Pers_Floc_ID_Address").Column("Pers_Floc_ID_Address");
            this.Property(x => x.Pers_Floc_Content, "Pers_Floc_Content").Column("Pers_Floc_Content");
            this.Property(x => x.Pers_Floc_CountryCode, "Pers_Floc_CountryCode").Column("Pers_Floc_CountryCode");
            this.Property(x => x.Pers_Floc_CountryName, "Pers_Floc_CountryName").Column("Pers_Floc_CountryName");
            this.Property(x => x.Pers_Floc_RegionCode, "Pers_Floc_RegionCode").Column("Pers_Floc_RegionCode");
            this.Property(x => x.Pers_Floc_RegionName, "Pers_Floc_RegionName").Column("Pers_Floc_RegionName");
            this.Property(x => x.Pers_Floc_Code_OKATO, "Pers_Floc_Code_OKATO").Column("Pers_Floc_Code_OKATO");
            this.Property(x => x.Pers_Floc_Code_KLADR, "Pers_Floc_Code_KLADR").Column("Pers_Floc_Code_KLADR");
            this.Property(x => x.Pers_Floc_DistrictName, "Pers_Floc_DistrictName").Column("Pers_Floc_DistrictName");
            this.Property(x => x.Pers_Floc_Urban_DistrictName, "Pers_Floc_Urban_DistrictName").Column("Pers_Floc_Urban_DistrictName");
            this.Property(x => x.Pers_Floc_FlocalityName, "Pers_Floc_FlocalityName").Column("Pers_Floc_FlocalityName");
            this.Property(x => x.Pers_Floc_StreetName, "Pers_Floc_StreetName").Column("Pers_Floc_StreetName");
            this.Property(x => x.Pers_Floc_Level1Name, "Pers_Floc_Level1Name").Column("Pers_Floc_Level1Name");
            this.Property(x => x.Pers_Floc_Level2Name, "Pers_Floc_Level2Name").Column("Pers_Floc_Level2Name");
            this.Property(x => x.Pers_Floc_Level3Name, "Pers_Floc_Level3Name").Column("Pers_Floc_Level3Name");
            this.Property(x => x.Pers_Floc_ApartmentName, "Pers_Floc_ApartmentName").Column("Pers_Floc_ApartmentName");
            this.Property(x => x.Pers_Floc_Other, "Pers_Floc_Other").Column("Pers_Floc_Other");
            this.Property(x => x.Gov_Code_SP, "Gov_Code_SP").Column("Gov_Code_SP");
            this.Property(x => x.Gov_Content, "Gov_Content").Column("Gov_Content");
            this.Property(x => x.Gov_Name, "Gov_Name").Column("Gov_Name");
            this.Property(x => x.Gov_OKATO_Code, "Gov_OKATO_Code").Column("Gov_OKATO_Code");
            this.Property(x => x.Gov_Country, "Gov_Country").Column("Gov_Country");
            this.Property(x => x.Gov_Address, "Gov_Address").Column("Gov_Address");
            this.Property(x => x.Org_Code_SP, "Org_Code_SP").Column("Org_Code_SP");
            this.Property(x => x.Org_Content, "Org_Content").Column("Org_Content");
            this.Property(x => x.Org_Code_OPF, "Org_Code_OPF").Column("Org_Code_OPF");
            this.Property(x => x.Org_Name, "Org_Name").Column("Org_Name");
            this.Property(x => x.Org_Inn, "Org_Inn").Column("Org_Inn");
            this.Property(x => x.Org_Code_OGRN, "Org_Code_OGRN").Column("Org_Code_OGRN");
            this.Property(x => x.Org_RegDate, "Org_RegDate").Column("Org_RegDate");
            this.Property(x => x.Org_AgencyRegistration, "Org_AgencyRegistration").Column("Org_AgencyRegistration");
            this.Property(x => x.Org_Code_CPP, "Org_Code_CPP").Column("Org_Code_CPP");
            this.Property(x => x.Org_AreaUnit, "Org_AreaUnit").Column("Org_AreaUnit");
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
            this.Property(x => x.Reg_ID_Record, "Reg_ID_Record").Column("Reg_ID_Record");
            this.Property(x => x.Reg_RegNumber, "Reg_RegNumber").Column("Reg_RegNumber");
            this.Property(x => x.Reg_Type, "Reg_Type").Column("Reg_Type");
            this.Property(x => x.Reg_Name, "Reg_Name").Column("Reg_Name");
            this.Property(x => x.Reg_RegDate, "Reg_RegDate").Column("Reg_RegDate");
            this.Property(x => x.Reg_ShareText, "Reg_ShareText").Column("Reg_ShareText");*/
        }
    }
    public class RosRegExtractNhMaping : ClassMapping<RosRegExtract>
    {
        public RosRegExtractNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
