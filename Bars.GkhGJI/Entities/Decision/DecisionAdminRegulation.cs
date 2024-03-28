namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
	/// Административный регламент приказа ГЖИ
	/// </summary>
	public class DecisionAdminRegulation : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение ГЖИ 
        /// </summary>
        public virtual Decision Decision { get; set; }

		/// <summary>
		/// Административный регламент
		/// </summary>
		public virtual NormativeDoc AdminRegulation { get; set; }
    }
}