using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Gkh.Entities;

    public class LongTermPrObjectController : B4.Alt.DataController<LongTermPrObject>
    {
        public ILongTermPrObjectService Service { get; set; }

        public void AddLongTermObjs()
        {
            var realObjDomain = Container.Resolve<IDomainService<RealityObject>>();
            var longPrObjectDomain = Container.Resolve<IDomainService<LongTermPrObject>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var realityObjs = realObjDomain.GetAll()
                        .Where(x => (x.TypeHouse == TypeHouse.ManyApartments 
                            || x.TypeHouse == TypeHouse.BlockedBuilding 
                            || x.TypeHouse == TypeHouse.SocialBehavior)
                                && !longPrObjectDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                        .Select(x => x.Id)
                        .ToArray();

                    foreach (var realityObjId in realityObjs)
                    {
                        longPrObjectDomain.Save(
                            new LongTermPrObject { RealityObject = realObjDomain.Load(realityObjId) });
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public ActionResult ListHasDecisionRegopAccount(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListHasDecisionRegopAccount(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult GetOrgForm(BaseParams baseParams)
        {
            var result = Service.GetOrgForm(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveDecision(BaseParams baseParams)
        {
            var result = Service.SaveDecision(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListAccounts(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListAccounts(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
