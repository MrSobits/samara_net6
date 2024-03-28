using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class LongTermPrObjectController : B4.Alt.DataController<LongTermPrObject>
    {
        public void AddLongTermObjs()
        {
            var realObjDomain = Container.Resolve<IDomainService<Gkh.Entities.RealityObject>>();
            var longPrObjectDomain = Container.Resolve<IDomainService<LongTermPrObject>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            { 
                try
                {
                    var realityObjs = realObjDomain.GetAll()
                                     .Where(x => x.TypeHouse == TypeHouse.ManyApartments
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

        public ActionResult GetOrgForm (BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ILongTermPrObjectService>().GetOrgForm(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetManagingOrganization(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ILongTermPrObjectService>().GetManagingOrganization(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveDecision(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<ILongTermPrObjectService>().SaveDecision(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        } 
    }
}
