using Bars.GkhGji.Regions.Tomsk.Enums;

namespace Bars.GkhGji.Regions.Tomsk.Entities.ResolPros
{
    using System;

    using B4.DataAccess;
    using Gkh.Entities;
    using GkhGji.Entities;

    /// <summary>
    /// Определение постановления ГЖИ
    /// </summary>
    public class ResolProsDefinition : BaseEntity
    {
        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        public virtual ResolPros ResolPros { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// Время исполнения
        /// </summary>
        public virtual DateTime? ExecutionTime { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Постановление о возбуждении дела об административном правонарушении
        /// </summary>
        public virtual string ResolutionInitAdminViolation { get; set; }

        /// <summary>
        /// Доводы в обоснование возврата
        /// </summary>
        public virtual string ReturnReason { get; set; }

        /// <summary>
        /// Что требуется запросить
        /// </summary>
        public virtual string RequestNeed { get; set; }

        /// <summary>
        /// Дополнительные документы
        /// </summary>
        public virtual string AdditionalDocuments { get; set; }
        
        /// <summary>
        /// ДЛ, вынесшее определение
        /// </summary>
        public virtual Inspector IssuedDefinition { get; set; }

        /// <summary>
        /// Тип определения
        /// </summary>
        public virtual TypeResolProsDefinition TypeResolProsDefinition { get; set; }

        /// <summary>
        /// Срок предоставления документов
        /// </summary>
        public virtual DateTime? DateSubmissionDocument { get; set; }
    }
}