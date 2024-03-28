
namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal
{
    using Bars.B4.DataAccess;

    public class DisposalLongText : BaseEntity
    {
        public virtual ChelyabinskDisposal Disposal { get; set; }

        public virtual byte[] NoticeDescription { get; set; }
    }
}