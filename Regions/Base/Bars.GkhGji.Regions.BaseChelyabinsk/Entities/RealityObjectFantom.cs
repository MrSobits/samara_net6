namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
	/// Промежуточная сущность, которая хранит либо реальный жилой дом,
	/// который выбрал пользователь, либо если такого дома нет - произвольный адрес,
	/// который пользователь забил руками
	/// </summary>
	public class RealityObjectFantom : BaseEntity
	{
		/// <summary>
		/// Жилой дом
		/// </summary>
		public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality MunicipalityFantom { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality SettlementFantom { get; set; }

		/// <summary>
		/// Адрес, который ввел пользователь, если жилого дома нет в системе,
		/// может быть в произвольной форме
		/// </summary>
		public virtual string Fantom { get; set; }
	}
}