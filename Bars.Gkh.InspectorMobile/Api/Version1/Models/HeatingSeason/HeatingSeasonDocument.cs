namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Документ по подготовке к отопительному сезону. Модель выборки
    /// </summary>
    public class HeatingSeasonDocumentGet : BaseHeatingSeasonDocument<StateInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип документа по подготовке к отопительному сезону
        /// </summary>
        public HeatSeasonDocType DocumentType { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? FileId { get; set; }
    }

    /// <summary>
    /// Документ по подготовке к отопительному сезону. Модель обновления
    /// </summary>
    public class HeatingSeasonDocumentUpdate : BaseHeatingSeasonDocument<StateInfoUpdate>, IEntityId
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(HeatSeasonDoc))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Документ по подготовке к отопительному сезону. Базовая модель
    /// </summary>
    public class BaseHeatingSeasonDocument<TStateInfo>
    {
        /// <summary>
        /// Статус
        /// </summary>
        [Required]
        public TStateInfo State { get; set; }
    }
}