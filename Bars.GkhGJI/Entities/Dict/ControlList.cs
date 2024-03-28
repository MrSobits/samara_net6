namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using System;

    public class ControlList : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual KindKNDGJI KindKNDGJI { get; set; }


        /// <summary>
        /// Дата актуальности с
        /// </summary>
        public virtual DateTime ActualFrom { get; set; }

        /// <summary>
        /// Дата актуальности по
        /// </summary>
        public virtual DateTime? ActualTo { get; set; }

        /// <summary>
        /// ЕРКНМ ГУИД
        /// </summary>
        public virtual string ERKNMGuid { get; set; }

    }
}
