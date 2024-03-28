namespace Bars.Gkh.Entities.TechnicalPassport
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.TechnicalPassport;
    using System.Collections.Generic;

    /// <summary>
    /// Аттрибут формы
    /// </summary>
    public class FormAttribute : BaseEntity
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
        /// Отображаемый текст
        /// </summary>
        public virtual string DisplayText { get; set; }

        /// <summary>
        /// Название столбца
        /// </summary>
        public virtual string ColumnName { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public virtual bool Required { get; set; }

        /// <summary>
        /// Форма
        /// </summary>
        public virtual Form Form { get; set; }

        /// <summary>
        /// Редактор
        /// </summary>
        public virtual Editor Editor { get; set; }

        /// <summary>
        /// Ограничения
        /// </summary>
        public virtual IDictionary<ContstraintType, int> Contstraints { get; set; }
    }
}