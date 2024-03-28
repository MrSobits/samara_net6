namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Представление "КЭ"
    /// </summary>
    /*
     * Вьюха КЭ домов, для веб-сервиса
     */
    public class ViewStructElementRealObj : PersistentObject
    {
        /// <summary>
        /// Идентификатор жилого дома
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Наименование группы конструктивного элемента (ООИ)
        /// </summary>
        public virtual string Ooi { get; set; }

        /// <summary>
        /// Наименование КЭ
        /// </summary>
        public virtual string NameStructEl { get; set; }

        /// <summary>
        /// Последний год кап ремонта или год установки
        /// </summary>
        public virtual int LastYear { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual decimal Wearout { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }
    }
}
