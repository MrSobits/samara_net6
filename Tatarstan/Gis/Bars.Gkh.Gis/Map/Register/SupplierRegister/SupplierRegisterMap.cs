/// <mapping-converter-backup>
/// namespace Bars.Gkh.Gis.Map.Register.SupplierRegister
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Register.SupplierRegister;
/// 
///     public class SupplierRegisterMap : BaseEntityMap<SupplierRegister>
///     {
///         public SupplierRegisterMap()
///             : base("GIS_SUPPLIER_REGISTER")        
///         {
///             Map(x => x.Name, "NAME");
///             Map(x => x.Inn, "INN");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gis.Map.Register.SupplierRegister
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gis.Entities.Register.SupplierRegister;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gis.Entities.Register.SupplierRegister.SupplierRegister"</summary>
    public class SupplierRegisterMap : BaseEntityMap<SupplierRegister>
    {
        
        public SupplierRegisterMap() : 
                base("Bars.Gkh.Gis.Entities.Register.SupplierRegister.SupplierRegister", "GIS_SUPPLIER_REGISTER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(250);
            Property(x => x.Inn, "Inn").Column("INN");
        }
    }
}
