namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public interface IEstimateCalculationService
    {
        IDataResult ListEstimateRegisterDetail(BaseParams baseParams);

        IQueryable<ViewObjCrEstimateCalc> GetFilteredByOperator();
    }
}