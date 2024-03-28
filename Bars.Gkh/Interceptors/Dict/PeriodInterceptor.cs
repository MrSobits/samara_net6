namespace Bars.Gkh.Interceptors.Dict
{
    using System;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities.Dicts;

    public class PeriodInterceptor : EmptyDomainInterceptor<Period>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Period> service, Period entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Period> service, Period entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(Period entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Наименование");
            }

            if (entity.DateStart == default (DateTime))
            {
                return Failure("Не заполнены обязательные поля: Дата начала");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
