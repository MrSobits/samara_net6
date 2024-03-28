namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
	/// Промежуточная сущность, которая хранит либо реальный жилой дом,
	/// который выбрал пользователь, либо если такого дома нет - произвольный адрес,
	/// который пользователь забил руками
	/// </summary>
	public class DictionaryERKNMRecord : BaseEntity
	{
		public virtual DictionaryERKNM DictionaryERKNM { get; set; } 
		/// <summary>
		/// Индификатор 
		/// </summary>
		public virtual long RecId { get; set; }
		/// <summary>
		/// Наименование 
		/// </summary>
		public virtual string  Name  { get; set; }
		/// <summary>
		///  Второе Наименование  
		/// </summary>
		public virtual string Name1 { get; set; }
		/// <summary>
		///  Третье Наименование  
		/// </summary>
		public virtual string Name2 { get; set; }
		/// <summary>
		///  Имя справочника в системе 
		/// </summary>
		public virtual string EntityName { get; set; }
		/// <summary>
		///   ID справочника в системе 
		/// </summary>
		public virtual long? EntityId { get; set; }
	}
}