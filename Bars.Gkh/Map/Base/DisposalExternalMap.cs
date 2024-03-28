/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// мап для DisposalExternal
///     /// </summary>
///     public class DisposalExternalMap : BaseGkhEntityMap<DisposalExternal>
///     {
///         public DisposalExternalMap()
///             : base("CONVERTER_DISP_EXTERNAL")
///         {
///             Map(x => x.NewExternalId, "NEW_EXTERNAL_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Таблица для конвертации связь старого родного Распоряжения и схлопнутого"</summary>
    public class DisposalExternalMap : BaseImportableEntityMap<DisposalExternal>
    {
        
        public DisposalExternalMap() : 
                base("Таблица для конвертации связь старого родного Распоряжения и схлопнутого", "CONVERTER_DISP_EXTERNAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.NewExternalId, "новый идентификатор распоряжения").Column("NEW_EXTERNAL_ID");
        }
    }
}
