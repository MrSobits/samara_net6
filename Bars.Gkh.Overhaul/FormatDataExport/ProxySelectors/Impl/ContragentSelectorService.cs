namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="ContragentProxy"/>
    /// </summary>
    public class ContragentSelectorService : BaseProxySelectorService<ContragentProxy>
    {
        /// <inheritdoc />
        protected override ICollection<ContragentProxy> GetAdditionalCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();

            using (this.Container.Using(contragentRepository, contragentContactRepository))
            {
                var query = contragentRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds);
                var contactQuery = contragentContactRepository.GetAll()
                    .WhereContainsBulked(x => x.Contragent.ExportId, this.AdditionalIds);

                return this.GetProxies(query, contactQuery).ToList();
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, ContragentProxy> GetCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();

            using (this.Container.Using(contragentRepository, contragentContactRepository))
            {
                var query = this.FilterService.FilterByContragent(contragentRepository.GetAll());
                var contactQuery = this.FilterService
                    .FilterByContragent(contragentContactRepository.GetAll(), x => x.Contragent);

                return this.GetProxies(query, contactQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected IEnumerable<ContragentProxy> GetProxies(IQueryable<Contragent> contragentQuery, IQueryable<ContragentContact> contactQuery)
        {
            var creditOrgRepository = this.Container.ResolveRepository<CreditOrg>();
            var leaderId = this.SelectParams.GetAsId("LeaderPositionId");

            using (this.Container.Using(creditOrgRepository))
            {
                var contactList = contactQuery
                    .WhereNotNull(x => x.Contragent)
                    .WhereNotNull(x => x.Position)
                    .Where(x => x.Position.Id == leaderId)
                    .ToList();

                var contactInnKppDict = contactList
                    .GroupBy(x => $"{x.Contragent.Inn}|{x.Contragent.Kpp}")
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var contactContragentDict = contactList
                    .GroupBy(x => x.Contragent.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var bankList = creditOrgRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.AdditionalIds)
                    .Select(x => new
                    {
                        x.ExportId,
                        x.Id,
                        x.Name,
                        x.Address,
                        ActualFiasAddress = x.FiasAddress.HouseGuid,
                        ActualAddress = x.FiasAddress.AddressName,
                        PostFiasAddress = x.FiasMailingAddress.HouseGuid,
                        PostAddress = x.FiasMailingAddress.AddressName,
                        x.Ogrn,
                        x.Inn,
                        x.Kpp,
                        ParentId = (long?)x.Parent.ExportId,
                        x.Bik,
                        x.CorrAccount
                    })
                    .AsEnumerable()
                    .Select(x => new ContragentProxy
                    {
                        Id = x.ExportId,
                        BankId = x.Id,
                        Type = 1,
                        FullName = x.Name,
                        LegalAddress = x.Address,
                        ActualFiasAddress = x.ActualFiasAddress.ToStr(),
                        ActualAddress = x.ActualAddress,
                        PostFiasAddress = x.PostFiasAddress.ToStr(),
                        PostAddress = x.PostAddress,
                        Ogrn = x.Ogrn,
                        Inn = x.Inn,
                        Kpp = x.Kpp,
                        ParentId = x.ParentId,
                        Contact = contactInnKppDict.Get($"{x.Inn}|{x.Kpp}"),

                        IsBankContragent = true,
                        Bik = x.Bik,
                        CorrAccount = x.CorrAccount,
                        MainRoleCode = "26" // Кредитная организация/Банк (Bars.Gkh.Migrations._2018.Version_2018082600.UpdateSchema)
                    })
                    .ToList();

                var contragentList = contragentQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.ExportId,
                        OrganizationFormCode = x.OrganizationForm.Code,
                        x.Name,
                        x.ShortName,
                        LegalFiasAddress = x.FiasJuridicalAddress.HouseGuid,
                        LegalAddress = x.FiasJuridicalAddress.AddressName,
                        ActualFiasAddress = x.FiasFactAddress.HouseGuid,
                        ActualAddress = x.FiasFactAddress.AddressName,
                        PostFiasAddress = x.FiasMailingAddress.HouseGuid,
                        PostAddress = x.FiasMailingAddress.AddressName,
                        x.Ogrn,
                        x.Inn,
                        x.Kpp,
                        x.OgrnRegistration,
                        x.DateRegistration,
                        x.OrganizationForm.OkopfCode,
                        x.DateTermination,
                        IsActive = x.ContragentState == ContragentState.Active,
                        x.OfficialWebsite,
                        x.Phone,
                        x.Fax,
                        x.Email,
                        ParentId = (long?)x.Parent.ExportId,
                        x.Oktmo,
                        x.TypeEntrepreneurship,
                        MainRoleCode = x.MainRole.Code
                    })
                    .AsEnumerable()
                    .Select(x => new ContragentProxy
                    {
                        Id = x.ExportId,
                        ContragentId = x.Id,
                        Type = this.GetContragentType(x.OrganizationFormCode),
                        FullName = x.Name,
                        ShortName = x.ShortName,
                        LegalFiasAddress = x.LegalFiasAddress.ToStr(),
                        LegalAddress = x.LegalAddress,
                        ActualFiasAddress = x.ActualFiasAddress.ToStr(),
                        ActualAddress = x.ActualAddress,
                        PostFiasAddress = x.PostFiasAddress.ToStr(),
                        PostAddress = x.PostAddress,
                        Ogrn = x.Ogrn,
                        Inn = x.Inn,
                        Kpp = x.Kpp,
                        Registrator = x.OgrnRegistration,
                        RegistrationDate = x.DateRegistration,
                        Okopf = x.OkopfCode,
                        TerminationDate = x.DateTermination,
                        IsActive = x.IsActive,
                        WebSite = x.OfficialWebsite,
                        Contact = contactContragentDict.Get(x.Id),
                        Phone = x.Phone,
                        Fax = x.Fax,
                        Email = x.Email,
                        ParentId = x.ParentId,
                        IsSmallBusiness = x.TypeEntrepreneurship == TypeEntrepreneurship.Small ? 1 : 2,
                        Oktmo = x.Oktmo,
                        MainRoleCode = x.MainRoleCode
                    })
                    .ToList();

                return contragentList
                    .Union(bankList, this.Comparer);
            }
        }

        protected int GetContragentType(string organizationFormCode)
        {
            switch (organizationFormCode.ToInt(0))
            {
                case 90:
                    return 2;
                case 91:
                    return 3;
                default:
                    return 1;
            }
        }
    }
}