namespace Bars.GkhGji.Regions.Nso.Entities
{
	using Bars.B4.DataAccess;
	using Bars.B4.Modules.States;
	using Bars.Gkh.Entities;

	using System;
	using Bars.B4.Modules.FIAS;
	using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Уведомление о смене способа управления МКД
    /// </summary>
	public class MkdChangeNotification : BaseEntity, IStatefulEntity
    {
		/// <summary>
		/// Регистрационный номер дела
		/// </summary>
		public virtual int RegistrationNumber { get; set; }
 
		/// <summary>
		/// Дата
		/// </summary>
		public virtual DateTime Date { get; set; }

		/// <summary>
		/// Адрес
		/// </summary>
		public virtual RealityObjectFantom RealityObjectFantom { get; set; }
		
		/// <summary>
		/// Причина уведомления
		/// </summary>
		public virtual NotificationCause NotificationCause { get; set; }

		/// <summary>
		/// Входящий номер
		/// </summary>
		public virtual string InboundNumber { get; set; }

		/// <summary>
		/// Дата регистрации уведомления
		/// </summary>
		public virtual DateTime RegistrationDate { get; set; }

		/// <summary>
		/// Предыдущий способ управления
		/// </summary>
		public virtual MkdManagementMethod OldMkdManagementMethod { get; set; }

		/// <summary>
		/// Управляющая организация предыдущего способа управления
		/// </summary>
		public virtual ManagingOrganization OldManagingOrganization { get; set; }

		/// <summary>
		/// ИНН предыдущего способа управления
		/// </summary>
		public virtual string OldInn { get; set; }

		/// <summary>
		/// ОГРН предыдущего способа управления
		/// </summary>
		public virtual string OldOgrn { get; set; }

		/// <summary>
		/// Новый способ управления
		/// </summary>
		public virtual MkdManagementMethod NewMkdManagementMethod { get; set; }

		/// <summary>
		/// Управляющая организация нового способа управления
		/// </summary>
		public virtual ManagingOrganization NewManagingOrganization { get; set; }

		/// <summary>
		/// ИНН нового способа управления
		/// </summary>
		public virtual string NewInn { get; set; }

		/// <summary>
		/// ОГРН нового способа управления
		/// </summary>
		public virtual string NewOgrn { get; set; }

		/// <summary>
		/// Юридический адрес нового способа управления
		/// </summary>
		public virtual string NewJuridicalAddress { get; set; }

		/// <summary>
		/// Руководитель нового способа управления
		/// </summary>
		public virtual string NewManager { get; set; }

		/// <summary>
		/// Телефон нового способа управления
		/// </summary>
		public virtual string NewPhone { get; set; }

		/// <summary>
		/// Email нового способа управления
		/// </summary>
		public virtual string NewEmail { get; set; }

		/// <summary>
		/// Официальный сайт нового способа управления
		/// </summary>
		public virtual string NewOfficialSite { get; set; }

		/// <summary>
		/// Дата предоставления копии акта
		/// </summary>
		public virtual DateTime? NewActCopyDate { get; set; }

		/// <summary>
		/// ФИАС адрес (не хранимое, нужно для получения значения с клиента)
		/// Перед сохранением по нему идет поиск жилого дома
		/// </summary>
		public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
		/// Статус
        /// </summary>
        public virtual State State { get; set; }
	}
}