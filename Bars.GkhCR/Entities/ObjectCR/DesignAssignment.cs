namespace Bars.GkhCr.Entities
{
    using System;
    using System.Collections.Generic;

    using B4.Modules.States;

    using Bars.Gkh.Enums;

    using Gkh.Entities;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Задание на проектирование
    /// </summary>
    public class DesignAssignment : BaseGkhEntity, IStatefulEntity
    {
        private IList<DesignAssignmentTypeWorkCr> typeWorksCr;

        /// <summary>
        /// .ctor   
        /// </summary>
        public DesignAssignment()
        {
            this.typeWorksCr = new List<DesignAssignmentTypeWorkCr>();
        }

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        [Obsolete("Использовать объект DesignAssignmentTypeWorkCr")]
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string Document { get; set; }

        /// <summary>
        /// Дата размещения
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Виды работ
        /// </summary>
        public virtual IEnumerable<DesignAssignmentTypeWorkCr> TypeWorksCr => this.typeWorksCr;

        /// <summary>
        /// Добавить вид работ
        /// </summary>
        /// <param name="value">Значение</param>
        public virtual void Add(DesignAssignmentTypeWorkCr value)
        {
            this.typeWorksCr.Add(value);
        }
    }
}