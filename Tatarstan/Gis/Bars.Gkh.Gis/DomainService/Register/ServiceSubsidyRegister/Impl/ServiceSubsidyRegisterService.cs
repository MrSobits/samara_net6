namespace Bars.Gkh.Gis.DomainService.Register.ServiceSubsidyRegister.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities.PersonalAccount;
    using Entities.Register.ServiceSubsidyRegister;

    public class ServiceSubsidyRegisterService : IServiceSubsidyRegisterService
    {
        protected IWindsorContainer Container;
        protected IRepository<GisPersonalAccount> PersonalAccountRepository;
        protected IRepository<ServiceSubsidyRegister> ServiceSubsidyRepository;

        public ServiceSubsidyRegisterService(IWindsorContainer container, IRepository<GisPersonalAccount> personalAccountRepository
            , IRepository<ServiceSubsidyRegister> serviceSubsidyRepository)
        {
            Container = container;
            PersonalAccountRepository = personalAccountRepository;
            ServiceSubsidyRepository = serviceSubsidyRepository;
        }

        /// <summary>
        /// Список поставщиков
        /// </summary>
        public IDataResult ListByApartmentId(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var apartmentId = baseParams.Params.GetAs<long>("apartmentId");
            var month = baseParams.Params.GetAs<int>("month");
            var year = baseParams.Params.GetAs<int>("year");
            var date = new DateTime(year, month, 1);

            var data = ServiceSubsidyRepository
                .GetAll()
                .Where(x => x.PersonalAccountId == apartmentId
                    && (x.CalculationMonth == default(DateTime) || x.CalculationMonth == date))
                    .Select(x => new
                    {
                        x.Id,
                        x.Pss,
                        Service = x.Service.Name,
                        x.AccruedBenefitSum,
                        x.AccruedEdvSum,
                        x.RecalculatedBenefitSum,
                        x.RecalculatedEdvSum
                    })
                .Order(loadParam)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Skip(loadParam.Start).Take(loadParam.Limit).ToList(), data.Count());
        }
    }
}