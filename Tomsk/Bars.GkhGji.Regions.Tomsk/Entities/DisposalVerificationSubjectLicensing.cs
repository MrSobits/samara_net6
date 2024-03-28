namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using B4.DataAccess;
    using GkhGji.Entities;
    using GkhGji.Entities.Dict;

	/// <summary>
	/// Предмет проверки для приказа лицензирование
	/// </summary>
	public class DisposalVerificationSubjectLicensing : BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

		/// <summary>
		/// Предмет проверки Лицензирование
		/// </summary>
		public virtual SurveySubjectLicensing SurveySubject { get; set; }
	}
}
