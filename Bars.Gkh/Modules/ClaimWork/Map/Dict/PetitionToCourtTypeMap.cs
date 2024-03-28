/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class PetitionToCourtTypeMap : BaseEntityMap<PetitionToCourtType>
///     {
///         public PetitionToCourtTypeMap()
///             : base("CLW_DICT_PETITION_TYPE")
///         {
///             Map(x => x.Code, "CODE", true, 10);
///             Map(x => x.ShortName, "SHORT_NAME", false, 250);
///             Map(x => x.FullName, "FULL_NAME", false, 500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Справочник заявлений в суд"</summary>
    public class PetitionToCourtTypeMap : BaseEntityMap<PetitionToCourtType>
    {
        
        public PetitionToCourtTypeMap() : 
                base("Справочник заявлений в суд", "CLW_DICT_PETITION_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(10);
            Property(x => x.ShortName, "Короткое наименование").Column("SHORT_NAME").Length(250);
            Property(x => x.FullName, "Полное наименование").Column("FULL_NAME").Length(500);
        }
    }
}
