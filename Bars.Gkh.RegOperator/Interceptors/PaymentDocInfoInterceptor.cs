namespace Bars.Gkh.RegOperator.Interceptors
{
    using B4;

    using Bars.B4.Utils;

    using Entities.Dict;
    using Gkh.Utils;
    using System.Linq;

    public class PaymentDocInfoInterceptor : EmptyDomainInterceptor<PaymentDocInfo>
    {
        private const string Message = "В системе уже существует запись с данными параметрами, попадающими в введенный период!";

        public override IDataResult BeforeCreateAction(IDomainService<PaymentDocInfo> service, PaymentDocInfo entity)
        {
            var query = service.GetAll();
            return this.CheckExisting(query, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PaymentDocInfo> service, PaymentDocInfo entity)
        {
            var query = service.GetAll().Where(x => x.Id != entity.Id);
            return this.CheckExisting(query, entity);
        }

        private IDataResult CheckExisting(IQueryable<PaymentDocInfo> query, PaymentDocInfo entity)
        {
            var entityDateRange = new DateRange(entity.DateStart, entity.DateEnd);
            query = this.BuildQuery(query, entity);

            return query.AsEnumerable().Any(x => new DateRange(x.DateStart, x.DateEnd).Intersect(entityDateRange))
                ? this.Failure(PaymentDocInfoInterceptor.Message)
                : this.Success();
        }

        private IQueryable<PaymentDocInfo> BuildQuery(IQueryable<PaymentDocInfo> query, PaymentDocInfo entity)
        {
            query = query
                    .WhereIf(entity.RealityObject != null, x => x.RealityObject.Id == entity.RealityObject.Id)
                    .WhereIf(entity.RealityObject == null, x => x.RealityObject == null);

            query = query
                    .WhereIf(entity.LocalityAoGuid != null, x => x.LocalityAoGuid == entity.LocalityAoGuid)
                    .WhereIf(entity.LocalityAoGuid == null, x => x.LocalityAoGuid == null);

            query = query
                    .WhereIf(entity.MoSettlement != null, x => x.MoSettlement.Id == entity.MoSettlement.Id)
                    .WhereIf(entity.MoSettlement == null, x => x.MoSettlement == null);

            query = query
                    .WhereIf(entity.Municipality != null, x => x.Municipality.Id == entity.Municipality.Id)
                    .WhereIf(entity.Municipality == null, x => x.Municipality == null);

            query = query
                    .WhereIf(entity.IsForRegion && entity.Municipality != null, x => x.Municipality.Id == entity.Municipality.Id)
                    .WhereIf(entity.Municipality == null, x => x.Municipality == null);
            query = query.Where(x => x.IsForRegion == entity.IsForRegion);

            query = query.Where(x => x.FundFormationType == entity.FundFormationType);

            return query;
        }
    }
}
