namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочники - ГЖИ - Правовые основания
    /// </summary>
    public class LegalReason : BaseGkhEntity
    {
		/// <summary>
		/// Код
		/// </summary>
        public virtual string Code { get; set; }

		/// <summary>
		/// Наименование
		/// </summary>
        public virtual string Name { get; set; }
    }
}