namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
	/// Таблица связи Направдения деятелньости субъекта проверки и Предписания
	/// </summary>
	public class PrescriptionActivityDirection : BaseEntity
    {

		/// <summary>
		/// Предписание
		/// </summary>
        public virtual Prescription Prescription { get; set; }

		/// <summary>
		/// Направление деятельности субъекта првоерки
		/// </summary>
        public virtual ActivityDirection ActivityDirection { get; set; }
    }
}