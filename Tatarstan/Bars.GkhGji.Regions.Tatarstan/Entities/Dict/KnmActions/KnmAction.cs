using Bars.B4.DataAccess;
using Bars.GkhGji.Regions.Tatarstan.Enums;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    /// <summary>
    /// Действие в рамках КНМ
    /// </summary>
    public class KnmAction : BaseEntity
    {
        /// <summary>
        /// Вид действия
        /// </summary>
        public virtual ActCheckActionType? ActCheckActionType { get; set; }

        /// <summary>
        /// Идентификатор в ЕРВК
        /// </summary>
        public virtual string ErvkId { get; set; }
    }
}
