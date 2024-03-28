namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
    /// Наименования требований по устранению нарушений
    /// </summary>
    public class ResolveViolationClaim : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }
    }
}