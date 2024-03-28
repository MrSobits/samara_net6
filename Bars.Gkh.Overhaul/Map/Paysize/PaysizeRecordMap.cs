/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PaysizeRecordMap : BaseImportableEntityMap<PaysizeRecord>
///     {
///         public PaysizeRecordMap() : base("OVRHL_PAYSIZE_REC")
///         {
///             Map(x => x.Value, "DVALUE");
/// 
///             References(x => x.Paysize, "PAYSIZE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Муниципальное образование размера взноса на кр"</summary>
    public class PaysizeRecordMap : BaseImportableEntityMap<PaysizeRecord>
    {
        
        public PaysizeRecordMap() : 
                base("Муниципальное образование размера взноса на кр", "OVRHL_PAYSIZE_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Paysize, "Размер взноса на кр").Column("PAYSIZE_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").NotNull().Fetch();
            Property(x => x.Value, "Значение").Column("DVALUE");
        }
    }
}
