/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Постановление"
///     /// </summary>
///     public class ResolutionMap : SubclassMap<Resolution>
///     {
///         public ResolutionMap()
///         {
///             Table("GJI_RESOLUTION");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(500);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DeliveryDate, "DELIVERY_DATE");
///             Map(x => x.SectorNumber, "SECTOR_NUMBER").Length(250);
///             Map(x => x.PenaltyAmount, "PENALTY_AMOUNT");
///             Map(x => x.TypeInitiativeOrg, "TYPE_INITIATIVE_ORG").Not.Nullable().CustomType<TypeInitiativeOrgGji>();
///             Map(x => x.Paided, "PAIDED").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.DateTransferSsp, "DATE_TRANSFER_SSP");
///             Map(x => x.DocumentNumSsp, "DOCUMENT_NUM_SSP").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.GisUin, "GIS_UIN").Length(50);
///             Map(x => x.DocumentTime, "DOCUMENT_TIME");
///             Map(x => x.BecameLegal, "BECAME_LEGAL");
///             Map(x => x.DateWriteOut, "DATE_WRITE_OUT");
///             Map(x => x.TypeTerminationBasement, "TERMINATION_BASEMENT").CustomType<TypeTerminationBasement>();
///             Map(x => x.AbandonReason, "ABANDON_REASON").Length(512);
/// 
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///             References(x => x.FineMunicipality, "FINEMUNICIPALITY_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///             References(x => x.Sanction, "SANCTION_ID").Fetch.Join();
///             References(x => x.Official, "OFFICIAL_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Постановление"</summary>
    public class ResolutionMap : JoinedSubClassMap<Resolution>
    {

        public ResolutionMap() :
                base("Постановление", "GJI_RESOLUTION")
        {
        }

        protected override void Map()
        {
            Property(x => x.Surname, "Фамилия").Column("SURNAME");
            Property(x => x.FirstName, "Имя").Column("FIRSTNAME");
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC");
            Property(x => x.Position, "Должность").Column("POSITION");
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(500);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DeliveryDate, "Дата вручения").Column("DELIVERY_DATE");
            Property(x => x.SectorNumber, "Номер участка").Column("SECTOR_NUMBER").Length(250);
            Property(x => x.PenaltyAmount, "Сумма штрафов").Column("PENALTY_AMOUNT");
            Property(x => x.TypeInitiativeOrg, "Тип инициативного органа").Column("TYPE_INITIATIVE_ORG").NotNull();
            Property(x => x.Paided, "Штраф оплачен").Column("PAIDED").NotNull();
            Property(x => x.PayStatus, "Штраф оплачен (подробно)").Column("PAY_STATUS").NotNull();
            Property(x => x.DateTransferSsp, "Дата передачи в ССП").Column("DATE_TRANSFER_SSP");
            Property(x => x.DocumentNumSsp, "Номер документа, передача в ССП").Column("DOCUMENT_NUM_SSP").Length(300);
            Property(x => x.GisUin, "Код Гис - униклаьный идентификатор начисления").Column("GIS_UIN").Length(50);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Property(x => x.DocumentTime, "Время составления документа").Column("DOCUMENT_TIME");
            Property(x => x.BecameLegal, "Вступило в законную силу").Column("BECAME_LEGAL");
            Property(x => x.DateWriteOut, "Выписка из ЕГРЮЛ").Column("DATE_WRITE_OUT");
            Property(x => x.TypeTerminationBasement, "Основание прекращения").Column("TERMINATION_BASEMENT");
            Property(x => x.AbandonReason, "Основание аннулирования").Column("ABANDON_REASON").Length(512);
            Property(x => x.RulingNumber, "Номер постановления").Column("RULING_NUMBER");
            Property(x => x.OffenderWas, "Нарушитель явился на рассмотрение").Column("OFFENDER_WAS");
            Property(x => x.RulingDate, "Дата постановления").Column("RULING_DATE");
            Property(x => x.RulinFio, "ФИО в постановлении").Column("RULIN_FIO");
            Reference(x => x.Executant, "тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
            Reference(x => x.FineMunicipality, "МО получателя штрафа").Column("FINEMUNICIPALITY_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.Sanction, "Вид санкции").Column("SANCTION_ID").Fetch();
            Reference(x => x.Official, "Должностное лицо").Column("OFFICIAL_ID").Fetch();

            Property(x => x.PostGUID, "Почтовый идентификатор").Column("POST_UIN").Length(50);
            Reference(x => x.OSP, "Орган ОСП").Column("OSP_ID").Fetch();
            Property(x => x.DateEndExecuteSSP, "Дата окончания исполнительного производства").Column("END_EXECUTE_DATE");
            Property(x => x.DateExecuteSSP, "Дата исполнительного производства").Column("EXECUTESTART_DATE");
            Property(x => x.DateOSPListArrive, "Дата вручения исполнительного документа").Column("EXECUTEDOCARRIVE_DATE");
            Property(x => x.Comment, "Комментарий").Column("COMMENT").Length(500);
            Property(x => x.ExecuteSSPNumber, "Номер исполнительного производства").Column("EXECUTESSP_NUMBER").Length(250);
            Property(x => x.OSPDecisionType, "Решение ОСП").Column("OSP_DECISION");

            //новые поля по решениям судебных участков
            Property(x => x.DecisionDate, "Дата решения").Column("DECISION_DATE");
            Property(x => x.DecisionEntryDate, "Дата вступления решения в законную силу").Column("DECISION_ENTRY_DATE");
            Property(x => x.DecisionNumber, "Номер решения").Column("DECISION_NUMBER");
            Reference(x => x.JudicalOffice, "Судебный участок").Column("JUDICAL_OFFICE_ID").Fetch();
            Property(x => x.Violation, "Нарушение").Column("VIOLATION");

            Property(x => x.InLawDate, "Дата вступления в силу").Column("INLAW_DATE");
            Property(x => x.DueDate, "Срок оплаты").Column("DUE_DATE");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");

            Property(x => x.PhysicalPersonDocumentNumber, "Номер документа").Column("PHYSICALPERSON_DOC_NUM").Length(500);
            Property(x => x.PhysicalPersonIsNotRF, "Гражданство").Column("PHYSICALPERSON_NOT_CITIZENSHIP").DefaultValue(false);
            Property(x => x.PhysicalPersonDocumentSerial, "Серия документа").Column("PHYSICALPERSON_DOC_SERIAL").Length(500);
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();

            Reference(x => x.ConcederationResult, "Результат рассмотрения").Column("CONSIDERATION_RESULT_ID");

            Property(x => x.Payded50Percent, "Нарушение").Column("PAYDED_HALF");
            Property(x => x.WrittenOff, "Нарушение").Column("WRITTEN_OFF");
            Property(x => x.WrittenOffComment, "Нарушение").Column("WRITTEN_OFF_DESCRIPTION");

            Property(x => x.PersonRegistrationAddress, "Нарушение").Column("PERSON_REG_ADDRESS");
            Property(x => x.PersonFactAddress, "Нарушение").Column("PERSON_FACT_ADDRESS");
            Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
            Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
            Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
            Property(x => x.PersonBirthDate, "TypePresence").Column("PERSON_BIRTH_DATE");
            Property(x => x.PersonBirthPlace, "TypePresence").Column("PERSON_BIRTH_PLACE");
        }
    }
}

