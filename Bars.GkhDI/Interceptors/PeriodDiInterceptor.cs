namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;

    public class PeriodDiInterceptor : EmptyDomainInterceptor<PeriodDi>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PeriodDi> service, PeriodDi entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PeriodDi> service, PeriodDi entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PeriodDi> service, PeriodDi entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<DisclosureInfo>>().GetAll().Any(x => x.PeriodDi.Id == id) ? "Раскрытие информации" : null,
                                  id => this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>().GetAll().Any(x => x.PeriodDi.Id == id) ? "Объект управления раскрытия информации" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return this.Success();
        }

        private IDataResult CheckForm(PeriodDi entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            var emptyFields = GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(PeriodDi entity)
        {
            List<string> fieldList = new List<string>();

            if (entity.DateStart == null)
            {
                fieldList.Add("Дата начала");
            }

            if (entity.DateEnd == null)
            {
                fieldList.Add("Дата окончания");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}

