namespace Bars.GkhGji.Interceptors
{

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Utils;

    public class BaseProsClaimServiceInterceptor : BaseProsClaimServiceInterceptor<BaseProsClaim>
    {
    }

    public class BaseProsClaimServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseProsClaim
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            Utils.CreateInspectionNumber(Container, entity, entity.ProsClaimDateCheck.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }
    }
}
