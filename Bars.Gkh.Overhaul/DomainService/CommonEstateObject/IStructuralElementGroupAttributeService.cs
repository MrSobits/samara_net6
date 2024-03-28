namespace Bars.Gkh.Overhaul.DomainService
{
    using B4;

    public interface IStructuralElementGroupAttributeService
    {
        IDataResult ListWithResolvers(BaseParams baseParams);
    }
}