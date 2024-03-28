namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface ITemplateServService
    {
        IDataResult GetOptionsFields(BaseParams baseParams);

        IDataResult ConstructOptionsFields(BaseParams baseParams);

        IDataResult GetUnitMeasure(BaseParams baseParams);
    }
}