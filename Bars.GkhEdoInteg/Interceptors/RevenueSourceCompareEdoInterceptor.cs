using System.Linq;
using Bars.B4;
using Bars.GkhEdoInteg.Entities;

namespace Bars.GkhEdoInteg.Interceptors
{
    public class RevenueSourceCompareEdoInterceptor : EmptyDomainInterceptor<RevenueSourceCompareEdo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RevenueSourceCompareEdo> service, RevenueSourceCompareEdo entity)
        {
            return this.CheckToUnique(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RevenueSourceCompareEdo> service, RevenueSourceCompareEdo entity)
        {
            return this.CheckToUnique(service, entity);
        }

        private IDataResult CheckToUnique(IDomainService<RevenueSourceCompareEdo> service, RevenueSourceCompareEdo entity)
        {
            if (entity.CodeEdo > 0 && service.GetAll().Any(x => x.CodeEdo == entity.CodeEdo && x.Id != entity.Id))
            {
                return Failure("Существует запись с таким кодом ЭДО");
            }

            return new BaseDataResult();
        }
    }
}