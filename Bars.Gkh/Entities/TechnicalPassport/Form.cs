namespace Bars.Gkh.Entities.TechnicalPassport
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.TechnicalPassport;

    /// <summary>
    /// Форма
    /// </summary>
    public class Form : BaseEntity
    {
        private readonly IList<FormAttribute> attributes;

        public Form()
        {
            this.attributes = new List<FormAttribute>();
        }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int Order { get; set; }

        /// <summary>
        /// Наименование таблицы
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// Секция
        /// </summary>
        public virtual Section Section { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual FormType Type { get; set; }

        /// <summary>
        /// Список аттрибутов
        /// </summary>
        public virtual IList<FormAttribute> Attributes => this.attributes;
    }

}