/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол МВД"
///     /// </summary>
///     public class ProtocolMvdMap : SubclassMap<ProtocolMvd>
///     {
///         public ProtocolMvdMap()
///         {
///             Table("GJI_PROTOCOL_MVD");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.TypeExecutant, "TYPE_EXECUTANT").Not.Nullable().CustomType<TypeExecutantProtocolMvd>();
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DateSupply, "DATE_SUPPLY");
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Протокол МВД"</summary>
    public class ProtocolMvdMap : JoinedSubClassMap<ProtocolMvd>
    {

        public ProtocolMvdMap() :
                base("Протокол МВД", "GJI_PROTOCOL_MVD")
        {
        }

        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.TypeExecutant, "Тип исполнителя документа").Column("TYPE_EXECUTANT").NotNull();
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");

            Property(x => x.DateOffense, "Дата парвонарушения").Column("DATE_OFFENSE");
            Property(x => x.SerialAndNumber, "Серия и номер паспорта").Column("SERIAL_AND_NUMBER");
            Property(x => x.BirthDate, "Дата рождения").Column("BIRTH_DATE");
            Property(x => x.IssueDate, "Дата выдачи").Column("ISSUE_DATE");
            Property(x => x.BirthPlace, "Место рождения").Column("BIRTH_PLACE");
            Property(x => x.IssuingAuthority, "Кем выдан").Column("ISSUING_AUTHORITY");
            Property(x => x.Company, "Место работы, должность").Column("COMPANY");
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
            Reference(x => x.OrganMvd, "Орган МВД, оформивший протокол").Column("ORGAN_MVD_ID").Fetch();
        }
    }
}
