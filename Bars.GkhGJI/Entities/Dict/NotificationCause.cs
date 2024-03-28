namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
	/// Причина уведомления
    /// </summary>
	public class NotificationCause : BaseEntity
    {
        /// <summary>
		/// Наименование причины
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
		/// Код причины
        /// </summary>
        public virtual string Code { get; set; }
    }
}