/// <mapping-converter-backup>
/// using Bars.Gkh.Map;
/// 
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     public class ActCheckInspectedPartMap : BaseGkhEntityMap<ActCheckInspectedPart>
///     {
///         public ActCheckInspectedPartMap() : base("GJI_ACTCHECK_INSPECTPART")
///         {
///             Map(x => x.Character, "CHARACTER").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.InspectedPart, "INSPECTIONPART_ID").Not.Nullable().Fetch.Join();
///         } 
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Инспектируемая часть в акте проверки"</summary>
    public class ActCheckInspectedPartMap : BaseEntityMap<ActCheckInspectedPart>
    {
        
        public ActCheckInspectedPartMap() : 
                base("Инспектируемая часть в акте проверки", "GJI_ACTCHECK_INSPECTPART")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Character, "Характер и местоположение").Column("CHARACTER").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            Reference(x => x.ActCheck, "Акт обследования").Column("ACTCHECK_ID").NotNull().Fetch();
            Reference(x => x.InspectedPart, "Инспектируемая часть").Column("INSPECTIONPART_ID").NotNull().Fetch();

        }
    }
}
