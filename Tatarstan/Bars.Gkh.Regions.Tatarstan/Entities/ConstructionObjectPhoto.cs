namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Regions.Tatarstan.Enums;

	/// <summary>
	/// Фото-архив объекта строительства
	/// </summary>
	public class ConstructionObjectPhoto : BaseEntity
    {
        /// <summary>
        /// Объект строительства
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }

		/// <summary>
		/// Дата изображения
		/// </summary>
		public virtual DateTime? Date { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// Группа
		/// </summary>
		public virtual ConstructionObjectPhotoGroup? Group { get; set; }

		/// <summary>
		/// Файл 
		/// </summary>
		public virtual FileInfo File { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public virtual string Description { get; set; }
    }
}
