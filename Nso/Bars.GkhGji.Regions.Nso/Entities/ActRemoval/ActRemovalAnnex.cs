namespace Bars.GkhGji.Regions.Nso.Entities
{
	using System;
	using Bars.B4.Modules.FileStorage;
	using Bars.Gkh.Entities;
	using Bars.GkhGji.Entities;

	/// <summary>
    /// Приложения акта проверки предписания ГЖИ
    /// </summary>
	public class ActRemovalAnnex : BaseGkhEntity
    {
        /// <summary>
        /// Акт проверки предписания
        /// </summary>
		public virtual ActRemoval ActRemoval { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}