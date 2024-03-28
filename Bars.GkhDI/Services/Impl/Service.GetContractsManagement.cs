namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetContractsManagementResponse GetContractsManagement(string manOrgId, string periodId)
        {
            var idManOrg = manOrgId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idManOrg != 0 && idPeriod != 0)
            {
                var disclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>()
             .GetAll().FirstOrDefault(x => x.PeriodDi.Id == idPeriod && x.ManagingOrganization.Id == idManOrg);

                if (disclosureInfo == null)
                {
                    return new GetContractsManagementResponse { Result = Result.DataNotFound };
                }

                var document = this.Container.Resolve<IDomainService<Documents>>()
                                        .GetAll().FirstOrDefault(x => x.DisclosureInfo.ManagingOrganization.Id == idManOrg
                                                                      && x.DisclosureInfo.PeriodDi.Id == idPeriod);

                var projectContractDoc = document != null && document.FileProjectContract != null ?
                    new DocumentProxy { IdFile = document.FileProjectContract.Id, NameFile = document.FileProjectContract.FullName } : null;
                var communalServiceDoc = document != null && document.FileCommunalService != null ?
                    new DocumentProxy { IdFile = document.FileCommunalService.Id, NameFile = document.FileCommunalService.FullName } : null;
                var serviceApartmentDoc = document != null && document.FileServiceApartment != null ?
                    new DocumentProxy { IdFile = document.FileServiceApartment.Id, NameFile = document.FileServiceApartment.FullName } : null;


                return new GetContractsManagementResponse
                    {
                        ProjectContractDoc = projectContractDoc,
                        CommunalServiceDoc = communalServiceDoc,
                        ServiceApartmentDoc = serviceApartmentDoc,
                        Result = Result.NoErrors
                    };
            }

            return new GetContractsManagementResponse { Result = Result.DataNotFound };
        }
    }
}
