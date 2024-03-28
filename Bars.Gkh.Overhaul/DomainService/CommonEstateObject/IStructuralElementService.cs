namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.B4;

    public interface IStructuralElementService
    {
        IDataResult ListTree(BaseParams baseParams);

        IDataResult GetAttributes(BaseParams baseParams);

        IDataResult IsStructElForRequiredGroupsAdded(BaseParams baseParams);
    }
}