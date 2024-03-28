namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using System;

    /// <summary>
    /// Контрольно-надзорные действия
    /// </summary>
    public class DecisionControlMeasures : BaseEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Мероприятие по контролю
        /// </summary>
        public virtual ControlActivity ControlActivity { get; set; }

        /// <summary>
        /// Описание мериприятия по контролю
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата начала мероприятия по контролю
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        ///  Дата окончания мероприятия по контролю
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}