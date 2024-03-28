using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GisIntegration.Base.Entities;
using Bars.GisIntegration.Smev.Entity;
using Bars.GisIntegration.Smev.Entity.ERKNM;
using System.Linq;

namespace Bars.GisIntegration.Smev.Interceptors
{
    public class RisTaskInterceptor : Base.Interceptors.RisTaskInterceptor
    {
        public override void AdditionalActions(RisTask entity)
        {
            var erpGuidDomain = Container.Resolve<IDomainService<ErpGuid>>();
            var erknmEntityDomain = Container.Resolve<IDomainService<ErknmEntity>>();
            using (Container.Using(erpGuidDomain, erknmEntityDomain))
            {
                erpGuidDomain.GetAll()
                    .Where(x => x.Task == entity)
                    .ForEach(x => erpGuidDomain.Delete(x.Id));

                erknmEntityDomain.GetAll()
                    .Where(x => x.Task == entity)
                    .ForEach(x => erknmEntityDomain.Delete(x.Id));
            }
        }
    }
}