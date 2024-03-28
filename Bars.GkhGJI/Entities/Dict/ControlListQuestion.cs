namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using System;

    public class ControlListQuestion : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual ControlList ControlList { get; set; }

        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Наименование НПД
        /// </summary>
        public virtual string NPDName { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// ЕРКНМ ГУИД
        /// </summary>
        public virtual string ERKNMGuid { get; set; }

    }
}
