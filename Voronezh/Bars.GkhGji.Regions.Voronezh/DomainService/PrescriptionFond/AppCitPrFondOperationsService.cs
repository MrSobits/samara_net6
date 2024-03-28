using Bars.B4;
using Bars.Gkh.Utils;
using Castle.Windsor;
using System.Linq;
using Bars.GkhCr.Entities;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class AppCitPrFondOperationsService : IAppCitPrFondOperationsService
    {     
        public IDomainService<MassBuildContractObjectCr> MassBuildContractObjectCrDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public IDataResult GetListObjectCr(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var bcId = loadParams.Filter.GetAs("bcId", 0L);
            if (bcId <= 0)
            {
                return new ListDataResult();
            }

            return MassBuildContractObjectCrDomain.GetAll()
                .Where(x => x.MassBuildContract.Id == bcId)
                .Select(x => new
                {
                    x.ObjectCr.Id,
                    Program = x.ObjectCr.ProgramCr.Name,
                    Address = x.ObjectCr.RealityObject.Address
                })
                .ToListDataResult(loadParams);
        }
    }
}
