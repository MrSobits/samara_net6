using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Entities
{
    public class AppealCitsTypeOfFeedback : BaseEntity
    {
        public virtual AppealCits AppealCits { get; set; }

        public virtual TypeOfFeedback TypeOfFeedback { get; set; }

        public virtual FileInfo FileInfo { get; set; }
    }
}
