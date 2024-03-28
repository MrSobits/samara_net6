using Bars.B4.DataAccess;

namespace Bars.Gkh.Modules.ClaimWork.Entities
{

    /// <summary>
    /// Справочник нарушений претензионной работы 
    /// </summary>
    public class ViolClaimWork : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименваоние
        /// </summary>
        public virtual string Name { get; set; }
    }
}