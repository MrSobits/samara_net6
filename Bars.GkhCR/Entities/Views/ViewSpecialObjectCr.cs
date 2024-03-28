namespace Bars.GkhCr.Entities
{
    using System;

    using B4.DataAccess;
    using B4.Modules.States;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Вьюха на Объект КР
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Объект КР для специальных счетов
     * с агрегированными показателями из реестра Виды Работ КР:
     * Сумма сумм всех работ данного объекта
     * Сумма сумм всех работ данного объекта(Тип работы - "Работа")
     * Сумма сумм всех работ данного объекта(Тип работы - "Услуга" Вид работ: "ПСД экспертиза" или "ПСД разработка")
     * Сумма сумм всех работ данного объекта(Тип работы - "Услуга" Вид работ: "Технадзор")
     */
    public class ViewSpecialObjectCr : PersistentObject
    {
        /// <summary>
        /// Статус мониторинга смр
        /// </summary>
        public virtual State SmrState { get; set; }

        /// <summary>
        /// Статус объекта кр
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер по программе
        /// </summary>
        public virtual string ProgramNum { get; set; }

        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual int? ProgramCrId { get; set; }

        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual string ProgramCrName { get; set; }

        /// <summary>
        /// Программа КР, на которую ссылался объект до удаления
        /// </summary>
        public virtual int? BeforeDeleteProgramCrId { get; set; }

        /// <summary>
        /// Программа КР, на которую ссылался объект до удаления
        /// </summary>
        public virtual string BeforeDeleteProgramCrName { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Id Муниципального района
        /// </summary>
        public virtual int MunicipalityId { get; set; }

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual int RealityObjectId { get; set; }

        /// <summary>
        /// Способ формирования фонда
        /// </summary>
        public virtual MethodFormFundCr MethodFormFund { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Дата принятия ГЖИ
        /// </summary>
        public virtual DateTime? DateAcceptCrGji { get; set; }

        /// <summary>
        /// Разрешение на повторное согласование
        /// </summary>
        public virtual bool AllowReneg { get; set; }

        /// <summary>
        /// Разрешение на повторное согласование
        /// </summary>
        public virtual int? MonitoringSmrId { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Id Муниципального образования
        /// </summary>
        public virtual int SettlementId { get; set; }

        /// <summary>
        /// Наименование периода
        /// </summary>
        public virtual string PeriodName { get; set; }
    }
}