namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Contragent;

    using Castle.Windsor;

    using NHibernate.Linq;


    /// <summary>
    /// Сервис для работы с контрагентами
    /// </summary>
    public class ContragentService : IContragentService
    {
        #region DependencyInjection
        private readonly IWindsorContainer container;
        private readonly IDomainService<Contragent> _contragentDomain;

        /// <inheritdoc cref="ContragentService" />
        public ContragentService(IWindsorContainer container, IDomainService<Contragent> contragentDomain)
        {
            this.container = container;
            this._contragentDomain = contragentDomain;
        }
        #endregion

        /// <inheritdoc />
        public async Task<object> GetAsync(long contragentId) =>
            (await this.GetContragentListAsync(contragentIds: new[] { contragentId }))?.FirstOrDefault();

        /// <inheritdoc />
        public async Task<IEnumerable<object>> GetListAsync(bool fullList, long[] contragentIds) =>
            await this.GetContragentListAsync(contragentIds, fullList);

        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="fullList">Признак получения полного списка контрагентов</param>
        /// <param name="contragentIds">Массив идентификаторов контрагентов</param>
        /// <returns></returns>
        private async Task<IEnumerable<object>> GetContragentListAsync(long[] contragentIds, bool fullList = false)
        {
            if (fullList)
            {
                var userManager = this.container.Resolve<IGkhUserManager>();
                var currentUserMoIds = userManager.GetMunicipalityIds();

                return await this._contragentDomain.GetAll()
                    .Where(x => x.ContragentState == ContragentState.Active)
                    .WhereIf(currentUserMoIds.Any(), x => currentUserMoIds.Contains(x.Municipality.Id))
                    .Select(x => new ShortContragentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        Inn = x.Inn,
                        Ogrn = x.Ogrn
                    }).ToListAsync();
            }

            if (!contragentIds?.Any() ?? true) return null;
            
            var contragentContactDomain = this.container.Resolve<IDomainService<ContragentContact>>();
            var manOrgContractRealityObjectDomain = this.container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var manOrgContractOwnersDomain = this.container.Resolve<IDomainService<ManOrgContractOwners>>();

            using (this.container.Using(contragentContactDomain, manOrgContractRealityObjectDomain,
                manOrgContractOwnersDomain))
            {
                var supervisorData = await contragentContactDomain.GetAll()
                    .Where(x => contragentIds.Contains(x.Contragent.Id))
                    .Where(x => x.Position.Code == "1")
                    .Where(x => x.DateEndWork == null || x.DateEndWork > DateTime.Now)
                    .Select(x => new
                    {
                        x.Contragent.Id,
                        x.FullName,
                        x.Phone,
                        x.Email
                    })
                    .ToListAsync();

                var supervisorDict = supervisorData
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y);

                var addressesContractsData = await manOrgContractRealityObjectDomain.GetAll()
                    .Where(x => contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                    .Where(x => x.ManOrgContract != null
                                && x.ManOrgContract.ManagingOrganization != null
                                && x.ManOrgContract.ManagingOrganization.Contragent != null
                                && x.RealityObject != null)
                    .Join(manOrgContractOwnersDomain.GetAll(),
                        x => x.ManOrgContract.Id,
                        y => y.Id,
                        (x, y) => new
                        {
                            ContragentId = x.ManOrgContract.ManagingOrganization.Contragent.Id,
                            AddressId = x.RealityObject.Id,
                            x.RealityObject.Address,
                            x.ManOrgContract.StartDate,
                            x.ManOrgContract.EndDate,
                            y.ContractFoundation,
                            x.ManOrgContract.ContractStopReason
                        })
                    .ToListAsync();

                var addressesContractsDict = addressesContractsData
                    .GroupBy(x => x.ContragentId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        СountActiveHouses = y.Count(z => z.EndDate == null || z.EndDate > DateTime.Now),
                        Addresses = y.Select(z => new AddressDto
                        {
                            AddressId = z.AddressId,
                            Address = z.Address,
                            StartDate = z.StartDate,
                            EndtDate = z.EndDate,
                            Base = z.ContractFoundation.GetDisplayName(),
                            CompletionBasis = z.ContractStopReason.GetDisplayName()
                        }).ToArray()
                    });

                var contragentModels = await this._contragentDomain.GetAll()
                    .Where(x => contragentIds.Contains(x.Id))
                    .Select(x => new FullContragentDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        OrganizationalForm = x.OrganizationForm.Name,
                        Role = x.MainRole.Name,
                        Inn = x.Inn,
                        Kpp = x.Kpp,
                        Ogrn = x.Ogrn,
                        RegistrationDate = x.DateRegistration,
                        LegalAddress = x.FiasJuridicalAddress.AddressName,
                        ActualAddress = x.FactAddress,
                        PostalAddress = x.MailingAddress,
                        Telephone = x.Phone,
                        Mail = x.Email,
                        Fax = x.Fax
                    })
                    .ToListAsync();

                contragentModels.ForEach(x =>
                {
                    if (supervisorDict.TryGetValue(x.Id, out var sv))
                    {
                        x.Supervisor = string.Join(", ", sv.Select(y => y.FullName));
                        x.AdditionalTelephone = string.Join(", ", sv.Select(y => y.Phone));
                        x.AdditionalMail = string.Join(", ", sv.Select(y => y.Email));
                    }

                    x.CountActiveHouses = addressesContractsDict.Get(x.Id)?.СountActiveHouses ?? 0;
                    x.Addresses = addressesContractsDict.Get(x.Id)?.Addresses;
                });

                return contragentModels;
            }
        }
    }
}