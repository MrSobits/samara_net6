/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Administration.Instructions
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities;
/// 
///     public class InstructionsMap : BaseEntityMap<Instruction>
///     {
///         public InstructionsMap()
///             : base("GKH_INSTRUCTIONS")
///         {
///             Map(x => x.DisplayName, "DISPLAY_NAME", true);
///             Map(x => x.Code, "CODE", false, 300);
///             References(x => x.File, "FILE_INFO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Инструкция"</summary>
    public class InstructionMap : BaseEntityMap<Instruction>
    {
        
        public InstructionMap() : 
                base("Инструкция", "GKH_INSTRUCTIONS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DisplayName, "Отображаемое имя").Column("DISPLAY_NAME").Length(250).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Reference(x => x.File, "Файл инструкции").Column("FILE_INFO_ID").NotNull().Fetch();
            Reference(x => x.InstructionGroup, "Категория документации").Column("GROUP_ID").Fetch();
        }
    }
}
