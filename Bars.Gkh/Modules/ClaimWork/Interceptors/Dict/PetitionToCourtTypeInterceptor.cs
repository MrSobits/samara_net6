namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    public class PetitionToCourtTypeInterceptor : EmptyDomainInterceptor<PetitionToCourtType>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PetitionToCourtType> service, PetitionToCourtType entity)
        {
            return CheckPetitionForm(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PetitionToCourtType> service, PetitionToCourtType entity)
        {
            return CheckPetitionForm(service, entity);
        }

        private IDataResult CheckPetitionForm(IDomainService<PetitionToCourtType> service, PetitionToCourtType entity)
        {
            if (entity.Code.IsNotEmpty() && entity.Code.Length > 100)
            {
                return Failure("Количество знаков в поле Код не должно превышать 100 символов");
            }

            if (entity.ShortName.IsNotEmpty() && entity.ShortName.Length > 500)
            {
                return Failure("Количество знаков в поле Короткое наименование не должно превышать 500 символов");
            }

            if (entity.FullName.IsNotEmpty() && entity.FullName.Length > 3000)
            {
                return Failure("Количество знаков в поле Полное наименование не должно превышать 3000 символов");
            }

            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id))
            {
                return Failure("Поле Код должно быть уникальным.");
            }

            return Success();
        }
    }
}
