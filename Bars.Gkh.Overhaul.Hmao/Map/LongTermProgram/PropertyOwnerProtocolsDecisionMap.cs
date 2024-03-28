/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
///     using Bars.Gkh.Overhaul.Hmao.Enum;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протоколы собственников помещений МКД"
///     /// </summary>
/// 
///     public class PropertyOwnerProtocolsMap : BaseImportableEntityMap<PropertyOwnerProtocols>
///     {
///         public PropertyOwnerProtocolsMap() : base("OVRHL_PROP_OWN_PROTOCOLS")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.Description, "DESCRIPTION");
///             Map(x => x.NumberOfVotes, "NUMBER_OF_VOTES");
///             Map(x => x.TotalNumberOfVotes, "TOTAL_NUMBER_OF_VOTES");
///             Map(x => x.PercentOfParticipating, "PERCENT_OF_PARTICIPATE");
///             Map(x => x.TypeProtocol, "TYPE_PROTOCOL").Not.Nullable().CustomType<PropertyOwnerProtocolType>();
///                 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DocumentFile, "DOCUMENT_FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Протоколы собственников помещений МКД"</summary>
    public class PropertyOwnerProtocolsDecisionMap : BaseImportableEntityMap<PropertyOwnerProtocolsDecision>
    {
        
        public PropertyOwnerProtocolsDecisionMap() : 
                base("Протоколы собственников помещений МКД", "OVRHL_PROP_OWN_PROTOCOLS_DEC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").Fetch();
            Reference(x => x.Decision, "Решение").Column("PROT_TYPE_DEC_ID").Fetch();
            Reference(x => x.DocumentFile, "Файл (документ)").Column("DOCUMENT_FILE_ID").Fetch();
        }
    }
}
