namespace Bars.Gkh.Regions.Nnovgorod.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class MunicipalityServiceInterceptor : Gkh.Interceptors.MunicipalityServiceInterceptor
    {
        public override IDataResult BeforeCreateAction(IDomainService<Municipality> service, Municipality entity)
        {
            // закоммитил для импорта ОКТМО
            //if (service.GetAll().Any(x => x.Id != entity.Id && x.FiasId == entity.FiasId))
            //{
            //    return Failure("Невозможно добавить муниципальное образование т.к. такое муниципальное образование уже существует");
            //}

            ///*
            // * Хак - если мы ДЕЙСТВИТЕЛЬНО хотим добавить такое МО, то проверяем "пароль"
            // */
            //if (entity.Description != "является отдельным МО")
            //{
            //    return base.BeforeCreateAction(service, entity);
            //}

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Municipality> service, Municipality entity)
        {
            //if (service.GetAll().Any(x => x.Id != entity.Id && x.FiasId == entity.FiasId))
            //{
            //    return Failure("Невозможно добавить муниципальное образование т.к. такое муниципальное образование уже существует");
            //}

            ///*
            // * Хак - если мы ДЕЙСТВИТЕЛЬНО хотим добавить такое МО, то проверяем "пароль"
            // */
            //if (entity.Description != "является отдельным МО")
            //{
            //    return base.BeforeUpdateAction(service, entity);
            //}

            return Success();
        }
    }
}
