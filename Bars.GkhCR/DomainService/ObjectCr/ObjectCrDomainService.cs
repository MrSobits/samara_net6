namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.DomainService.BaseParams;

    public class ObjectCrDomainService : BaseDomainService<Entities.ObjectCr>
    {
       

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");

            var objectCrList = this.Container.ResolveDomain<Entities.ObjectCr>().GetAll()
                .Where(x => ids.Contains(x.Id))
                .ToList();

            this.InTransaction(() =>
            {
                foreach (var objectCr in objectCrList)
                {
                    objectCr.BeforeDeleteProgramCr = objectCr.ProgramCr;
                    objectCr.ProgramCr = null;

                    IDataResult result = null;
                    var interceptors = this.Container.ResolveAll<IDomainServiceInterceptor<Entities.ObjectCr>>();

                    try
                    {
                        this.CallBeforeDeleteInterceptors(objectCr, ref result, interceptors);
                        this.UpdateEntityInternal(objectCr);
                        this.CallAfterDeleteInterceptors(objectCr, ref result, interceptors);
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