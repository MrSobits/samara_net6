namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    using Castle.Windsor;

    public class DisposalFactViolationService : IDisposalFactViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFactViolation(BaseParams baseParams)
        {
            var dispFactViolDomain = this.Container.ResolveDomain<DisposalFactViolation>();
            var factViolDomain = this.Container.ResolveDomain<TypeFactViolation>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {
                var disposalId = baseParams.Params.GetAsId("disposalId");
                var factViolIds = baseParams.Params.GetAs<string>("factViolIds").ToLongArray();

                var existRecs = dispFactViolDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposalId)
                    .ToList();

                var disposal = disposalDomain.Load(disposalId);

                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var disposalFactViolation in existRecs.Where(x => !factViolIds.Contains(x.TypeFactViolation.Id)))
                        {
                            dispFactViolDomain.Delete(disposalFactViolation.Id);
                        }

                        foreach (var factViol in factViolIds.Where(x => existRecs.All(y => y.TypeFactViolation.Id != x)))
                        {
                            dispFactViolDomain.Save(new DisposalFactViolation
                            {
                                Disposal = disposal,
                                TypeFactViolation = factViolDomain.Load(factViol)
                            });
                        }

                        tr.Commit();
                    }
                    catch(Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                this.Container.Release(dispFactViolDomain);
                this.Container.Release(factViolDomain);
                this.Container.Release(disposalDomain);
            }

            return new BaseDataResult();
        }
    }
}