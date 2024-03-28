namespace Bars.Gkh.Interceptors
{
    using B4;
    using B4.Utils;
    using Entities;

    class WorkKindCurrentRepairInterceptor : EmptyDomainInterceptor<WorkKindCurrentRepair>
    {
        public override IDataResult BeforeCreateAction(IDomainService<WorkKindCurrentRepair> service, WorkKindCurrentRepair entity)
        {
            return ValidateEntiy(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<WorkKindCurrentRepair> service, WorkKindCurrentRepair entity)
        {
            return ValidateEntiy(entity);
        }

        private IDataResult ValidateEntiy(WorkKindCurrentRepair entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
