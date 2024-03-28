namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
	/// Правовое основание проведения проверки приказа ГЖИ
	/// </summary>
	public class DisposalInspFoundation : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

		/// <summary>
		/// Правовое основание проведения проверки
		/// </summary>
		public virtual NormativeDoc InspFoundation { get; set; }
    }
}