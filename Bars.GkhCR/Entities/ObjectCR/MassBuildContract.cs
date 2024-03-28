namespace Bars.GkhCr.Entities
{
    using System;

    using B4.Modules.FileStorage;
    using B4.Modules.States;

    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    using Gkh.Entities;
    using Enums;

    /// <summary>
    /// Договор подряда КР по многим домам
    /// </summary>
    public class MassBuildContract : BaseGkhEntity, IStatefulEntity
    {

        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }
        
        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Подрядчики
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Тип договора КР
        /// </summary>
        public virtual TypeContractBuild TypeContractBuild { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Договор внесен в реестр ГЖИ
        /// </summary>
        public virtual DateTime? DateInGjiRegister { get; set; }

        /// <summary>
        /// Дата от (документ)
        /// </summary>
        public virtual DateTime? DocumentDateFrom { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal? BudgetMo { get; set; }

        /// <summary>
        /// Бюджет субъекта
        /// </summary>
        public virtual decimal? BudgetSubject { get; set; }

        /// <summary>
        /// Средства собственников
        /// </summary>
        public virtual decimal? OwnerMeans { get; set; }

        /// <summary>
        /// Средства фонда
        /// </summary>
        public virtual decimal? FundMeans { get; set; }

        /// <summary>
        /// Дата отклонения от регистрации
        /// </summary>
        public virtual DateTime? DateCancelReg { get; set; }

        /// <summary>
        /// Дата принятия на регистрацию, но еще не зарегистрированно
        /// </summary>
        public virtual DateTime? DateAcceptOnReg { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Сумма договора подряда
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Начальная цена контракта договора подряда
        /// </summary>
        public virtual decimal? StartSum { get; set; }

        /// <summary>
        /// Файл (документ)
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Протокол
        /// </summary>
        public virtual string ProtocolName { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string ProtocolNum { get; set; }

        /// <summary>
        /// Дата от (протокол)
        /// </summary>
        public virtual DateTime? ProtocolDateFrom { get; set; }

        /// <summary>
        /// Файл (протокол)
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Дата расторжения
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo TerminationDocumentFile { get; set; }

        /// <summary>
        /// Основание расторжения
        /// </summary>
        public virtual string TerminationReason { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string TerminationDocumentNumber { get; set; }

        /// <summary>
        /// Причина расторжения из справочника
        /// </summary>
        public virtual TerminationReason TerminationDictReason { get; set; }

        /// <summary>
        /// Гарантийный срок (лет)
        /// </summary>
        public virtual int? GuaranteePeriod { get; set; }

        /// <summary>
        /// Ссылка на результаты проведения торгов
        /// </summary>
        public virtual string UrlResultTrading { get; set; }
    }
}
