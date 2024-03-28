namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Должность
    /// </summary>
    public class Position : BaseGkhEntity
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
        /// Наименование, родительский падеж
        /// </summary>
        public virtual string NameGenitive { get; set; }

        /// <summary>
        /// Наименование, Дательный падеж
        /// </summary>
        public virtual string NameDative { get; set; }

        /// <summary>
        /// Наименование, Винительный падеж
        /// </summary>
        public virtual string NameAccusative { get; set; }

        /// <summary>
        /// Наименование, Творительный падеж
        /// </summary>
        public virtual string NameAblative { get; set; }

        /// <summary>
        /// Наименование, Предложный падеж
        /// </summary>
        public virtual string NamePrepositional { get; set; }
    }
}