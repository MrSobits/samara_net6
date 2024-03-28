namespace Bars.GkhGji.Interceptors
{
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    public class PlanInsCheckGjiInterceptor : EmptyDomainInterceptor<PlanInsCheckGji>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PlanInsCheckGji> service, PlanInsCheckGji entity)
        {
            return CheckPlanInsCheckGjiForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PlanInsCheckGji> service, PlanInsCheckGji entity)
        {
            return CheckPlanInsCheckGjiForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PlanInsCheckGji> service, PlanInsCheckGji entity)
        {
            if (Container.Resolve<IDomainService<BaseInsCheck>>().GetAll().Any(x => x.Plan.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Инспекционная проверка;");
            }

            return this.Success();
        }

        private IDataResult CheckPlanInsCheckGjiForm(PlanInsCheckGji entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            var emptyFields = GetEmptyFields(ref entity);
            if (emptyFields.IsNotEmpty())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            return Success();
        }

        private string GetEmptyFields(ref PlanInsCheckGji entity)
        {
            List<string> fieldList = new List<string>();
            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.DateApproval == null)
            {
                fieldList.Add("Дата утверждения");
            }

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