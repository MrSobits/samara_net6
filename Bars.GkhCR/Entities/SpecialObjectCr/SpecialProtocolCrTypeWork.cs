namespace Bars.GkhCr.Entities
{
	using Bars.Gkh.Entities;

	/// <summary>
	/// Виды работ протокола(акта) КР
	/// </summary>
	public class SpecialProtocolCrTypeWork : BaseGkhEntity
	{
		/// <summary>
		/// Протокол КР
		/// </summary>
		public virtual SpecialProtocolCr Protocol { get; set; }

		/// <summary>
		/// Вид работы
		/// </summary>
		public virtual SpecialTypeWorkCr TypeWork { get; set; }
	}
}
