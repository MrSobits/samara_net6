namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using B4.Modules.States;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Служебная записка по Предписанию
    /// </summary>
    public class PrescriptionOfficialReport : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Предписание
        /// </summary>
        public virtual Prescription Prescription { get; set; }

        /// <summary>
        /// Тип служебной записки
        /// </summary>
        public virtual OfficialReportType OfficialReportType { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата устранения нарушений
        /// </summary>
        public virtual DateTime? ViolationDate { get; set; }

        /// <summary>
        /// Дата продления нарушений
        /// </summary>
        public virtual DateTime? ExtensionViolationDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Нарушения устранены
        /// </summary>
        public virtual YesNo YesNo { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Inspector
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

    }
}