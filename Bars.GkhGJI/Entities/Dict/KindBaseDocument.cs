namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
    /// Вид документа-основания субъекта проверки
    /// </summary>
    public class KindBaseDocument : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}