namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Таблица связи вида документа основания субъекта првоерки и Протокола
    /// </summary>
    public class ProtocolBaseDocument : BaseEntity
    {
		/// <summary>
		/// Протокол
		/// </summary>
        public virtual Protocol Protocol { get; set; }

        /// <summary>
        /// МКД
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

		/// <summary>
		/// Направление деятельности субъекта проверки
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