namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities;

    public class PositionServiceInterceptor : EmptyDomainInterceptor<Position>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Position> service, Position entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Position> service, Position entity)
        {
            return CheckForm(entity);
        }


        public override IDataResult BeforeDeleteAction(IDomainService<Position> service, Position entity)
        {
            if (Container.Resolve<IDomainService<ContragentContact>>().GetAll().Any(x => x.Position.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Контакты контрагента;");
            }

            return Success();
        }

        private IDataResult CheckForm(Position entity)
        {
            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.NameAblative.IsNotEmpty() && entity.NameAblative.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование, Творительный падеж не должно превышать 300 символов");
            }

            if (entity.NameAccusative.IsNotEmpty() && entity.NameAccusative.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование, Винительный падеж не должно превышать 300 символов");
            }

            if (entity.NameDative.IsNotEmpty() && entity.NameDative.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование, Дательный падеж не должно превышать 300 символов");
            }

            if (entity.NameGenitive.IsNotEmpty() && entity.NameGenitive.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование, родительский падеж не должно превышать 300 символов");
            }

            if (entity.NamePrepositional.IsNotEmpty() && entity.NamePrepositional.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование, Предложный падеж не должно превышать 300 символов");
            }

            var emptyFields = GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(Position entity)
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

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}
