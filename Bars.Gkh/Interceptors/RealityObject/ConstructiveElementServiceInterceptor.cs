namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using System.Collections.Generic;
    using Bars.Gkh.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.B4.Utils;

    public class ConstructiveElementServiceInterceptor : EmptyDomainInterceptor<ConstructiveElement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ConstructiveElement> service, ConstructiveElement entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ConstructiveElement> service, ConstructiveElement entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ConstructiveElement> service, ConstructiveElement entity)
        {
            if (Container.Resolve<IDomainService<RealityObjectConstructiveElement>>().GetAll().Any(x => x.ConstructiveElement.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Конструктивные элементы жилого дома;");
            }

            return Success();
        }

        private IDataResult CheckForm(ConstructiveElement entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            var emptyFields = GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(ConstructiveElement entity)
        {
            List<string> fieldList = new List<string>();

            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.Code.IsEmpty())
            {
                fieldList.Add("Код");
            }

            if (entity.Group == null)
            {
                fieldList.Add("Группа");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}
