namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using System;

    /// <summary>
    /// Контрольно-надзорные действия
    /// </summary>
    public class DecisionControlList : BaseEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Мероприятие по контролю
        /// </summary>
        public virtual ControlList ControlList { get; set; }

        /// <summary>
        /// Описание мериприятия по контролю
        /// </summary>
        public virtual string Description { get; set; }       
    }
}