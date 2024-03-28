/// <mapping-converter-backup>
/// using Bars.B4.Utils;
/// 
/// namespace Bars.GkhGji.Regions.Smolensk.Map
/// {
///     using Entities;
///     using Enums;
///     using FluentNHibernate.Mapping;
/// 
///     public class PrescriptionCancelSmolMap : SubclassMap<PrescriptionCancelSmol>
///     {
///         public PrescriptionCancelSmolMap()
///         {
///             Table("GJI_PRESCR_CANCEL_SMOL");
///             KeyColumn("ID");
///             Map(x => x.SmolCancelResult, "CANCEL_RESULT").Length(2000);
///             Map(x => x.SmolDescriptionSet, "DESCRIPTION_SET").Length(2000);
///             Map(x => x.SmolPetitionDate, "PETITION_DATE");
///             Map(x => x.SmolPetitionNum, "PETITION_NUM").Length(1000);
///             Map(x => x.SmolTypeCancel, "TYPE_CANCEL").Not.Nullable().CustomType<TypePrescriptionCancel>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.PrescriptionCancelSmol"</summary>
    public class PrescriptionCancelSmolMap : JoinedSubClassMap<PrescriptionCancelSmol>
    {
        
        public PrescriptionCancelSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.PrescriptionCancelSmol", "GJI_PRESCR_CANCEL_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SmolCancelResult, "SmolCancelResult").Column("CANCEL_RESULT").Length(2000);
            Property(x => x.SmolDescriptionSet, "SmolDescriptionSet").Column("DESCRIPTION_SET").Length(2000);
            Property(x => x.SmolPetitionDate, "SmolPetitionDate").Column("PETITION_DATE");
            Property(x => x.SmolPetitionNum, "SmolPetitionNum").Column("PETITION_NUM").Length(1000);
            Property(x => x.SmolTypeCancel, "SmolTypeCancel").Column("TYPE_CANCEL").NotNull();
        }
    }
}
