namespace Bars.GkhDi.Entities
{
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    public class DisclosureInfoEmptyFieldsBase : BaseGkhEntity
    {
        /// <summary>
        /// Наименование поля
        /// </summary>
        public virtual string FieldName { get; set; }

        /// <summary>
        /// Идентификатор пути к форме с полем
        /// </summary>
        public virtual DiFieldPathType PathId { get; set; }
    }
}