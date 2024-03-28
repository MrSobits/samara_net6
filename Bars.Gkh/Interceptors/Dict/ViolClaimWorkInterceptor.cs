namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities.Dicts;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    public class ViolClaimWorkInterceptor : EmptyDomainInterceptor<ViolClaimWork>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ViolClaimWork> service, ViolClaimWork entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ViolClaimWork> service, ViolClaimWork entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(ViolClaimWork entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 1000)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 1000 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 300)
            {
                return Failure("Количество знаков в поле Код не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
