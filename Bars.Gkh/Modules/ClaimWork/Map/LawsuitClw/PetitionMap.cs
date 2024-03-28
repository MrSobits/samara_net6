/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map.LawsuitClw
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class PetitionMap : BaseJoinedSubclassMap<Petition>
///     {
///         public PetitionMap() : base("clw_petition", "id")
///         {
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Исковое заявление"</summary>
    public class PetitionMap : JoinedSubClassMap<Petition>
    {
        
        public PetitionMap() : 
                base("Исковое заявление", "CLW_PETITION")
        {
        }
        
        protected override void Map()
        {
        }
    }
}
