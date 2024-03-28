using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Entities
{
    public class TypeOfFeedback : BaseEntity
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }
    }
}
