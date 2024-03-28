namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractPers"</summary>
    public class RosRegExtractPersMap : PersistentObjectMap<RosRegExtractPers>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractPersMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractPers")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
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
            this.Property(x => x.Pers_SNILS, "Pers_SNILS").Column("Pers_SNILS");
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
        }
    }
    public class RosRegExtractPersNhMaping : ClassMapping<RosRegExtractPers>
    {
        public RosRegExtractPersNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
