namespace Bars.Gkh1468.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;
    using Bars.Gkh.Modules.Gkh1468.Entities.Dict;
    using Bars. Gkh.Modules.Gkh1468.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Вьюха для договора с поставщиком ресурсов
    /// </summary>
    public class RealObjPublicServiceOrgViewModel : BaseViewModel<PublicServiceOrgContract>
    {
        /// <summary>
        /// Сторона договора
        /// </summary>
        public IDomainService<BaseContractPart> BaseContractPartDomain { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domain">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PublicServiceOrgContract> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");
            var publicServOrgId = baseParams.Params.GetAs<long>("publicServOrgId");
            var fromContract = baseParams.Params.GetAs<bool>("fromContract");

            if (objectId > 0 && !fromContract)
            {
                return new ListDataResult(new List<object>(), 0);
            }

            var data1 = domain.GetAll()
                .WhereIf(publicServOrgId > 0, x => x.PublicServiceOrg.Id == publicServOrgId)
                .Select(x => new
                {
                   x.Id,
                    x.PublicServiceOrg.Contragent.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.ContractNumber,
                    x.ContractDate,
                    x.FileInfo,
                    x.Note,
                    BaseContractPart = this.BaseContractPartDomain.GetAll().FirstOrDefault(y => y.PublicServiceOrgContract.Id == x.Id)
                });

            var totalCount1 = data1.Count();

            var result = data1
                .Order(loadParams)
                .Paging(loadParams)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateStart,
                    x.DateEnd,
                    x.ContractNumber,
                    x.ContractDate,
                    x.FileInfo,
                    x.Note,
                    x.BaseContractPart?.TypeContractPart,
                    PartName = this.GetPartName(x.BaseContractPart)
                })
                .ToList();

            return new ListDataResult(result, totalCount1);
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<PublicServiceOrgContract> domainService, BaseParams baseParams)
        {
            var contract = domainService.Get(baseParams.Params.GetAs<long>("id"));
            var contractPart = this.BaseContractPartDomain.GetAll().FirstOrDefault(x => x.PublicServiceOrgContract.Id == contract.Id);
            var contractProxy = new ContractPartyProxy();

            // т.к. у нас несколько видов договоров, используем рефлексию
            if (contractPart.IsNotNull())
            {
                var contractPartyType = contractPart.GetType();
                var contractProxyType = typeof(ContractPartyProxy);

                contractPartyType.GetProperties(BindingFlags.Public | BindingFlags.Instance).ForEach(
                    x =>
                    {
                        var property = contractProxyType.GetProperty(x.Name);
                        property?.SetValue(contractProxy, x.GetValue(contractPart));
                    });

                contractProxy.TypeOwnerContract = contractPart is JurPersonOwnerContract
                   ? TypeOwnerContract.JurPersonOwnerContact
                   : contractPart is IndividualOwnerContract
                       ? TypeOwnerContract.IndividualOwnerContract
                       : (TypeOwnerContract?)null;
                contractProxy.ContractPartId = contractPart.Id;
            }

            var resultData = new
            {
                contract.Id,
                ResOrgReason = contract.ResOrgReason == 0 ? (ResOrgReason?)null : contract.ResOrgReason,
                PublicServiceOrg = new
                {
                    contract.PublicServiceOrg.Id,
                    ContragentName = contract.PublicServiceOrg.Contragent.Name
                },
                ResourceOrg = new
                {
                    contract.PublicServiceOrg.Id,
                    ContragentName = contract.PublicServiceOrg.Contragent.Name
                },
                contract.DateStart,
                contract.DateEnd,
                contract.ContractNumber,
                contract.ContractDate,
                contract.FileInfo,
                contract.Note,
                contract.TermBillingPaymentNoLaterThan,
                contract.TermPaymentNoLaterThan,
                contract.DeadlineInformationOfDebt,
                contract.DayStart,
                contract.DayEnd,
                contract.EndDeviceMetteringIndication,
                contract.StartDeviceMetteringIndication,
                contract.StopReason,
                contract.DateStop,

                contractProxy.ContractPartId,
                contractProxy.TypeContractPart,
                contractProxy.FirstName,
                contractProxy.LastName,
                contractProxy.MiddleName,
                contractProxy.Gender,
                contractProxy.OwnerDocumentType,
                contractProxy.IssueDate,
                contractProxy.DocumentSeries,
                contractProxy.DocumentNumber,
                contractProxy.BirthPlace,
                contractProxy.Contragent,
                ManagingOrganization = contractProxy.ManagingOrganization == null ? null :
                new
                {
                    contractProxy.ManagingOrganization.Id,
                    ContragentShortName = contractProxy.ManagingOrganization.Contragent.ShortName.IsEmpty() 
                        ? contractProxy.ManagingOrganization.Contragent.Name 
                        : contractProxy.ManagingOrganization.Contragent.ShortName,
                },
                contractProxy.CommercialMeteringResourceType,
                contractProxy.TypeContactPerson,
                contractProxy.BirthDate,
                contractProxy.TypeOwnerContract,
                contractProxy.Organization,
                contractProxy.TypeCustomer,
                FuelEnergyResourceOrg = contractProxy.FuelEnergyResourceOrg == null ? null :
                new
                {
                    contractProxy.FuelEnergyResourceOrg.Id,
                    ContragentName = contractProxy.FuelEnergyResourceOrg.Contragent.Name
                }
            };

            return new BaseDataResult(resultData);
        }

        private string GetPartName(BaseContractPart contract)
        {
            var contractUo = contract as RsoAndServicePerformerContract;
            if (contractUo != null)
            {
                return contractUo.ManagingOrganization.Contragent.Name;
            }

            var contractJurPerson = contract as JurPersonOwnerContract;
            if (contractJurPerson != null)
            {
                return contractJurPerson.Contragent.Name;
            }

            var contractPhysPerson = contract as IndividualOwnerContract;
            if (contractPhysPerson != null)
            {
                return new List<string>
                {
                    contractPhysPerson.LastName,
                    contractPhysPerson.FirstName,
                    contractPhysPerson.MiddleName
                }
                .Where(x => x.IsNotEmpty())
                .AggregateWithSeparator(" ");
            }

            var contractBudget = contract as BudgetOrgContract;
            if (contractBudget != null)
            {
                return contractBudget.TypeCustomer.Name;
            }

            var contractFuelEnergy = contract as FuelEnergyResourceContract;
            if (contractFuelEnergy != null)
            {
                return contractFuelEnergy.FuelEnergyResourceOrg.Contragent.Name;
            }

            return null;
        }

        private class ContractPartyProxy
        {
            public TypeContractPart? TypeContractPart { get; set; }
            public TypeContactPerson? TypeContactPerson { get; set; }
            public TypeOwnerContract? TypeOwnerContract { get; set; }
            public Contragent Contragent { get; set; }
            public ManagingOrganization ManagingOrganization { get; set; }
            public PublicServiceOrg FuelEnergyResourceOrg { get; set; }
            public Contragent Organization { get; set; }
            public TypeCustomer TypeCustomer { get; set; }
            public CommercialMeteringResourceType? CommercialMeteringResourceType { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public GenderR? Gender { get; set; }
            public OwnerDocumentType? OwnerDocumentType { get; set; }
            public DateTime? IssueDate { get; set; }
            public string DocumentSeries { get; set; }
            public string DocumentNumber { get; set; }
            public string BirthPlace { get; set; }
            public DateTime BirthDate { get; set; }
            public long ContractPartId { get; set; }
        }
    }
}