namespace Bars.Gkh.Gji.DomainService
{
    using B4;

    public interface IGjiParamsService
    {
        IDataResult SaveParams(BaseParams baseParams);

        IDataResult GetParams();

        string GetParamByKey(string key);
    }
}