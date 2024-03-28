namespace Bars.Gkh.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Управляющая организация
    /// </summary>
    public class ManagingOrganization : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual OrgStateRole OrgStateRole { get; set; }

        /// <summary>
        /// Наименование контрагента (не хранимое)
        /// </summary>
        [Obsolete("Не использовать в запросах")]
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Наименование контрагента (не хранимое)
        /// </summary>
        [Obsolete("Не использовать в запросах")]
        public virtual string ContragentShortName { get; set; }

        /// <summary>
        /// Количество МО
        /// </summary>
        public virtual int? CountMo { get; set; }

        /// <summary>
        /// Количество офисов
        /// </summary>
        public virtual int? CountOffices { get; set; }

        /// <summary>
        /// Количество СРФ
        /// </summary>
        public virtual int? CountSrf { get; set; }

        /// <summary>
        /// Передано управление (Только для ТСЖ)
        /// </summary>
        public virtual bool IsTransferredManagementTsj { get; set; }

        /// <summary>
        /// Участвует в рейтинге УК
        /// </summary>
        public virtual YesNoNotSet MemberRanking { get; set; }

        /// <summary>
        /// Общая численность сотрудников
        /// </summary>
        public virtual int? NumberEmployees { get; set; }

        /// <summary>
        /// Официальный сайт
        /// </summary>
        public virtual string OfficialSite { get; set; }

        /// <summary>
        /// Официальный сайт для раскрытия информации по 731
        /// </summary>
        public virtual bool OfficialSite731 { get; set; }

        /// <summary>
        /// Доля участия МО (%)
        /// </summary>
        public virtual decimal? ShareMo { get; set; }

        /// <summary>
        /// Доля участия СФ (%)
        /// </summary>
        public virtual decimal? ShareSf { get; set; }

        /// <summary>
        /// Тип управления 
        /// </summary>
        public virtual TypeManagementManOrg TypeManagement { get; set; }

        //Деятельность
        /// <summary>
        /// Дата окончания деятельности
        /// </summary>
        public virtual DateTime? ActivityDateEnd { get; set; }

        /// <summary>
        /// Описание для деятельности
        /// </summary>
        public virtual string ActivityDescription { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

        #region NSO

        /// <summary>
        /// Председатель ТСЖ
        /// </summary>
        public virtual ContragentContact TsjHead { get; set; }

        /// <summary>
        /// № дела
        /// </summary>
        public virtual string CaseNumber { get; set; }

        #endregion NSO

        #region 988
        /// <summary>
        /// Адрес диспетчерской службы соответствует фактическому адресу
        /// </summary>
        public virtual bool IsDispatchCrrespondedFact { get; set; }

        /// <summary>
        /// Контактные номера телефонов
        /// </summary>
        public virtual string DispatchPhone { get; set; }

        /// <summary>
        /// Адрес диспетчерской службы
        /// </summary>
        public virtual FiasAddress DispatchAddress { get; set; }

        /// <summary>
        /// Устав товарищества собственников жилья или кооператива
        /// </summary>
        public virtual FileInfo DispatchFile { get; set; }
        #endregion 
    }
}
