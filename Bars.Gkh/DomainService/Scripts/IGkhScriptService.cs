namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IGkhScriptService
    {
        IDataResult CorrectJskTsjContract(BaseParams baseParams);

        IDataResult CorrectContractJskTsj(BaseParams baseParams);

        IDataResult CreateRelation(BaseParams baseParams);

        IDataResult CreateRelationSecond(BaseParams baseParams);
    }
}