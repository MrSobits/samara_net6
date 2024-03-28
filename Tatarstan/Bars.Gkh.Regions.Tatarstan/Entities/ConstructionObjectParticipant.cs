namespace Bars.Gkh.Regions.Tatarstan.Entities
{
	using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

	/// <summary>
	/// Участник объекта строительства
	/// </summary>
	public class ConstructionObjectParticipant : BaseGkhEntity
    {
        /// <summary>
        /// Объект строительства
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }

		/// <summary>
		/// Тип участника строительства
		/// </summary>
		public virtual ConstructionObjectParticipantType ParticipantType { get; set; }

		/// <summary>
		/// Тип заказчика
		/// </summary>
		public virtual ConstructionObjectCustomerType? CustomerType { get; set; }
		
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

		/// <summary>
		/// Дополнительная информация
		/// </summary>
		public virtual string Description { get; set; }
    }
}
