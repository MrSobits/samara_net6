namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetManContractAppResponse GetManContractApp(string houseId, string periodId)
        {
            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var diRealObj = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                     .GetAll()
                     .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetManContractAppResponse { Result = Result.DataNotFound };
                }

                var documentRo = this.Container.Resolve<IDomainService<DocumentsRealityObj>>()
                                          .GetAll().FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id);

                var manContractApps = new ManContractApps();
                if (documentRo != null)
                {
                    manContractApps = new ManContractApps
                    {
                        ActState = documentRo.FileActState != null ? 
                            new DocumentProxy { IdFile = documentRo.FileActState.Id, NameFile = documentRo.FileActState.FullName } : null,
                        CatalogRepair = documentRo.FileCatalogRepair != null ? 
                            new DocumentProxy { IdFile = documentRo.FileCatalogRepair.Id, NameFile = documentRo.FileCatalogRepair.FullName } : null,
                        ReportPlanRepair = documentRo.FileReportPlanRepair != null ?
                            new DocumentProxy { IdFile = documentRo.FileReportPlanRepair.Id, NameFile = documentRo.FileReportPlanRepair.FullName } : null
                    };
                }

                return new GetManContractAppResponse { ManContractApps = manContractApps, Result = Result.NoErrors };
            }

            return new GetManContractAppResponse { Result = Result.DataNotFound };
        }
    }
}
