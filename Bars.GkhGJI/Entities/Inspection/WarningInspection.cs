namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    using ControlType = Bars.GkhGji.Entities.Dict.ControlType;

    /// <summary>
    /// Основание предостережения ГЖИ (РТ)
    /// </summary>
    public class WarningInspection : InspectionGji
    {
        /// <summary>
        /// Дата предостережения
        /// </summary>
        public virtual DateTime Date { get; set; }

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
        /// Форма поступления
        /// </summary>
        public virtual TypeBase SourceFormType { get; set; }

        /// <summary>
        /// Основание создания проверки
        /// </summary>
        public virtual InspectionCreationBasis InspectionBasis { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Обращение гражданина
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }
        
        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType WarningInspectionControlType { get; set; }
        
        /// <summary>
        /// ИНН физ./долж. лица
        /// </summary>
        public virtual string Inn { get; set; }
    }
}