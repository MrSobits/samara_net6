namespace Bars.GkhCr.Interceptors.Dicts
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class FinanceSourceInterceptor: EmptyDomainInterceptor<FinanceSource>
    {
        public override IDataResult BeforeCreateAction(IDomainService<FinanceSource> service, FinanceSource entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FinanceSource> service, FinanceSource entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(FinanceSource entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Наименование");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 200)
            {
                return Failure("Количество знаков в поле Код не должно превышать 200 символов");
            }

            return Success();
        }
    }
}
