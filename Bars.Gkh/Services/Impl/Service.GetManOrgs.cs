namespace Bars.Gkh.Services.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.ManagementOrganizationSearch;

    public partial class Service
    {
        public GetManOrgsResponse GetManOrgs(string munId)
        {
            var moMunicipality = Container.Resolve<IDomainService<ManagingOrgMunicipality>>()
                .GetAll()
                .Where(x => x.Municipality.FiasId == munId || x.Municipality.ParentMo != null && x.Municipality.ParentMo.FiasId == munId)
                .Select(x => new ManagementOrganization
                    {
                        ContractorName = x.ManOrg.Contragent.Name,
                        Id = x.ManOrg.Id,
                        MunicipalUnion = x.ManOrg.Contragent.FiasJuridicalAddress.PlaceName,
                        Name = x.ManOrg.Contragent.Name,
                        Address = x.ManOrg.Contragent.FiasJuridicalAddress.AddressName,
                        Status = this.getStatus(x.ManOrg.Contragent.ContragentState)
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

            if (moMunicipality.Count == 0)
            {
                var mo =
                    Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                        .Where(x => x.Contragent.Municipality.FiasId == munId 
                            || x.Contragent.Municipality.ParentMo != null && x.Contragent.Municipality.ParentMo.FiasId == munId)
                        .Select(x => new ManagementOrganization
                        {
                            ContractorName = x.Contragent.Name,
                            Id = x.Id,
                            MunicipalUnion = x.Contragent.FiasJuridicalAddress.PlaceName,
                            Name = x.Contragent.Name,
                            Address = x.Contragent.FiasJuridicalAddress.AddressName,
                            Status = this.getStatus(x.Contragent.ContragentState)
                        })
                        .OrderBy(x => x.Name)
                        .ToArray();

                moMunicipality.AddRange(mo);
            }

            var result = moMunicipality.Count == 0 ? Result.DataNotFound : Result.NoErrors;
            return new GetManOrgsResponse { ManagementOrganizations = moMunicipality.ToArray(), Result = result };
        }

        private string getStatus(ContragentState state)
        {
            switch (state)
            {
                case ContragentState.Active:
                    return "Действует";
                case ContragentState.Bankrupt:
                    return "Банкрот";
                case ContragentState.Liquidated:
                    return "Ликвидирован";
                case ContragentState.NotManagementService:
                    return "Не предоставляет услуги управления";
                default:
                    return "";
            }
        }
    }
}