namespace Bars.Gkh.Gis.DomainService.Register.TenantSubsidyRegister.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities.PersonalAccount;
    using Entities.Register.TenantSubsidyRegister;

    public class TenantSubsidyRegisterService : ITenantSubsidyRegisterService
    {
        protected IWindsorContainer Container;
        protected IRepository<GisPersonalAccount> PersonalAccountRepository;
        protected IRepository<TenantSubsidyRegister> TenantSubsidyRepository;

        public TenantSubsidyRegisterService(IWindsorContainer container, IRepository<GisPersonalAccount> personalAccountRepository
            , IRepository<TenantSubsidyRegister> tenantSubsidyRepository)
        {
            Container = container;
            PersonalAccountRepository = personalAccountRepository;
            TenantSubsidyRepository = tenantSubsidyRepository;
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

            var data = TenantSubsidyRepository
                .GetAll()
                .Where(x => x.PersonalAccountId == apartmentId
                    && (x.BeginDate == default(DateTime) || x.BeginDate <= date)
                    && (x.EndDate == default(DateTime) || x.EndDate >= date))
                    .Select(x => new
                    {
                        x.Id,
                        x.Pss,
                        x.Surname,
                        x.Name,
                        x.Patronymic,
                        x.DateOfBirth,
                        x.ArticleCode,
                        x.BankName,
                        x.BeginDate,
                        x.IncomingSaldo,
                        x.AccruedSum,
                        x.AdvancedPayment,
                        x.PaymentSum,
                        x.SmoSum,
                        x.ChangesSum,
                        x.EndDate,
                        Service = x.Service.Name
                    })
                .Order(loadParam)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Skip(loadParam.Start).Take(loadParam.Limit).ToList(), data.Count());
        }
    }
}