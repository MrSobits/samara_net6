namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Проверка в реестре электронного документооборота
    /// </summary>
    public class EDSInspection : BaseEntity
    {
        /// <summary>
        /// Проверка
        /// </summary>
        public virtual InspectionGji InspectionGji { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime InspectionDate { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Не прочитано
        /// </summary>
        public virtual bool NotOpened { get; set; }
    }
}