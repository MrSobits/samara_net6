/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class LegalAccountOwnerMap : BaseJoinedSubclassMap<LegalAccountOwner>
///     {
///         public LegalAccountOwnerMap()
///             : base("REGOP_LEGAL_ACC_OWN", "ID")
///         {
///             Map(x => x.PrintAct, "PRINT_ACT", false, false);
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Абонент - юр.лицо"</summary>
    public class LegalAccountOwnerMap : JoinedSubClassMap<LegalAccountOwner>
    {
        
        public LegalAccountOwnerMap() : 
                base("Абонент - юр.лицо", "REGOP_LEGAL_ACC_OWN")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.PrintAct, "PrintAct").Column("PRINT_ACT").DefaultValue(false);
        }
    }
}
