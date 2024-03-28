namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzor: DocumentGji
    {
        /// <summary>
        /// Документ-основание
        /// </summary>
        /// <remarks>По умолчанию Акт проверки</remarks>
        public virtual string DocumentReason { get; set; }

        /// <summary>
        /// Дата вручения
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Код Гис - униклаьный идентификатор начисления
        /// </summary>
        public virtual string GisUin { get; set; }

        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public virtual string RevocationReason { get; set; }

        /// <summary>
        /// Тип инициативного органа (кем вынесено)
        /// </summary>
        public virtual TypeInitiativeOrgGji TypeInitiativeOrg { get; set; }

        /// <summary>
        /// Муниципальное образование получателя штрафа
        /// </summary>
        public virtual Municipality FineMunicipality { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Inspector Official { get; set; }

        /// <summary>
        /// Местонахождение
        /// </summary>
        public virtual Municipality LocationMunicipality { get; set; }

        /// <summary>
        /// Основание прекращения
        /// </summary>
        public virtual TypeTerminationBasement? ExpireReason { get; set; }

        /// <summary>
        /// Вид санкции
        /// </summary>
        public virtual SanctionGji Sanction { get; set; }

        /// <summary>
        /// Сумма штрафов
        /// </summary>
        public virtual decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// Номер документа (Санкция)
        /// </summary>
        public virtual string SspDocumentNum { get; set; }

        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public virtual YesNoNotSet Paided { get; set; }

        /// <summary>
        /// Дата передачи в ССП
        /// </summary>
        public virtual DateTime? TransferToSspDate { get; set; }

        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}