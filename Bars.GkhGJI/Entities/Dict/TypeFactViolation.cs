namespace Bars.GkhGji.Entities.Dict
{
	using Bars.B4.DataAccess;

	/// <summary>
    /// Справочники - ГЖИ - Виды фактов нарушений.
    /// </summary>
    public class TypeFactViolation : BaseEntity
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
