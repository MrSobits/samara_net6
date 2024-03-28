namespace Bars.Gkh.Regions.Tula.Overrides
{
    using System.Linq;
    using B4;
    using Entities;
    using Interceptors;

    public class MunicipalityInterceptor : MunicipalityServiceInterceptor
    {
        public override IDataResult BeforeCreateAction(IDomainService<Municipality> service, Municipality entity)
        {
            if (service.GetAll().Any(x => x.Id != entity.Id && x.FiasId == entity.FiasId))
            {
                return Failure("Невозможно добавить муниципальное образование т.к. такое муниципальное образование уже существует");
            }

            /*
             * Хак - если мы ДЕЙСТВИТЕЛЬНО хотим добавить такое МО, то проверяем "пароль"
             */
            if (entity.Description != "является отдельным МО")
            {
                return base.BeforeCreateAction(service, entity);
            }

            return Success();
        }
    }
}