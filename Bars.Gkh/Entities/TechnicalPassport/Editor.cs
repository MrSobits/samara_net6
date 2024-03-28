namespace Bars.Gkh.Entities.TechnicalPassport
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.TechnicalPassport;

    /// <summary>
    /// Редактор
    /// </summary>
    public class Editor : BaseEntity
    {
        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Тип редактора
        /// </summary>
        public virtual EditorType EditorType { get; set; }

        /// <summary>
        /// Название таблицы (если тип редактора ссылочный)
        /// </summary>
        public virtual string ReferenceTableName { get; set; }

        /// <summary>
        /// Отображаемое значение (если тип редактора ссылочный)
        /// </summary>
        public virtual string DisplayValue { get; set; }

        /// <summary>
        /// Список возможных ограничений
        /// </summary>
        public virtual IList<ContstraintType> AvaliableConstraints { get; set; }
    }
}