namespace Bars.GkhCr.DomainService.ObjectCr.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Gkh.Domain;

    public class SpecialProtocolCrTypeWorkService : ISpecialProtocolCrTypeWorkService
    {
        protected IWindsorContainer Container;
        protected IDomainService<Entities.SpecialProtocolCrTypeWork> ProtocolCrTypeWorkDomain;
        protected IDomainService<Entities.SpecialProtocolCr> ProtocolCrDomain;
        protected IDomainService<Entities.SpecialTypeWorkCr> TypeWorkCrDomain;

        public SpecialProtocolCrTypeWorkService(IWindsorContainer container,
            IDomainService<Entities.SpecialProtocolCrTypeWork> protocolCrTypeWorkDomain,
            IDomainService<Entities.SpecialProtocolCr> protocolCrDomain,
            IDomainService<Entities.SpecialTypeWorkCr> typeWorkCrDomain)
        {
            this.Container = container;
            this.ProtocolCrTypeWorkDomain = protocolCrTypeWorkDomain;
            this.ProtocolCrDomain = protocolCrDomain;
            this.TypeWorkCrDomain = typeWorkCrDomain;
        }

        public IDataResult ListProtocolCrTypeWork(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var data = this.ProtocolCrTypeWorkDomain.GetAll()
                .Where(x => x.Protocol.ObjectCr.Id == objectCrId)
                .Select(x => new
                {
                    x.TypeWork.Id,
                    WorkName = x.TypeWork.Work.Name
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            var typeWorkCrIds = baseParams.Params.GetAs("typeWorkCrIds", string.Empty)
                .ToStr().Split(",").Select(x => x.Trim().ToLong()).ToList();

            var existTypeWorks = this.ProtocolCrTypeWorkDomain.GetAll()
                .Where(x => x.Protocol.Id == protocolId)
                .Select(x => x.TypeWork.Id)
                .ToList();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var typeWorkCrId in typeWorkCrIds)
                    {
                        if (existTypeWorks.Contains(typeWorkCrId))
                        {
                            continue;
                        }

                        var newProtocolCrTypeWork = new Entities.SpecialProtocolCrTypeWork
                        {
                            Protocol = this.ProtocolCrDomain.Get(protocolId),
                            TypeWork = this.TypeWorkCrDomain.Get(typeWorkCrId)
                        };

                        this.ProtocolCrTypeWorkDomain.Save(newProtocolCrTypeWork);
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
