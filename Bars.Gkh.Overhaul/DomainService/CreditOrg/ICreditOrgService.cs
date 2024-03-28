namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.B4;

    public interface ICreditOrgService
    {
        IDataResult ListExceptChildren(BaseParams baseParams);
    }
}
