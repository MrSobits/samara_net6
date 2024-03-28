namespace Bars.GkhGji.Entities.Dict
{
	using Bars.Gkh.Entities;
	using Bars.GkhGji.Enums;

	/// <summary>
    /// Коды документов
    /// </summary>
    public class DocumentCode : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual TypeDocumentGji Type { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }
    }
}
