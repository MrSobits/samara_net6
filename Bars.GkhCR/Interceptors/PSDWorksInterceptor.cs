using Bars.B4.Utils;

namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class PSDWorksInterceptor : EmptyDomainInterceptor<PSDWorks>
    {
       
        public IDomainService<PSDWorks> PSDWorksCrDomain { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }


        public override IDataResult AfterUpdateAction(IDomainService<PSDWorks> service, PSDWorks entity)
        {
            var psdWorkContainer = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var psdWork = TypeWorkCrDomain.Get(entity.PSDWork.Id);
            var worksForPSD = PSDWorksCrDomain.GetAll()
                .Where(x => x.PSDWork != null)
                .Where(x => x.PSDWork.Id == entity.PSDWork.Id)
                .Select(x => x.Cost).ToList();

            decimal summWorks = worksForPSD.Sum();

            psdWork.Sum = summWorks;
            psdWorkContainer.Update(psdWork);

            return Success();
        }

   
    }
}