using Bars.B4.DataAccess;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Категория заявителя
    /// </summary>
    public class AppealCitsCategory : BaseEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Категории заявителя
        /// </summary>
        public virtual ApplicantCategory ApplicantCategory { get; set; }
    }
}
