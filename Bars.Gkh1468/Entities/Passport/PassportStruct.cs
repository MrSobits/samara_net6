namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Enums;

    public class PassportStruct : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Месяц начала действия
        /// </summary>
        public virtual int ValidFromMonth { get; set; }

        /// <summary>
        /// Год начала действия
        /// </summary>
        public virtual int ValidFromYear { get; set; }

        /// <summary>
        /// Тип паспорта
        /// </summary>
        public virtual PassportType PassportType { get; set; }
    }
}
