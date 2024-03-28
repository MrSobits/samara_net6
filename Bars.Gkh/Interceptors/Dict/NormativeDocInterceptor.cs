namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts;
    using Bars.B4;
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    public class NormativeDocInterceptor : EmptyDomainInterceptor<NormativeDoc>
    {
        public override IDataResult BeforeCreateAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            if (string.IsNullOrEmpty(entity.FullName))
            {
                entity.FullName = entity.Name;
            }

            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            if (string.IsNullOrEmpty(entity.FullName))
            {
                entity.FullName = entity.Name;
            }

            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<NormativeDoc> service, NormativeDoc entity)
        {
            // Внимание если в дальнешем будет принято решение что нормативный документ можно удалять 
            // Неважно уществуют ли нарушения по нему или нет , то тогданужн оперегенерить поле NormativeDocNames в таблице ViolationGji
            var npdDomain = this.Container.Resolve<IDomainService<NormativeDocItem>>();

            using (this.Container.Using(npdDomain))
            {
                return npdDomain.GetAll().Any(x => x.NormativeDoc.Id == entity.Id)
                    ? this.Failure("Данный нормативно - правовой документ содержит пункты")
                    : this.Success();
            }
        }

        private IDataResult CheckForm(NormativeDoc entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.FullName.IsNotEmpty() && entity.FullName.Length > 1000)
            {
                return Failure("Количество знаков в поле Полное наименование не должно превышать 1000 символов");
            }

            var emptyFields = GetEmptyFields(entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(NormativeDoc entity)
        {
            List<string> fieldList = new List<string>();
            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.FullName.IsEmpty())
            {
                fieldList.Add("Полное наименование");
            }

            if (entity.Code == default(int))
            {
                fieldList.Add("Код");
            }

            return fieldList.AggregateWithSeparator(", ");
        }
    }
}
