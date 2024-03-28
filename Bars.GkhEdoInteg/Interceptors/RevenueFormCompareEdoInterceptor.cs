using System.Linq;
using Bars.B4;
using Bars.GkhEdoInteg.Entities;

namespace Bars.GkhEdoInteg.Interceptors
{
    public class RevenueFormCompareEdoInterceptor : EmptyDomainInterceptor<RevenueFormCompareEdo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RevenueFormCompareEdo> service, RevenueFormCompareEdo entity)
        {
            return this.CheckToUnique(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RevenueFormCompareEdo> service, RevenueFormCompareEdo entity)
        {
            return this.CheckToUnique(service, entity);
        }

        private IDataResult CheckToUnique(IDomainService<RevenueFormCompareEdo> service, RevenueFormCompareEdo entity)
        {
            if (entity.CodeEdo > 0 && service.GetAll().Any(x => x.CodeEdo == entity.CodeEdo && x.Id != entity.Id))
            {
                return Failure("Существует запись с таким кодом ЭДО");
            }

            return new BaseDataResult();
        }
    }
}