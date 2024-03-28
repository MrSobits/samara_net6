namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сметы. Модель выборки
    /// </summary>
    public class EstimateGet : BaseEstimate<StateInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи во вкладке "Сметный расчет по работе"
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование работы
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public string FinancingSection { get; set; }

        /// <summary>
        /// Итого по смете
        /// </summary>
        public decimal? SumTotal { get; set; }

        /// <summary>
        /// Сумма по смете
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Наименование файла "Документ сметы"
        /// </summary>
        public string EstimateFileName { get; set; }

        /// <summary>
        /// Номер файла "Документ сметы"
        /// </summary>
        public string EstimateFileNumber { get; set; }

        /// <summary>
        /// Дата файла "Документ сметы"
        /// </summary>
        public DateTime? EstimateFileDate { get; set; }

        /// <summary>
        /// Идентификатор файла "Документ сметы"
        /// </summary>
        public long? EstimateFileId { get; set; }

        /// <summary>
        /// Наименование файла "Документ ведомости ресурсов"
        /// </summary>
        public string ResourceFileName { get; set; }

        /// <summary>
        /// Номер файла "Документ ведомости ресурсов"
        /// </summary>
        public string ResourceFileNumber { get; set; }

        /// <summary>
        /// Дата файла "Документ ведомости ресурсов"
        /// </summary>
        public DateTime? ResourceFileDate { get; set; }

        /// <summary>
        /// Идентификатор файла "Документ ведомости ресурсов"
        /// </summary>
        public long? ResourceFileId { get; set; }
    }
    
    /// <summary>
    /// Сметы. Модель обновления
    /// </summary>
    public class EstimateUpdate : BaseEstimate<StateInfoUpdate>, IEntityId
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(EstimateCalculation))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Сметы. Базовая модель
    /// </summary>
    public class BaseEstimate<TStateInfo>
    {
        /// <summary>
        /// Статус записи "Сметный расчет по работе"
        /// </summary>
        public TStateInfo State { get; set; }
    }
}