/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Nso.Map
/// {
///    using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Органместного самоуправления в НСО"
///     /// </summary>
///     public class NsoLocalGovernmentlMap : SubclassMap<NsoLocalGovernment>
///     {
///         public NsoLocalGovernmentlMap()
///         {
///             Table("GKH_NSO_LOCALGOV");
///             KeyColumn("ID");
/// 
///             Map(x => x.Fio, "FIO");
/// 
///             Map(x => x.RegNumNotice, "REG_NUM_NOTICE");
///             Map(x => x.RegDateNotice, "REG_DATE_NOTICE");
/// 
///             Map(x => x.NumNpa, "NUM_NPA");
///             Map(x => x.DateNpa, "DATE_NPA");
///             Map(x => x.NameNpa, "NAME_NPA");
/// 
///             Map(x => x.InteractionMuNum, "INTERACTION_MU_NUM");
///             Map(x => x.InteractionMuDate, "INTERACTION_MU_DATE");
/// 
///             Map(x => x.InteractionGjiNum, "INTERACTION_GJI_NUM");
///             Map(x => x.InteractionGjiDate, "INTERACTION_GJI_DATE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Nso
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Nso;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Nso.NsoLocalGovernment"</summary>
    public class NsoLocalGovernmentMap : JoinedSubClassMap<NsoLocalGovernment>
    {
        
        public NsoLocalGovernmentMap() : 
                base("Bars.Gkh.Regions.Nso.NsoLocalGovernment", "GKH_NSO_LOCALGOV")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Fio, "Fio").Column("FIO");
            Property(x => x.RegNumNotice, "RegNumNotice").Column("REG_NUM_NOTICE");
            Property(x => x.RegDateNotice, "RegDateNotice").Column("REG_DATE_NOTICE");
            Property(x => x.NumNpa, "NumNpa").Column("NUM_NPA");
            Property(x => x.DateNpa, "DateNpa").Column("DATE_NPA");
            Property(x => x.NameNpa, "NameNpa").Column("NAME_NPA");
            Property(x => x.InteractionMuNum, "InteractionMuNum").Column("INTERACTION_MU_NUM");
            Property(x => x.InteractionMuDate, "InteractionMuDate").Column("INTERACTION_MU_DATE");
            Property(x => x.InteractionGjiNum, "InteractionGjiNum").Column("INTERACTION_GJI_NUM");
            Property(x => x.InteractionGjiDate, "InteractionGjiDate").Column("INTERACTION_GJI_DATE");
        }
    }
}
