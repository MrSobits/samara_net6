namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    /// <summary>
    /// Пустншка на тот случай если от этого класса наследовались
    /// </summary>
    public class LocalGovServiceInterceptor : LocalGovServiceInterceptor<LocalGovernment>
    {
        //Внимание!!! все override и ноые методы делать в Generic классе
    }
    
    /// <summary>
    /// Generic класс для того чтобы лучше работать и расширять сущность LocalGovernment
    /// </summary>
    public class LocalGovServiceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : LocalGovernment
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            if (service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id))
            {
                return Failure("Орган местного самоуправления с таким контрагентом уже создан");
            }

            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            return BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var localGovMunicipalityService = Container.Resolve<IDomainService<LocalGovernmentMunicipality>>();
            var localGovWorkModeService = Container.Resolve<IDomainService<LocalGovernmentWorkMode>>();

            try
            {
                var localGovMunicipalityList = localGovMunicipalityService.GetAll().Where(x => x.LocalGovernment.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in localGovMunicipalityList)
                {
                    localGovMunicipalityService.Delete(value);
                }

                var localGovWorkModeList = localGovWorkModeService.GetAll().Where(x => x.LocalGovernment.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in localGovWorkModeList)
                {
                    localGovWorkModeService.Delete(value);
                }

                return this.Success();
            }
            finally 
            {
                Container.Release(localGovMunicipalityService);
                Container.Release(localGovWorkModeService);
            }
        }
    }
}