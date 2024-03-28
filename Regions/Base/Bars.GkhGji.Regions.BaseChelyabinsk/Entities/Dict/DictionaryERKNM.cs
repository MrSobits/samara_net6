namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// Промежуточная сущность, которая хранит либо реальный жилой дом,
    /// который выбрал пользователь, либо если такого дома нет - произвольный адрес,
    /// который пользователь забил руками
    /// </summary>
    public class DictionaryERKNM : BaseEntity
	{
		/// <summary>
		/// Гуид справочника 
		/// </summary>
		public virtual string DictionaryERKNMGuid { get; set; }
        /// <summary>
		/// Название справочника 
		/// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Тип  
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
		/// Описание 
		/// </summary>
        public virtual string Description { get; set; }

        public virtual int? Order { get; set; }

        public virtual bool Required { get; set; }
       
        /// <summary>
        /// Дата последнего обновления  
        /// </summary>
        public virtual DateTime DateLastUpdate { get; set; }

        /// <summary>
        /// Наименование Ентити нашего справочника
        /// </summary>
        public virtual string EntityName { get; set; }

        /// <summary>
        /// ИД Ентити нашего справочника
        /// </summary>
        public virtual long EntityId { get; set; }



    }
}