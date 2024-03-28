namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник "Группы льготных категорий граждан"
    /// </summary>
    public class PrivilegedCategory : BaseImportableEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Процент льготы
        /// </summary>
        public virtual decimal? Percent { get; set; }

        /// <summary>
        /// Действует с
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Действует по
        /// </summary>
        public virtual DateTime? DateTo { get; set; }

        /// <summary>
        /// Предельное значение площади
        /// </summary>
        public virtual decimal? LimitArea { get; set; }
    }
}