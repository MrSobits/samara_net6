namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Представление проверки ГЖИ для экспорта <see cref="IExportFormatProvider"/>
    /// </summary>
    public class ViewFormatDataExportInspection : BaseEntity
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Акт проверки
        /// </summary>
        public virtual ActCheck ActCheck { get; set; }

        /// <summary>
        /// Проверка плановая
        /// </summary>
        public virtual bool IsPlanned { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Субъект проверки
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Наименование субъекта проверки
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Наименование муниципального образования
        /// </summary>
        public virtual string MunicipalityName { get; set; }
    }
}