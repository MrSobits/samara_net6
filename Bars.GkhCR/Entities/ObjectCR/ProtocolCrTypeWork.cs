namespace Bars.GkhCr.Entities
{
	using Bars.Gkh.Entities;

	/// <summary>
	/// Виды работ протокола(акта) КР
	/// </summary>
	public class ProtocolCrTypeWork : BaseGkhEntity
	{
		/// <summary>
		/// Протокол КР
		/// </summary>
		public virtual ProtocolCr Protocol { get; set; }

		/// <summary>
		/// Вид работы
		/// </summary>
		public virtual TypeWorkCr TypeWork { get; set; }
	}
}
