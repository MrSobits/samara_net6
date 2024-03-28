/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Nso.Entities;
///     using Bars.Gkh.Overhaul.Nso.Enum;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протоколы собственников помещений МКД"
///     /// </summary>
///     public class PropertyOwnerProtocolsMap : BaseEntityMap<PropertyOwnerProtocols>
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

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.PropertyOwnerProtocols"</summary>
    public class PropertyOwnerProtocolsMap : BaseEntityMap<PropertyOwnerProtocols>
    {
        
        public PropertyOwnerProtocolsMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.PropertyOwnerProtocols", "OVRHL_PROP_OWN_PROTOCOLS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER");
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.NumberOfVotes, "NumberOfVotes").Column("NUMBER_OF_VOTES");
            Property(x => x.TotalNumberOfVotes, "TotalNumberOfVotes").Column("TOTAL_NUMBER_OF_VOTES");
            Property(x => x.PercentOfParticipating, "PercentOfParticipating").Column("PERCENT_OF_PARTICIPATE");
            Property(x => x.TypeProtocol, "TypeProtocol").Column("TYPE_PROTOCOL").NotNull();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.DocumentFile, "DocumentFile").Column("DOCUMENT_FILE_ID").Fetch();
        }
    }
}
