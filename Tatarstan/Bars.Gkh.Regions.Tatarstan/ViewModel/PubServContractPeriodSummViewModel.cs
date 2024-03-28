namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.States;

    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление <see cref="PubServContractPeriodSumm"/>
    /// </summary>
    public class PubServContractPeriodSummViewModel : BaseViewModel<PubServContractPeriodSumm>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }


        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PubServContractPeriodSumm> domainService, BaseParams baseParams)
        {
            // TODO: Добавить фильтрацию по статусам

            var loadParam = baseParams.GetLoadParam();
            var contragentIds = this.UserManager.GetContragentIds();

            var periodId = baseParams.Params.GetAs<long>("period");
            var serviceIds = baseParams.Params.GetAs("service", new List<long>());
            var municipalityIds = baseParams.Params.GetAs("municipality", new List<long>());
            var manOrgIds = baseParams.Params.GetAs("manOrg", new List<long>());
            var pubServOrgIds = baseParams.Params.GetAs("pubServOrg", new List<long>());

            var query = domainService.GetAll()
                .Where(x => x.ContractPeriod.Id == periodId)
                .WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.ContractPeriodSummRso.PublicServiceOrg.Contragent.Id))
                .WhereIfContains(serviceIds.IsNotEmpty(), x => x.PublicService.Service.Id, serviceIds)
                .WhereIfContains(municipalityIds.IsNotEmpty(), x => x.Municipality.Id, municipalityIds)
                .WhereIfContains(manOrgIds.IsNotEmpty(), x => x.ContractPeriodSummUo.ManagingOrganization.Id, manOrgIds)
                .WhereIfContains(pubServOrgIds.IsNotEmpty(), x => x.ContractPeriodSummRso.PublicServiceOrg.Id, pubServOrgIds)
                .Select(
                    x => new ContractPeriodSummProxy
                    {
                        Id = x.Id,
                        Municipality = x.Municipality.Name,
                        Service = x.PublicService.Service.Name,

                        RsoState = x.ContractPeriodSummRso.State,
                        ChargedManOrg = x.ContractPeriodSummRso.ChargedManOrg,
                        PaidManOrg = x.ContractPeriodSummRso.PaidManOrg,
                        SaldoOut = x.ContractPeriodSummRso.SaldoOut,
                        PublicServiceOrg = x.ContractPeriodSummRso.PublicServiceOrg.Contragent.Name,

                        UoState = x.ContractPeriodSummUo.State,
                        ManagingOrganization = x.ContractPeriodSummUo.ManagingOrganization.Contragent.Name,
                        StartDebt = x.ContractPeriodSummUo.StartDebt,
                        ChargedResidents = x.ContractPeriodSummUo.ChargedResidents,
                        RecalcPrevPeriod = x.ContractPeriodSummUo.RecalcPrevPeriod,
                        ChangeSum = x.ContractPeriodSummUo.ChangeSum,
                        NoDeliverySum = x.ContractPeriodSummUo.NoDeliverySum,
                        PaidResidents = x.ContractPeriodSummUo.PaidResidents,
                        EndDebt = x.ContractPeriodSummUo.EndDebt,
                        ChargedToPay = x.ContractPeriodSummUo.ChargedToPay,
                        TransferredPubServOrg = x.ContractPeriodSummUo.TransferredPubServOrg
                    })
                    .Order(loadParam);

            var count = query.Count();

            return new ListDataResult(query.Paging(loadParam).ToList(), count);
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<PubServContractPeriodSumm> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var periodSumm = domainService.Get(id);

            var proxy = new ContractPeriodSummProxy
            {
                Id = periodSumm.Id,
                Period = periodSumm.ContractPeriod.Name,
                Service = periodSumm.PublicService.Service.Name,
                ChargedResidents = periodSumm.ContractPeriodSummUo.ChargedResidents,
                Municipality = periodSumm.Municipality.Name,
                PaidResidents = periodSumm.ContractPeriodSummUo.PaidResidents,
                TransferredPubServOrg = periodSumm.ContractPeriodSummUo.TransferredPubServOrg,
                ManagingOrganization = periodSumm.ContractPeriodSummUo.ManagingOrganization.Contragent.Name,

                ChargedManOrg = periodSumm.ContractPeriodSummRso.ChargedManOrg,
                PaidManOrg = periodSumm.ContractPeriodSummRso.PaidManOrg,
                PublicServiceOrg = periodSumm.ContractPeriodSummRso.PublicServiceOrg.Contragent.Name,

                UoSummId = periodSumm.ContractPeriodSummUo.Id,
                UoState = periodSumm.ContractPeriodSummUo.State,
                RsoSummId = periodSumm.ContractPeriodSummRso.Id,
                RsoState = periodSumm.ContractPeriodSummRso.State
            };

            return new BaseDataResult(proxy);
        }

        protected class ContractPeriodSummProxy
        {
            public long Id { get; set; }

            public string Period { get; set; }

            public string Municipality { get; set; }

            public string Service { get; set; }

            public string ManagingOrganization { get; set; }

            public decimal ChargedManOrg { get; set; }

            public decimal PaidManOrg { get; set; }

            public decimal SaldoOut { get; set; }

            public string PublicServiceOrg { get; set; }

            public decimal BilledPubServOrg { get; set; }

            public decimal PaidPubServOrg { get; set; }

            public decimal StartDebt { get; set; }

            public decimal ChargedResidents { get; set; }

            public decimal RecalcPrevPeriod { get; set; }

            public decimal ChangeSum { get; set; }

            public decimal NoDeliverySum { get; set; }

            public decimal PaidResidents { get; set; }

            public decimal EndDebt { get; set; }

            public decimal ChargedToPay { get; set; }

            public decimal TransferredPubServOrg { get; set; }

            public State RsoState { get; set; }

            public State UoState { get; set; }

            public long UoSummId { get; set; }

            public long RsoSummId { get; set; }
        }
    }
}
