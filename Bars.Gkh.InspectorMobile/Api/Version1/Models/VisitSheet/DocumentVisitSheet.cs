namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.VisitSheet
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Документ "Лист визита". Модель выборки
    /// </summary>
    public class DocumentVisitSheetGet : BaseDocumentVisitSheet<VisitSheetInformationProvidedGet, FileInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор документа "Лист визита"
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор проверки ГЖИ
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Уникальный идентификатор родительского документа листа визита - Задание по профилактическому мероприятию
        /// </summary>
        public long ParentDocumentId { get; set; }
        
        /// <summary>
        /// Номер документа "Лист визита"
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Подписанные файлы документа
        /// </summary>
        public IEnumerable<SignedFileInfo> SignedFiles { get; set; }
    }

    /// <summary>
    /// Документ "Лист визита". Модель создания
    /// </summary>
    public class DocumentVisitSheetCreate : BaseDocumentVisitSheet<VisitSheetInformationProvidedCreate, FileInfoCreate>
    {
        /// <summary>
        /// Уникальный идентификатор родительского документа листа визита - Задание по профилактическому мероприятию
        /// </summary>
        [RequiredExistsEntity(typeof(PreventiveActionTask))]
        public long? ParentDocumentId { get; set; }
    }

    /// <summary>
    /// Документ "Лист визита". Модель обновления
    /// </summary>
    public class DocumentVisitSheetUpdate : BaseDocumentVisitSheet<VisitSheetInformationProvidedUpdate, FileInfoUpdate>
    {
    }

    /// <summary>
    /// Документ "Лист визита". Базовая модель
    /// </summary>
    public abstract class BaseDocumentVisitSheet<TInformationProvided, TFileInfo>
    {
        /// <summary>
        /// Дата документа "Лист визита"
        /// </summary>
        [Required]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата проведения мероприятия с
        /// </summary>
        [Required]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата проведения мероприятия по
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Время мероприятия с
        /// </summary>
        [Required]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Время мероприятия по
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Идентификатор инспектора
        /// </summary>
        [RequiredExistsEntity(typeof(Inspector))]
        public long? InspectorId { get; set; }

        /// <summary>
        /// Экземпляр листа визита получен
        /// </summary>
        [Required]
        public bool? SignReceipt { get; set; }

        /// <summary>
        /// Информация о предоставленных данных
        /// </summary>
        public IEnumerable<TInformationProvided> InformationProvided { get; set; }

        /// <summary>
        /// Файлы - вложения документа
        /// </summary>
        public IEnumerable<TFileInfo> Files { get; set; }
    }
}