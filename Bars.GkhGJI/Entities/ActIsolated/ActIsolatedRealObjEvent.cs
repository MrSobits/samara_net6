namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Мероприятия в доме для акта без взаимодействия
    /// </summary>
    public class ActIsolatedRealObjEvent : BaseEntity
    {
        /// <summary>
        /// Дом акта без взаимодействия
        /// </summary>
        public virtual ActIsolatedRealObj ActIsolatedRealObj { get; set; }

        /// <summary>
        /// Наименование мероприятия
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Срок проведения мероприятия (в днях)
        /// </summary>
        public virtual int Term { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime DateEnd { get; set; }
    }
}