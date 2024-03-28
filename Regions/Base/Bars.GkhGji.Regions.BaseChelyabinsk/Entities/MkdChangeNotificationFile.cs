namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
	/// Приложение уведомления о смене способа управления МКД
    /// </summary>
	public class MkdChangeNotificationFile : BaseEntity
    {
        /// <summary>
		/// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string Number { get; set; }

		/// <summary>
		/// Дата документа
		/// </summary>
		public virtual DateTime Date { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public virtual string Desc { get; set; }

		/// <summary>
		/// Уведомления о смене способа управления МКД
		/// </summary>
		public virtual MkdChangeNotification MkdChangeNotification { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
