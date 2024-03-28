namespace Bars.GkhCr.DomainService.ObjectCr.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Utils;

    public class ProtocolCrTypeWorkService : IProtocolCrTypeWorkService
    {
        protected IWindsorContainer Container;
        protected IDomainService<Entities.ProtocolCrTypeWork> ProtocolCrTypeWorkDomain;
        protected IDomainService<Entities.ProtocolCr> ProtocolCrDomain;
        protected IDomainService<Entities.TypeWorkCr> TypeWorkCrDomain;

        public ProtocolCrTypeWorkService(IWindsorContainer container,
            IDomainService<Entities.ProtocolCrTypeWork> protocolCrTypeWorkDomain,
            IDomainService<Entities.ProtocolCr> protocolCrDomain,
            IDomainService<Entities.TypeWorkCr> typeWorkCrDomain)
        {
            Container = container;
            ProtocolCrTypeWorkDomain = protocolCrTypeWorkDomain;
            ProtocolCrDomain = protocolCrDomain;
            TypeWorkCrDomain = typeWorkCrDomain;
        }

        public IDataResult ListProtocolCrTypeWork(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var data = ProtocolCrTypeWorkDomain.GetAll()
                .Where(x => x.Protocol.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    x.TypeWork.Id,
                    WorkName = x.TypeWork.Work.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            var typeWorkCrIds = baseParams.Params.GetAs("typeWorkCrIds", string.Empty)
                .ToStr().Split(",").Select(x => x.Trim().ToLong()).ToList();

            var existTypeWorks = ProtocolCrTypeWorkDomain.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => x.TypeWork.Id)
                .ToList();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var typeWorkCrId in typeWorkCrIds)
                    {
                        if (existTypeWorks.Contains(typeWorkCrId))
                        {
                            continue;
                        }

                        var newProtocolCrTypeWork = new Entities.ProtocolCrTypeWork
                        {
                            Protocol = ProtocolCrDomain.Get(protocolId),
                            TypeWork = TypeWorkCrDomain.Get(typeWorkCrId)
                        };

                        ProtocolCrTypeWorkDomain.Save(newProtocolCrTypeWork);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return new BaseDataResult(false, "При добавлении видов работ произошла ошибка!");
                }
            }
        }
    }
}
