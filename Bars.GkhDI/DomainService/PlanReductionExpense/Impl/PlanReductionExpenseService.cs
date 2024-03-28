using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhDi.Entities;
using Castle.Windsor;

namespace Bars.GkhDi.DomainService
{
    public class PlanReductionExpenseService : IPlanReductionExpenseService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddBaseService(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');

                var service = this.Container.Resolve<IDomainService<PlanReductionExpense>>();

                // получаем у контроллера услуги что бы не добавлять их повторно
                var exsistingPlanReductionExpense = service.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .Select(x => x.BaseService.Id)
                    .ToList();

                foreach (var id in objectIds)
                {
                    if (exsistingPlanReductionExpense.Contains(id.ToLong()))
                    {
                        continue;
                    }

                    var newId = id.ToLong();

                    var newPlanReductionExpense = new PlanReductionExpense
                    {
                        BaseService = new BaseService { Id = newId },
                        DisclosureInfoRealityObj = new DisclosureInfoRealityObj { Id = disclosureInfoRealityObjId }
                    };

                    service.Save(newPlanReductionExpense);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}
