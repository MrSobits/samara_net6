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
    public class SpecialDesignAssignment : BaseGkhEntity, IStatefulEntity
    {
        private IList<SpecialDesignAssignmentTypeWorkCr> typeWorksCr;

        /// <summary>
        /// .ctor   
        /// </summary>
        public SpecialDesignAssignment()
        {
            this.typeWorksCr = new List<SpecialDesignAssignmentTypeWorkCr>();
        }

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual SpecialObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        [Obsolete("Использовать объект DesignAssignmentTypeWorkCr")]
        public virtual SpecialTypeWorkCr TypeWorkCr { get; set; }

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
        public virtual IEnumerable<SpecialDesignAssignmentTypeWorkCr> TypeWorksCr => this.typeWorksCr;

        /// <summary>
        /// Добавить вид работ
        /// </summary>
        /// <param name="value">Значение</param>
        public virtual void Add(SpecialDesignAssignmentTypeWorkCr value)
        {
            this.typeWorksCr.Add(value);
        }
    }
}