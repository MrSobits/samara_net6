/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.HouseRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.HouseRegister;
///     using Enums;
/// 
///     public class HouseRegisterMap : BaseEntityMap<HouseRegister>
///     {
///         public HouseRegisterMap() : base("GIS_HOUSE_REGISTER")        
///         {            
///             References(x => x.FiasAddress, "FIASADDRESS");
///             References(x => x.Municipality, "MUNICIPALIY_ID");
///             References(x => x.WallMaterial, "WALLMATERIAL");
///             References(x => x.RoofingMaterial, "ROOFINGMATERIAL");
///             References(x => x.TypeProject, "TYPEPROJECT");
/// 
///             Map(x => x.Region, "REGION");
///             Map(x => x.Area, "AREA");
///             Map(x => x.City, "CITY");
///             Map(x => x.Street, "STREET");
///             Map(x => x.HouseNum, "HOUSENUM");
///             Map(x => x.BuildNum, "BUILDNUM");
///             Map(x => x.TotalSquare, "TOTALSQUARE");
///             Map(x => x.BuildDate, "BUILDDATE");
///             Map(x => x.TypeHouse, "HOUSETYPE");
///             Map(x => x.MinimumFloors, "MINIMUMFLOORS");
///             Map(x => x.MaximumFloors, "MAXIMUMFLOORS");
///             Map(x => x.NumberLiving, "NUMBERLIVING");
///             Map(x => x.NumberIndividualCounter, "NUMBERINDIVIDUALCOUNTER");
///             Map(x => x.PrivatizationDate, "PRIVATIZATIONDATE");
///             Map(x => x.NumberLifts, "NUMBERLIFTS");
///             Map(x => x.TypeRoof, "TYPEROOF");
///             Map(x => x.PhysicalWear, "PHYSICALWEAR");
///             Map(x => x.NumberEntrances, "NUMBERENTRANCES");
///             Map(x => x.HeatingSystem, "HEATINGSYSTEM");
///             Map(x => x.NumberAccount, "NUMBERACCOUNT");
///             Map(x => x.ManOrgs, "MANORGS");
///             Map(x => x.Supplier, "SUPPLIER");
///             Map(x => x.SupplierInn, "SUPPLIERINN");
///             Map(x => x.AreaOwned, "AREA_OWNED");
///             Map(x => x.AreaLivingNotLivingMkd, "AREA_LIV_NOT_LIV_MKD");
///             Map(x => x.NumberApartments, "NUMBER_APARTMENTS");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.HouseRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.HouseRegister.HouseRegister"</summary>
    public class HouseRegisterMap : BaseEntityMap<HouseRegister>
    {
        
        public HouseRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.HouseRegister.HouseRegister", "GIS_HOUSE_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.FiasAddress, "FiasAddress").Column("FIASADDRESS");
            Property(x => x.Region, "Region").Column("REGION").Length(250);
            Property(x => x.Area, "Area").Column("AREA").Length(250);
            Property(x => x.City, "City").Column("CITY").Length(250);
            Property(x => x.Street, "Street").Column("STREET").Length(250);
            Property(x => x.HouseNum, "HouseNum").Column("HOUSENUM").Length(250);
            Property(x => x.BuildNum, "BuildNum").Column("BUILDNUM").Length(250);
            Property(x => x.TotalSquare, "TotalSquare").Column("TOTALSQUARE");
            Property(x => x.BuildDate, "BuildDate").Column("BUILDDATE");
            Property(x => x.TypeHouse, "TypeHouse").Column("HOUSETYPE");
            Property(x => x.MinimumFloors, "MinimumFloors").Column("MINIMUMFLOORS");
            Property(x => x.MaximumFloors, "MaximumFloors").Column("MAXIMUMFLOORS");
            Property(x => x.NumberLiving, "NumberLiving").Column("NUMBERLIVING");
            Property(x => x.NumberIndividualCounter, "NumberIndividualCounter").Column("NUMBERINDIVIDUALCOUNTER");
            Property(x => x.PrivatizationDate, "PrivatizationDate").Column("PRIVATIZATIONDATE");
            Property(x => x.NumberLifts, "NumberLifts").Column("NUMBERLIFTS");
            Property(x => x.TypeRoof, "TypeRoof").Column("TYPEROOF");
            Reference(x => x.WallMaterial, "WallMaterial").Column("WALLMATERIAL");
            Property(x => x.PhysicalWear, "PhysicalWear").Column("PHYSICALWEAR");
            Property(x => x.NumberEntrances, "NumberEntrances").Column("NUMBERENTRANCES");
            Reference(x => x.RoofingMaterial, "RoofingMaterial").Column("ROOFINGMATERIAL");
            Reference(x => x.TypeProject, "TypeProject").Column("TYPEPROJECT");
            Property(x => x.HeatingSystem, "HeatingSystem").Column("HEATINGSYSTEM");
            Property(x => x.NumberAccount, "NumberAccount").Column("NUMBERACCOUNT");
            Property(x => x.ManOrgs, "ManOrgs").Column("MANORGS").Length(250);
            Property(x => x.Supplier, "Supplier").Column("SUPPLIER").Length(250);
            Property(x => x.SupplierInn, "SupplierInn").Column("SUPPLIERINN");
            Property(x => x.AreaLivingNotLivingMkd, "AreaLivingNotLivingMkd").Column("AREA_LIV_NOT_LIV_MKD");
            Property(x => x.AreaOwned, "AreaOwned").Column("AREA_OWNED");
            Property(x => x.NumberApartments, "NumberApartments").Column("NUMBER_APARTMENTS");
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALIY_ID");
        }
    }
}
