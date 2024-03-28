using Bars.B4.DataAccess;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Правило проставления номера документа гжи
    /// </summary>
    public class DocNumValidationRule : BaseEntity
    {
        /// <summary>
        /// Id правила
        /// </summary>
        public virtual string RuleId { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }
    }
}