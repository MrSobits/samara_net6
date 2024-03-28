namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Таблица связи вида документа основания субъекат првоерки и Предписания
    /// </summary>
    public class PrescriptionBaseDocument : BaseEntity
    {

		/// <summary>
		/// Предписание
		/// </summary>
        public virtual Prescription Prescription { get; set; }

		/// <summary>
		/// Направление деятельности субъекта првоерки
		/// </summary>
        public virtual KindBaseDocument KindBaseDocument { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DateDoc { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string NumDoc { get; set; }
    }
}