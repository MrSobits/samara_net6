using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.DataAccess;
    using Enums;
    using DomainService;
    using Entities;

    public class LongTermPrObjectController : B4.Alt.DataController<LongTermPrObject>
    {
        public ILongTermPrObjectService Service { get; set; }

        public IDomainService<Gkh.Entities.RealityObject> realObjDomain { get; set; }

        public IDomainService<LongTermPrObject> longPrObjectDomain { get; set; }

        public void AddLongTermObjs()
        {
            var listToSave = new List<LongTermPrObject>();

            var realityObjs = realObjDomain.GetAll()
                .Where(x => !longPrObjectDomain.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments
                            || x.TypeHouse == TypeHouse.BlockedBuilding
                            || x.TypeHouse == TypeHouse.SocialBehavior)
                .Select(x => x.Id)
                .ToList();

            foreach (var realityObjId in realityObjs)
            {
                listToSave.Add(new LongTermPrObject { RealityObject = new Gkh.Entities.RealityObject { Id = realityObjId } });
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var item in listToSave)
                    {
                        longPrObjectDomain.Save(item);
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
            var result = (BaseDataResult)Service.GetOrgForm(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SaveDecision(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.SaveDecision(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        } 
    }
}
