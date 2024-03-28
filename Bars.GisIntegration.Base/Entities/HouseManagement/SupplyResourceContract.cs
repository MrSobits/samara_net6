namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Договор с поставщиком ресурсов
    /// </summary>
    public class SupplyResourceContract : BaseRisEntity
    {
        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? ComptetionDate { get; set; }

        /// <summary>
        /// Дата начала (день)
        /// </summary>
        public virtual byte? StartDate { get; set; }

        /// <summary>
        /// Дата начала - Следующего месяца
        /// </summary>
        public virtual bool? StartDateNextMonth { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual byte? EndDate { get; set; }

        /// <summary>
        /// Дата окончания - Следующего месяца
        /// </summary>
        public virtual bool? EndDateNextMonth { get; set; }

        /// <summary>
        /// Код НСИ "Основание заключения договора" (реестровый номер 58)
        /// </summary>
        public virtual string ContractBaseCode { get; set; }

        /// <summary>
        /// Идентификатор НСИ "Основание заключения договора" (реестровый номер 58)
        /// </summary>
        public virtual string ContractBaseGuid { get; set; }

        /// <summary>
        /// Вид договора
        /// </summary>
        public virtual SupplyResourceContractType? ContractType { get; set; }

        /// <summary>
        /// Лицо, являющееся стороной договора
        /// </summary>
        public virtual SupplyResourceContactPersonType? PersonType { get; set; }

        /// <summary>
        /// Тип лица/ организации
        /// </summary>
        public virtual SupplyResourceContactPersonTypeOrganization? PersonTypeOrganization { get; set; }

        /// <summary>
        /// Сторона договора - Юридическое лицо
        /// </summary>
        public virtual RisContragent JurPerson { get; set; }

        /// <summary>
        /// Физическое лицо - Фамилия 
        /// </summary>
        public virtual string IndSurname { get; set; }

        /// <summary>
        /// Физическое лицо - Имя 
        /// </summary>
        public virtual string IndFirstName { get; set; }

        /// <summary>
        /// Физическое лицо - Отчество 
        /// </summary>
        public virtual string IndPatronymic { get; set; }

        /// <summary>
        /// Физическое лицо - Пол 
        /// </summary>
        public virtual RisGender? IndSex { get; set; }

        /// <summary>
        /// Физическое лицо - Дата рождения 
        /// </summary>
        public virtual DateTime? IndDateOfBirth { get; set; }

        /// <summary>
        /// Физическое лицо - СНИЛС 
        /// </summary>
        public virtual string IndSnils { get; set; }

        /// <summary>
        /// Физическое лицо - Код "Документ, удостоверяющий личность" (НСИ 95)
        /// </summary>
        public virtual string IndIdentityTypeCode { get; set; }

        /// <summary>
        /// Физическое лицо - Идентификатор "Документ, удостоверяющий личность" (НСИ 95)
        /// </summary>
        public virtual string IndIdentityTypeGuid { get; set; }

        /// <summary>
        /// Физическое лицо - Серия документа 
        /// </summary>
        public virtual string IndIdentitySeries { get; set; }

        /// <summary>
        /// Физическое лицо - Номер документа
        /// </summary>
        public virtual string IndIdentityNumber { get; set; }

        /// <summary>
        /// Физическое лицо - Дата выдачи документа
        /// </summary>
        public virtual DateTime? IndIdentityIssueDate { get; set; }

        /// <summary>
        /// Физическое лицо - Место рождения 
        /// </summary>
        public virtual string IndPlaceBirth { get; set; }

        /// <summary>
        /// Коммерческий учет ресурса осуществляет
        /// </summary>
        public virtual SupResCommercialMeteringResourceType? CommercialMeteringResourceType { get; set; }

        /// <summary>
        /// Адрес дома Глобальный уникальный идентификатор дома по ФИАС
        /// </summary>
        public virtual string FiasHouseGuid { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        public virtual DateTime? SigningDate { get; set; }

        /// <summary>
        /// Дата вступления договора в силу
        /// </summary>
        public virtual DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Срок выставления счетов к оплате, не позднее
        /// </summary>
        public virtual byte? BillingDate { get; set; }

        /// <summary>
        /// Срок оплаты, не позднее
        /// </summary>
        public virtual byte? PaymentDate { get; set; }

        /// <summary>
        /// Срок предоставления информации о поступивших платежах и о задолженностях
        /// </summary>
        public virtual byte? ProvidingInformationDate { get; set; }

        /// <summary>
        /// Код НСИ "54 Причина расторжения договора" 
        /// </summary>
        public virtual string TerminateReasonCode { get; set; }

        /// <summary>
        /// Идентификатор НСИ "54 Причина расторжения договора" 
        /// </summary>
        public virtual string TerminateReasonGuid { get; set; }

        /// <summary>
        /// Дата расторжения, прекращения действия устава
        /// </summary>
        public virtual DateTime? TerminateDate { get; set; }

        /// <summary>
        /// Дата окончания пролонгации
        /// </summary>
        public virtual DateTime? RollOverDate { get; set; }
    }
}
