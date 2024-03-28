namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Castle.Windsor;

    using Gkh.Entities;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class RealityObjectMissingCeoService : IRealityObjectMissingCeoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectMissingCeo> ServiceRealObjMissingCommonEstObj { get; set; }

        public IDomainService<RealityObject> ServiceRealityObject { get; set; }

        public IDomainService<CommonEstateObject> ServiceCommonEstateObject { get; set; }

        public IDataResult AddMissingCeo(BaseParams baseParams)
        {
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var commonEstObjIds = baseParams.Params.GetAs<long[]>("commonEstObjIds");

            var exsisting =
                ServiceRealObjMissingCommonEstObj.GetAll()
                    .Where(x => x.RealityObject.Id == realObjId)
                    .Select(x => x.MissingCommonEstateObject.Id)
                    .ToList();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var newId in commonEstObjIds.Where(id => !exsisting.Contains(id)))
                    {
                        var newInspectorSubscription = new RealityObjectMissingCeo
                        {
                            RealityObject = ServiceRealityObject.Load(realObjId),
                            MissingCommonEstateObject = ServiceCommonEstateObject.Load(newId)
                        };

                        ServiceRealObjMissingCommonEstObj.Save(newInspectorSubscription);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = "Произошла ошибка при сохранении" };
                }
            }
        }
    }
}