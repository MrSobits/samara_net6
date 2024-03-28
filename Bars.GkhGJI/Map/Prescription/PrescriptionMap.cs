/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
///     using NHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Предписание"
///     /// </summary>
///     public class PrescriptionMap : SubclassMap<Prescription>
///     {
///         public PrescriptionMap()
///         {
///             Table("GJI_PRESCRIPTION");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.IsFamiliar, "IS_FAMILIAR").CustomType<PrescriptionFamiliar>();
/// 
///             Map(x => x.Closed, "CLOSED");
///             Map(x => x.CloseReason, "CLOSE_REASON");
///             Map(x => x.CloseNote, "CLOSE_NOTE");
/// 
///             HasMany(x => x.CloseDocs)
///                 .Access.CamelCaseField(Prefix.Underscore)
///                 .AsBag()
///                 .KeyColumn("PRESCR_ID")
///                 .Inverse()
///                 .Cascade.AllDeleteOrphan();
/// 
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Properties;

    /// <summary>Маппинг для "Предписание"</summary>
    public class PrescriptionMap : JoinedSubClassMap<Prescription>
    {
        
        public PrescriptionMap() : 
                base("Предписание", "GJI_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Property(x => x.RenewalApplicationDate, "Дата решения о продлении").Column("RENEWAL_DATE");
            Property(x => x.RenewalApplicationNumber, "Номер решения о проблении").Column("RENEWAL_NUMBER").Length(50);
            Property(x => x.IsFamiliar, "Сведения об ознакомлении (для НСО)").Column("IS_FAMILIAR");
            Property(x => x.Closed, "Закрыто").Column("CLOSED");
            Property(x => x.CloseReason, "Причина закрыития").Column("CLOSE_REASON");
            Property(x => x.TypePrescriptionExecution, "TypePrescriptionExecution").Column("TYPE_EXECUTION");
            Property(x => x.CancelledGJI, "CancelledGJI").Column("CANCELLED_BY_GJI");
            Property(x => x.PrescriptionState, "PrescriptionState").Column("PRESCRIPTION_STATE");
            Property(x => x.CloseNote, "Примечание при закрытии").Column("CLOSE_NOTE");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
        }
    }
}
