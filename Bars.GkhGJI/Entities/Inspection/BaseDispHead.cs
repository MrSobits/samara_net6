namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание распоряжения руководителя ГЖИ
    /// </summary>
    public class BaseDispHead : InspectionGji
    {
        /// <summary>
        /// Руководитель
        /// </summary>
        public virtual Inspector Head { get; set; }

        /// <summary>
        /// Предыдущий документ
        /// </summary>
        public virtual DocumentGji PrevDocument { get; set; }

        /// <summary>
        /// Дата распоряжения
        /// </summary>
        public virtual DateTime? DispHeadDate { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Тип основания проверки поручения руководства
        /// </summary>
        public virtual TypeBaseDispHead TypeBaseDispHead { get; set; }

        /// <summary>
        /// Основание создания проверки
        /// </summary>
        public virtual InspectionBasis InspectionBasis { get; set; }

        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
        
        /// <summary>
        /// ИНН физ./долж. лица
        /// </summary>
        public virtual string Inn { get; set; }
    }
}