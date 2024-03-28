namespace Bars.Gkh.RegOperator.Entities.Owner
{
	using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
	/// Данные о собственнике
	/// </summary>
	public class PersonalAccountOwnerInformation : BaseImportableEntity
    {
		/// <summary>
		/// Номер документа о собственности
		/// </summary>
		public virtual string DocumentNumber { get; set; }

		/// <summary>
		/// Доля собственности из документа о собственности
		/// </summary>
		public virtual decimal AreaShare { get; set; }

		/// <summary>
		/// Дата начала документа о собственности
		/// </summary>
		public virtual DateTime StartDate { get; set; }

		/// <summary>
		/// Дата окончания документа о собственности
		/// </summary>
		public virtual DateTime? EndDate { get; set; }

		/// <summary>
		/// Лицевой счет
		/// </summary>
		public virtual BasePersonalAccount BasePersonalAccount { get; set; }

		/// <summary>
		/// Абонент
		/// </summary>
		public virtual PersonalAccountOwner Owner { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}