
namespace Bars.GkhGji.Regions.Nso.Entities.Disposal
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    public class DisposalLongText : BaseEntity
    {
        public virtual NsoDisposal Disposal { get; set; }

        public virtual byte[] NoticeDescription { get; set; }
    }
}