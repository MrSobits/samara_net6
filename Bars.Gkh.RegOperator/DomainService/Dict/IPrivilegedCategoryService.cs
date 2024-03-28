namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface IPrivilegedCategoryService
    {
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}