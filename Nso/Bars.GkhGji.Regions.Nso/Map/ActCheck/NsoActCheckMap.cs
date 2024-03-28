/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using Bars.GkhGji.Regions.Nso.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт проверки"
///     /// </summary>
///     public class NsoActCheckMap : SubclassMap<NsoActCheck>
///     {
///         public NsoActCheckMap()
///         {
///             Table("GJI_NSO_ACTCHECK");
///             KeyColumn("ID");
///             Map(x => x.AcquaintedWithDisposalCopy, "ACQUAINT_WITH_DISP");
///             Map(x => x.DocumentPlace, "DOCUMENT_PLACE").Length(1000);
/// 			Map(x => x.DocumentTime, "DOCUMENT_TIME");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.NsoActCheck"</summary>
    public class NsoActCheckMap : JoinedSubClassMap<NsoActCheck>
    {
        
        public NsoActCheckMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.NsoActCheck", "GJI_NSO_ACTCHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AcquaintedWithDisposalCopy, "AcquaintedWithDisposalCopy").Column("ACQUAINT_WITH_DISP");
        }
    }
}
