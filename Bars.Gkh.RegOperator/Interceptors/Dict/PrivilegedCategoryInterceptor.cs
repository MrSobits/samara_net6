namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    public class PrivilegedCategoryInterceptor : EmptyDomainInterceptor<PrivilegedCategory>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PrivilegedCategory> service, PrivilegedCategory entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code && x.Name != entity.Name && x.Id != entity.Id))
            {
                return Failure("Льготная категория с указанным кодом и другим наименованием уже существует");
            }

            if (service.GetAll().Any(x => x.Name == entity.Name && x.Code != entity.Code && x.Id != entity.Id))
            {
                return Failure("Льготная категория с указанным наименованием и другим кодом уже существует");
            }

            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id &&
                ((!x.DateTo.HasValue && (entity.DateFrom >= x.DateFrom || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom))) 
                    || (x.DateTo.HasValue && ((entity.DateFrom >= x.DateFrom && entity.DateFrom <= x.DateTo) || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom && entity.DateTo <= x.DateTo))))))
            {
                return Failure("Для выбранной категории льгот уже заданы проценты льготы. Пожалуйста выберите другие даты действия значений");
            }

            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PrivilegedCategory> service, PrivilegedCategory entity)
        {
            if (service.GetAll().Any(x => x.Code == entity.Code && x.Name != entity.Name && x.Id != entity.Id))
            {
                return Failure("Льготная категория с указанным кодом и другим наименованием уже существует");
            }

            if (service.GetAll().Any(x => x.Name == entity.Name && x.Code != entity.Code && x.Id != entity.Id))
            {
                return Failure("Льготная категория с указанным наименованием и другим кодом уже существует");
            }

            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id &&
                ((!x.DateTo.HasValue && (entity.DateFrom >= x.DateFrom || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom)))
                    || (x.DateTo.HasValue && ((entity.DateFrom >= x.DateFrom && entity.DateFrom <= x.DateTo) || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom && entity.DateTo <= x.DateTo))))))
            {
                return Failure("Для выбранной категории льгот уже заданы проценты льготы. Пожалуйста выберите другие даты действия значений");
            }

            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PrivilegedCategory> service, PrivilegedCategory entity)
        {
            var ownerService = Container.ResolveDomain<PersonalAccountOwner>();
            var accountPrivilegeService = Container.ResolveDomain<PersonalAccountPrivilegedCategory>();
            try
            {
                if (ownerService.GetAll().Any(x => x.PrivilegedCategory.Id == entity.Id))
                {
                    return Failure("Имеются абоненты с данной категорией льгот");
                }

                if (accountPrivilegeService.GetAll().Any(x => x.PrivilegedCategory.Id == entity.Id))
                {
                    return Failure("Имеются лицевые счета с данной категорией льгот");
                }
            }
            finally
            {
                Container.Release(ownerService);
                Container.Release(accountPrivilegeService);
            }

            return Success();
        }

        private IDataResult CheckForm(PrivilegedCategory entity)
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

        private string GetEmptyFields(PrivilegedCategory entity)
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

            if (!entity.Percent.HasValue)
            {
                fieldList.Add("Процент льготы");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}