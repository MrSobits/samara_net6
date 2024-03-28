namespace Bars.GkhGji.Regions.Nso.Entities
{
	using System;
	using Bars.B4.Modules.FileStorage;
	using Bars.Gkh.Entities;

	/// <summary>
    /// Приложения протокола ГЖИ 19.7
    /// </summary>
	public class Protocol197Annex : BaseGkhEntity
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol197 Protocol197 { get; set; }

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