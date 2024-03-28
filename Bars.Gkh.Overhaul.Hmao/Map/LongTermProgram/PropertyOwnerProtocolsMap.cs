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
    public class PropertyOwnerProtocolsMap : BaseImportableEntityMap<PropertyOwnerProtocols>
    {
        
        public PropertyOwnerProtocolsMap() : 
                base("Протоколы собственников помещений МКД", "OVRHL_PROP_OWN_PROTOCOLS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNumber, "Номер").Column("DOCUMENT_NUMBER");
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
            Property(x => x.NumberOfVotes, "Количество голосов (кв.м.)").Column("NUMBER_OF_VOTES");
            Property(x => x.TotalNumberOfVotes, "Общее количество голосов (кв.м.)").Column("TOTAL_NUMBER_OF_VOTES");
            Property(x => x.PercentOfParticipating, "Доля принявших участие (%)").Column("PERCENT_OF_PARTICIPATE");
            Property(x => x.TypeProtocol, "тип протокола").Column("TYPE_PROTOCOL").NotNull();
            Reference(x => x.RealityObject, "Объект долгосрочной программы").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.DocumentFile, "Файл (документ)").Column("DOCUMENT_FILE_ID").Fetch();
            Reference(x => x.ProtocolTypeId, "Тип протокола (новый)").Column("PROTOCOL_TYPE_ID").Fetch();
            //новые поля
            Property(x => x.VoteForm, "VoteForm").Column("VOTE_FORM").NotNull();
            Property(x => x.RegistrationDate, "Дата").Column("REGISTRATION_DATE");
            Property(x => x.RegistrationNumber, "Номер").Column("REGISTRATION_NUMBER");
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").Fetch();
            Reference(x => x.ProtocolMKDState, "ProtocolMKDState").Column("PROTOCOL_STATE_ID").Fetch();
            Reference(x => x.ProtocolMKDSource, "ProtocolMKDSource").Column("PROTOCOL_SOURCE_ID").Fetch();
            Reference(x => x.ProtocolMKDIniciator, "ProtocolMKDIniciator").Column("PROTOCOL_INICIATOR_ID").Fetch();
        }
    }
}
