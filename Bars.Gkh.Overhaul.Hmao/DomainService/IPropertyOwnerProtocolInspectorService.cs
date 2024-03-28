namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using Bars.B4;
    using System.Linq;
    using Entities;

    public interface IPropertyOwnerProtocolInspectorService
    {
        IDataResult AddInspectors(BaseParams baseParams);
        IDataResult AddDicisions(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);
    }
}