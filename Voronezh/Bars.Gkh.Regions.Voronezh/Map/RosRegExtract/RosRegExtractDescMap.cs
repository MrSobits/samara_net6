namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractDesc"</summary>
    public class RosRegExtractDescMap : PersistentObjectMap<RosRegExtractDesc>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractDescMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractDesc")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Desc_ID_Object, "Desc_ID_Object").Column("Desc_ID_Object");
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
            this.Property(x => x.Room_id, "Помещение").Column("ROOM_ID");
            this.Property(x => x.YesNoNotSet, "Сопоставлено").Column("IS_MERGED");

            this.Property(x => x.ExtractDate, "ExtractDate").Column("ExtractDate");
            this.Property(x => x.ExtractNumber, "ExtractNumber").Column("ExtractNumber");
            this.Property(x => x.HeadContent, "HeadContent").Column("HeadContent");
            this.Property(x => x.Registrator, "Registrator").Column("Registrator");
            this.Property(x => x.Appointment, "Appointment").Column("Appointment");
            this.Property(x => x.NoShareHolding, "NoShareHolding").Column("NoShareHolding");
            this.Property(x => x.RightAgainst, "RightAgainst").Column("RightAgainst");
            this.Property(x => x.RightAssert, "RightAssert").Column("RightAssert");
            this.Property(x => x.RightClaim, "RightClaim").Column("RightClaim");
            this.Property(x => x.RightSteal, "RightSteal").Column("RightSteal");
            //this.Property(x => x.XML, "XML").Column("XML");
        }
    }
    public class RosRegExtractDescNhMaping : ClassMapping<RosRegExtractDesc>
    {
        public RosRegExtractDescNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
