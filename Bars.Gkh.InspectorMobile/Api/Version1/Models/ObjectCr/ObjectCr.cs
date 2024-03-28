namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Информация об объекте капитального ремонта. Модель выборки
    /// </summary>
    public class ObjectCrGet : BaseObjectCr<DefectiveStatementGet, EstimateGet, ContractGet, ActGet>
    {
        /// <summary>
        /// Уникальный идентификатор объекта капитального ремонта
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор программы капитального ремонта
        /// </summary>
        public long ProgramId { get; set; }

        /// <summary>
        /// Уникальный идентификатор дома
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// Способ формирования фонда капитального ремонта
        /// </summary>
        public string FormWay { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата принятия ГЖИ
        /// </summary>
        public DateTime? DateAdoption { get; set; }

        /// <summary>
        /// Сумма на СМР
        /// </summary>
        public decimal? WorkSum { get; set; }

        /// <summary>
        /// Утвержденная сумма на СМР
        /// </summary>
        public decimal? ApprovedWorkSum { get; set; }

        /// <summary>
        /// Сумма на разработку экспертизы ПСД
        /// </summary>
        public decimal? ExpertiseSum { get; set; }

        /// <summary>
        /// Сумма на технадзор
        /// </summary>
        public decimal? TechnicalSum { get; set; }

        /// <summary>
        /// Работы капитального ремонта объекта КР
        /// </summary>
        public IEnumerable<TypeWorkCr> Work { get; set; }

        /// <summary>
        /// Протоколы и акты
        /// </summary>
        public IEnumerable<Protocol> Protocols { get; set; }
    }

    /// <summary>
    /// Информация об объекте капитального ремонта. Модель обновления
    /// </summary>
    public class ObjectCrUpdate : BaseObjectCr<DefectiveStatementUpdate, EstimateUpdate, ContractUpdate, ActUpdate>
    {
    }

    /// <summary>
    /// Информация об объекте капитального ремонта. Базовая модель
    /// </summary>
    public class BaseObjectCr<TDefectiveStatement, TEstimate, TContract, TAct>
    {
        /// <summary>
        /// Дефективные ведомости
        /// </summary>
        public IEnumerable<TDefectiveStatement> DefectiveStatements { get; set; }

        /// <summary>
        /// Сметы
        /// </summary>
        public IEnumerable<TEstimate> Estimates { get; set; }

        /// <summary>
        /// Договоры
        /// </summary>
        public IEnumerable<TContract> Contracts { get; set; }

        /// <summary>
        /// Акты выполненных работ
        /// </summary>
        public IEnumerable<TAct> Acts { get; set; }
    }
}