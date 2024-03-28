namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    using Castle.Windsor;

    public class DisposalFactViolationService : IDisposalFactViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFactViolation(BaseParams baseParams)
        {
            var dispFactViolDomain = Container.ResolveDomain<DisposalFactViolation>();
            var factViolDomain = Container.ResolveDomain<TypeFactViolation>();
            var disposalDomain = Container.ResolveDomain<Disposal>();

            try
            {
                var disposalId = baseParams.Params.GetAsId("disposalId");
                var factViolIds = baseParams.Params.GetAs<string>("factViolIds").ToLongArray();

                var existRecs = dispFactViolDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposalId)
                    .ToList();

                var disposal = disposalDomain.Load(disposalId);

                using (var tr = Container.Resolve<IDataTransaction>())
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
                Container.Release(dispFactViolDomain);
                Container.Release(factViolDomain);
                Container.Release(disposalDomain);
            }

            return new BaseDataResult();
        }
    }
}