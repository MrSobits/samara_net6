namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities.Base;

    /// <summary>
    /// Лист визита. Приложение
    /// </summary>
    public class VisitSheetAnnex: BaseEntity, IAnnexEntity
    {
        /// <summary>
        /// Лист визита
        /// </summary>
        public virtual VisitSheet VisitSheet { get; set; }
        
        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}