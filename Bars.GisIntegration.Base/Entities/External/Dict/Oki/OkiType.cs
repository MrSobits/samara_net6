namespace Bars.GisIntegration.Base.Entities.External.Dict.Oki
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Вид ОКИ (8)
    /// </summary>
    public class OkiType : BaseEntity
    {
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public virtual string OkiTypeShort { get; set; }
        /// <summary>
        /// Типы групп объектов
        /// </summary>
        public virtual OkiTypeGroup OkiTypeGroup { get; set; }
        /// <summary>
        /// Код
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// ГУИД из ГИС
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
