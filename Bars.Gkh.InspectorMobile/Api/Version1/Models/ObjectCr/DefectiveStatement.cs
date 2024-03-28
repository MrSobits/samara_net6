namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Дефективные ведомости. Модель выборки
    /// </summary>
    public class DefectiveStatementGet : BaseDefectiveStatement<StateInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи в разделе "Дефективные ведомости"
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата дефективной ведомости
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Работа капитального ремонта объекта КР
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// Сумма по ведомости
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? FileId { get; set; }
    }

    /// <summary>
    /// Дефективные ведомости. Модель обновления
    /// </summary>
    public class DefectiveStatementUpdate : BaseDefectiveStatement<StateInfoUpdate>, IEntityId
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(DefectList))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Дефективные ведомости. Базовая модель
    /// </summary>
    public class BaseDefectiveStatement<TStateInfo>
    {
        /// <summary>
        /// Статус записи в разделе "Дефективные ведомости"
        /// </summary>
        public TStateInfo State { get; set; }
    }
}