namespace Bars.Gkh.Entities.Dicts
{
	using Bars.Gkh.Entities;

    /// <summary>
	/// Приложение к обращению за выдачей лицензии
	/// </summary>
	public class AnnexToAppealForLicenseIssuance : BaseImportableEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
