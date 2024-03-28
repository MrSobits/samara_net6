namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    public class ShareFinancingCeoInterceptor : EmptyDomainInterceptor<ShareFinancingCeo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ShareFinancingCeo> service, ShareFinancingCeo entity)
        {
            if (service.GetAll().Any(x => x.CommonEstateObject.Id == entity.CommonEstateObject.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "В справочнике уже существует доля финансирования по данной работе");
            }

            if (service.GetAll().Where(x => x.Id != entity.Id).Select(x => x.Share).AsEnumerable().Sum() + entity.Share > 100)
            {
                return new BaseDataResult(false, "Сумма всех долей должна быть меньше 100%");
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ShareFinancingCeo> service, ShareFinancingCeo entity)
        {
            if (service.GetAll().Any(x => x.CommonEstateObject.Id == entity.CommonEstateObject.Id && x.Id != entity.Id))
            {
                return new BaseDataResult(false, "В справочнике уже существует доля финансирования по данной работе");
            }

            if (service.GetAll().Where(x => x.Id != entity.Id).Select(x => x.Share).AsEnumerable().Sum() + entity.Share > 100)
            {
                return new BaseDataResult(false, "Сумма всех долей должна быть меньше 100%");
            }

            return base.BeforeUpdateAction(service, entity);
        }
    }
}