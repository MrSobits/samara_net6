namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Tomsk.Enums;
    using GkhGji.Entities;

    /// <summary>
    /// Требование
    /// </summary>
    public class Requirement : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Адресат
        /// </summary>
        public virtual string Destination { get; set; }

        /// <summary>
        /// Ссылка на документ ГЖИ 
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Тип требования
        /// </summary>
        public virtual TypeRequirement TypeRequirement { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Срок предоставления материалов
        /// </summary>
        public virtual DateTime? MaterialSubmitDate { get; set; }

        /// <summary>
        /// Дополнительные материалы
        /// </summary>
        public virtual string AddMaterials { get; set; }

        /// <summary>
        /// Дата проведения проверки
        /// </summary>
        public virtual DateTime? InspectionDate { get; set; }

        /// <summary>
        /// Время проведения проверки: Час
        /// </summary>
        public virtual int? InspectionHour { get; set; }

        /// <summary>
        /// Время проведения проверки: Минута
        /// </summary>
        public virtual int? InspectionMinute { get; set; }
    }
}
