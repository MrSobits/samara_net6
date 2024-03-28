namespace Bars.GkhGji.Entities.Dict
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник "Идентификаторы ССТУ"
    /// </summary>
    public class SSTUTransferOrg : BaseGkhEntity
    {


        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Гуид
        /// </summary>
        public virtual string Guid { get; set; }


        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Address { get; set; }

    }
}