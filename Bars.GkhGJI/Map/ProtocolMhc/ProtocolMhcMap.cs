/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол МЖК"
///     /// </summary>
///     public class ProtocolMhcMap : SubclassMap<ProtocolMhc>
///     {
///         public ProtocolMhcMap()
///         {
///             Table("GJI_PROTOCOLMHC");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DateSupply, "DATE_SUPPLY");
/// 
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Протокол органа муниципального жилищного контроля"</summary>
    public class ProtocolMhcMap : JoinedSubClassMap<ProtocolMhc>
    {
        
        public ProtocolMhcMap() : 
                base("Протокол органа муниципального жилищного контроля", "GJI_PROTOCOLMHC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.Municipality, "Муниципальное образование (Орган прокуратуры, вынесший постановление)").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
