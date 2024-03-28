namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
	/// Способ управления МКД
    /// </summary>
	public class MkdManagementMethod : BaseEntity
    {
        /// <summary>
		/// Наименование способа
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
		/// Код способа
        /// </summary>
        public virtual string Code { get; set; }
    }
}