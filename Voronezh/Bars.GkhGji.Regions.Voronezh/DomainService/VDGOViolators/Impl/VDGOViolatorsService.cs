using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Castle.Windsor;
using System.Collections;
using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class VDGOViolatorsService : IVDGOViolatorsService
    {
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public virtual IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount)
        {
            var service = this.Container.Resolve<IDomainService<VDGOViolators>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var data = service.GetAll()
                            .Filter(loadParam, this.Container);

                totalCount = data.Count();

                return data.Order(loadParam).Paging(loadParam).ToList();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public IDataResult GetListRO(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var RealityObjectRepo = this.Container.Resolve<IRepository<RealityObject>>();

            var data = RealityObjectRepo.GetAll()
                .Select(x => new 
                { 
                    x.Id, 
                    x.Address 
                })
                .Filter(loadParams, Container);


            return new BaseDataResult(data.Order(loadParams).ToList());
        }

        public IDataResult GetListMinOrgContragent(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var ContragentRepo = this.Container.Resolve<IRepository<Contragent>>();

            var data = ContragentRepo.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, Container);

            return new BaseDataResult(data.Order(loadParams).ToList());
        }
        
    }
}
