namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Entities;
    using Castle.Windsor;

    public class DeliveryAgentService : IDeliveryAgentService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var domaneService = Container.Resolve<IDomainService<DeliveryAgent>>();

            var data = domaneService.GetAll().Select(x => new { x.Id, x.Contragent.Name }).Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Order(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var deliveryAgentDomain = Container.Resolve<IDomainService<DeliveryAgent>>();
                var deliveryAgentMuDomain = Container.Resolve<IDomainService<DeliveryAgentMunicipality>>();
                var municipalityDomain = Container.Resolve<IDomainService<Municipality>>();

                try
                {
                    var delAgentId = baseParams.Params.GetAs<long>("delAgentId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    var existRecs =
                        deliveryAgentMuDomain.GetAll()
                            .Where(x => x.DeliveryAgent.Id == delAgentId)
                            .Select(x => x.Municipality.Id)
                            .Distinct()
                            .AsEnumerable()
                            .ToDictionary(x => x);

                    var deliveryAgent = deliveryAgentDomain.Load(delAgentId);

                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new DeliveryAgentMunicipality
                        {
                            DeliveryAgent = deliveryAgent,
                            Municipality = municipalityDomain.Load(id)
                        };

                        deliveryAgentMuDomain.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                finally
                {
                    Container.Release(deliveryAgentDomain);
                    Container.Release(deliveryAgentMuDomain);
                    Container.Release(municipalityDomain);
                }
            }
        }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var deliveryAgentDomain = Container.Resolve<IDomainService<DeliveryAgent>>();
                var deliveryAgentRoDomain = Container.Resolve<IRepository<DeliveryAgentRealObj>>();
                var realObjectDomain = Container.Resolve<IDomainService<RealityObject>>();

                try
                {
                    var delAgentId = baseParams.Params.GetAs<long>("delAgentId");
                    var objectIds = baseParams.Params.GetAs("objectIds", new long[0]);
                    var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                    var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");

                    var existRoInPeriod =
                        deliveryAgentRoDomain.GetAll()
                            .Where(x => (x.DateStart <= dateStart && (!x.DateEnd.HasValue || x.DateEnd >= dateStart))
                                || (!dateEnd.HasValue && x.DateStart >= dateStart)
                                || (dateEnd.HasValue && x.DateStart <= dateEnd && (!x.DateEnd.HasValue || x.DateEnd >= dateEnd)))
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var delAgent = deliveryAgentDomain.Load(delAgentId);

                    var hasNoAddedRo = false;
                    foreach (var id in objectIds)
                    {
                        if (!existRoInPeriod.Contains(id))
                        {
                            var newDelAgentRealobj = new DeliveryAgentRealObj
                            {
                                RealityObject = realObjectDomain.Load(id),
                                DeliveryAgent = delAgent,
                                DateStart = dateStart,
                                DateEnd = dateEnd
                            };

                            deliveryAgentRoDomain.Save(newDelAgentRealobj);
                        }
                        else
                        {
                            hasNoAddedRo = true;
                        }
                    }

                    transaction.Commit();
                    return hasNoAddedRo ? new BaseDataResult(true, "Некоторые дома имеют действующий договор. Для добавления нового договора необходимо закрыть прошлый.")
                        : new BaseDataResult(true, "Жилые дома сохранены успешно");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                finally
                {
                    Container.Release(deliveryAgentDomain);
                    Container.Release(deliveryAgentRoDomain);
                    Container.Release(realObjectDomain);
                }
            }
        }

        public IDataResult ListRealObjForDelAgent(BaseParams baseParams)
        {
            var realObjDomain = Container.ResolveDomain<RealityObject>();
            var delAgentMuDomain = Container.ResolveDomain<DeliveryAgentMunicipality>();

            using (Container.Using(realObjDomain, delAgentMuDomain))
            {
                var loadParams = baseParams.GetLoadParam();
                var delAgentId = baseParams.Params.GetAsId("delAgentId");


                var data = realObjDomain
                             .GetAll()
                             .Where(x => delAgentMuDomain.GetAll().Where(y => y.DeliveryAgent.Id == delAgentId).Any(y => y.Municipality.Id == x.Municipality.Id))
                             .Select(x => new
                             {
                                 x.Id,
                                 x.Address,
                                 Municipality = x.Municipality.Name
                             })
                             .Order(loadParams)
                             .Filter(loadParams, Container);

                return new ListDataResult(data.Paging(loadParams), data.Count());
            }
        }
    }
}