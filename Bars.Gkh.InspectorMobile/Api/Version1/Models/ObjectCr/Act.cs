namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Акты выполненных работ. Модель выборки
    /// </summary>
    public class ActGet : BaseAct<StateInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование работы
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// Дата акта
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Объем выпиленных работ
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Сумма выполненных работ
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? FileId { get; set; }
    }

    /// <summary>
    /// Акты выполненных работ. Модель обновления
    /// </summary>
    public class ActUpdate : BaseAct<StateInfoUpdate>, IEntityId
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(PerformedWorkAct))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Акты выполненных работ. Базовая модель
    /// </summary>
    public class BaseAct<TStateInfo>
    {
        /// <summary>
        /// Статус записи в разделе "Акты выполненных работ"
        /// </summary>
        public TStateInfo State { get; set; }
    }
}