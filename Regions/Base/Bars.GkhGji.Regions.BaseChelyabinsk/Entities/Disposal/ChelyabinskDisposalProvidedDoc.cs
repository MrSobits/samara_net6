namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal
{
    using Bars.GkhGji.Entities;

    /// <summary>
	/// Предоставляемые документы рапоряжения ГЖИ (Расширение для Новосибирска)
	/// </summary>
	public class ChelyabinskDisposalProvidedDoc : DisposalProvidedDoc
	{
		/// <summary>
		/// Порядок, в котором пользователь добавил документ
		/// </summary>
		public virtual int Order { get; set; }
	}
}