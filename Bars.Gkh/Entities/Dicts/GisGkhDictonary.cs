using Bars.Gkh.Enums;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Справочники ГИС ЖКХ
    /// </summary>
    public class GisGkhDictonary : BaseGkhEntity
    {

        /// <summary>
        /// Тип справочника
        /// </summary>
        public virtual GisDictionaryType GisDictionaryType { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// ГУИД
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Значение справочника
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string EntityDict { get; set; }

        /// <summary>
        /// Системный справочник
        /// </summary>
        public virtual long EntityId { get; set; }
    }
}
