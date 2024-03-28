namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.GkhCr.Regions.Tatarstan.Entities;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrDomainService : BaseDomainService<ObjectOutdoorCr>
    {
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            var objectOutdoorDomain = this.Container.ResolveDomain<ObjectOutdoorCr>();
            
            using (this.Container.Using(objectOutdoorDomain))
            {

                var objectOutdoorCrList = objectOutdoorDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();

                var interceptors = this.Container.ResolveAll<IDomainServiceInterceptor<ObjectOutdoorCr>>();

                this.InTransaction(() =>
                {
                    foreach (var objectOutdoorCr in objectOutdoorCrList)
                    {
                        objectOutdoorCr.BeforeDeleteRealityObjectOutdoorProgram = objectOutdoorCr.RealityObjectOutdoorProgram;
                        objectOutdoorCr.RealityObjectOutdoorProgram = null;

                        IDataResult result = null;

                        try
                        {
                            this.CallBeforeDeleteInterceptors(objectOutdoorCr, ref result, interceptors);
                            this.UpdateEntityInternal(objectOutdoorCr);
                            this.CallAfterDeleteInterceptors(objectOutdoorCr, ref result, interceptors);
                        }
                        finally
                        {
                            this.ReleaseInterceptors(interceptors);
                        }
                    }
                });

                return new BaseDataResult(ids);
            }
        }
    }
}
