namespace Bars.Gkh.RegOperator.DomainService.RegoperatorParams
{
    using B4;

    public interface IRegoperatorParamsService
    {
        IDataResult SaveParams(BaseParams baseParams);

        IDataResult GetParams();

        string GetParamByKey(string key);

        IDataResult ValidateSave(BaseParams baseParams);

        T ReadParam<T>() where T : class;
    }
}