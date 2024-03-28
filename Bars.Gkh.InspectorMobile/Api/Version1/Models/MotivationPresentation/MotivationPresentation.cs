namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.MotivationPresentation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Модель для получения документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationGet : BaseMotivationPresentation<MotivationPresentationEventResultGet, FileInfoGet>
    {
        /// <summary>
        /// Идентификатор документа "Мотивированное представление"
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Идентификатор родительского документа Мотивированного представления - Акт КНМ
        /// </summary>
        public long? ParentDocumentId { get; set; }

        /// <summary>
        /// Номер документа "Мотивированного представления"
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Подписанные файлы документа
        /// </summary>
        public IEnumerable<SignedFileInfo> SignedFiles { get; set; }
    }

    /// <summary>
    /// Модель для создания документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationCreate : BaseMotivationPresentation<MotivationPresentationEventResultCreate, FileInfoCreate>
    {
        /// <summary>
        /// Идентификатор родительского документа Мотивированного представления - Акт КНМ
        /// </summary>
        [RequiredExistsEntity(typeof(ActActionIsolated))]
        public long? ParentDocumentId { get; set; }
    }

    /// <summary>
    /// Модель для редактирования документа "Мотивированное представление"
    /// </summary>
    public class MotivationPresentationUpdate : BaseMotivationPresentation<MotivationPresentationEventResultUpdate, FileInfoUpdate>
    {
    }

    /// <summary>
    /// Базовая модель документа "Мотивированное представление"
    /// </summary>
    public class BaseMotivationPresentation<TEventResult, TFileInfo>
        where TEventResult : BaseMotivationPresentationEventResult
        where TFileInfo : BaseFileInfo
    {
        /// <summary>
        /// Дата документа "Мотивированного представления"
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [Required]
        public FiasAddress DocumentPlace { get; set; }

        /// <summary>
        /// ДЛ, вынесшее мотивированное представление
        /// </summary>
        [OnlyExistsEntity(typeof(Inspector))]
        public long? ExecutiveId { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        [OnlyExistsEntity(typeof(Inspector))]
        public long? ResponsibleExecutionId { get; set; }

        /// <summary>
        /// Список инспекторов
        /// </summary>
        [RequiredExistsEntity(typeof(Inspector))]
        public IEnumerable<long> InspectorIds { get; set; }

        /// <summary>
        /// Перечень нарушений
        /// </summary>
        [Required]
        public IEnumerable<TEventResult> EventResults { get; set; }

        /// <summary>
        /// Файлы - вложения документа
        /// </summary>
        public IEnumerable<TFileInfo> Files { get; set; }
    }
}