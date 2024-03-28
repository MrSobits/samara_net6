/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Эксперты ГЖИ"
///     /// </summary>
///     public class DisposalExpertMap : BaseGkhEntityMap<DisposalExpert>
///     {
///         public DisposalExpertMap()
///             : base("GJI_DISPOSAL_EXPERT")
///         {
///             References(x => x.Disposal, "DISPOSAL_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Expert, "EXPERT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Эксперт рапоряжения ГЖИ"</summary>
    public class DisposalExpertMap : BaseEntityMap<DisposalExpert>
    {
        
        public DisposalExpertMap() : 
                base("Эксперт рапоряжения ГЖИ", "GJI_DISPOSAL_EXPERT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);;
            Reference(x => x.Disposal, "Распоряжение").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.Expert, "Эксперт").Column("EXPERT_ID").NotNull().Fetch();
        }
    }
}
