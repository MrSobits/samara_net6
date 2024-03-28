namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;
    using RegOperator.Distribution;

    public class SubsidyIncomeController : B4.Alt.DataController<SubsidyIncome>
    {
        public ActionResult Apply(BaseParams baseParams)
        {
            var subsidyIncomeService = Container.Resolve<ISubsidyIncomeService>();

            try
            {
                return new JsonNetResult(subsidyIncomeService.Apply(baseParams));
            }
            finally
            {
                Container.Release(subsidyIncomeService);
            }
        }

        public ActionResult Undo(BaseParams baseParams)
        {
            var subsidyIncomeService = Container.Resolve<ISubsidyIncomeService>();

            try
            {
                return new JsonNetResult(subsidyIncomeService.Undo(baseParams));
            }
            finally
            {
                Container.Release(subsidyIncomeService);
            }
        }

        public ActionResult ListSubsidyDistribution(BaseParams baseParams)
        {
            var distributionProvider = Container.Resolve<IDistributionProvider>();

            try
            {
                return new JsonNetResult(distributionProvider.ListSubsidyDistribution(baseParams));
            }
            finally
            {
                Container.Release(distributionProvider);
            }    
        }

        public ActionResult CheckDate(BaseParams baseParams)
        {
            var subsidyIncomeService = Container.Resolve<ISubsidyIncomeService>();

            try
            {
                var result = subsidyIncomeService.CheckDate(baseParams);

                if (!result.Success)
                {
                    return JsonNetResult.Failure(result.Message);
                }

                return JsSuccess();
            }
            finally
            {
                Container.Release(subsidyIncomeService);
            }
        }
    }
}