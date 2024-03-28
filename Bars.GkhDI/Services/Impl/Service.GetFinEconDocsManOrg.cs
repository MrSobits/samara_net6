namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetFinEconDocsManOrgResponse GetFinEconDocsManOrg(string manOrgId, string periodId)
        {
            var idManOrg = manOrgId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idManOrg != 0 && idPeriod != 0)
            {
                var disclosureInfo = Container.Resolve<IDomainService<DisclosureInfo>>()
                             .GetAll().FirstOrDefault(x => x.PeriodDi.Id == idPeriod && x.ManagingOrganization.Id == idManOrg);
                
                if (disclosureInfo == null)
                {
                    return new GetFinEconDocsManOrgResponse { DocFeAct = null, Result = Result.DataNotFound };
                }

                var finActivity = this.Container.Resolve<IDomainService<FinActivity>>().GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id);
	            if (finActivity == null)
	            {
					return new GetFinEconDocsManOrgResponse { DocFeAct = null, Result = Result.DataNotFound };
	            }

                var finActivityAuditYears = Container.Resolve<IDomainService<FinActivityAudit>>()
                             .GetAll()
                             .Where(x => x.ManagingOrganization.Id == finActivity.DisclosureInfo.ManagingOrganization.Id && x.File != null)
                             .GroupBy(x => x.Year)
                             .ToDictionary(
                                 x => x.Key,
                                 y => {
                                       var document = y.FirstOrDefault();
                                       return document != null ? new DocumentProxy { IdFile = document.File.Id, NameFile = document.File.FullName } : null;
                                   });

                var finActivityDocsByYear = Container.Resolve<IDomainService<FinActivityDocByYear>>()
                                     .GetAll()
                                     .Where(x => x.ManagingOrganization.Id == finActivity.DisclosureInfo.ManagingOrganization.Id && x.File != null)
                                     .Select(x => new
                                                      {
                                                          x.TypeDocByYearDi,
                                                          x.File,
                                                          x.Year
                                                      })
                                     .ToList();

                var finActivityDocs = this.Container.Resolve<IDomainService<FinActivityDocs>>().GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id);

                var periodYear = 0;
                if (disclosureInfo.PeriodDi.DateStart != null)
                {
                    periodYear = disclosureInfo.PeriodDi.DateStart.Value.Year;
                }

                var result = new DocFeAct();

                result.AccReportThisYear = finActivityDocsByYear.Where(x => x.Year == periodYear && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();
                result.AccReportTwoYearsAgo = finActivityDocsByYear.Where(x => x.Year == periodYear - 2 && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();
                result.AccReportPastYear = finActivityDocsByYear.Where(x => x.Year == periodYear - 1 && (x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();
                result.ReportInExPastYear = finActivityDocsByYear.Where(x => x.Year == periodYear - 1 && (x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();
                result.InExPastYear = finActivityDocsByYear.Where(x => x.Year == periodYear - 1 && (x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();
                result.InExThisYear = finActivityDocsByYear.Where(x => x.Year == periodYear && (x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome))
                                         .Select(x => new DocumentProxy { IdFile = x.File.Id, NameFile = x.File.FullName })
                                         .FirstOrDefault();

                result.AuditThisYear = finActivityAuditYears.ContainsKey(periodYear) ? finActivityAuditYears[periodYear] : null;
                result.AuditPastYear = finActivityAuditYears.ContainsKey(periodYear - 1) ? finActivityAuditYears[periodYear - 1] : null;
                result.AuditTwoYearsAgo = finActivityAuditYears.ContainsKey(periodYear - 2) ? finActivityAuditYears[periodYear - 2] : null;

                if (finActivityDocs != null)
                {
                    if (finActivityDocs.BookkepingBalance != null)
                    {
                        result.FinBalance = new DocumentProxy { IdFile = finActivityDocs.BookkepingBalance.Id, NameFile = finActivityDocs.BookkepingBalance.FullName };
                    }

                    if (finActivityDocs.BookkepingBalanceAnnex != null)
                    {
                        result.AppAccBalance = new DocumentProxy { IdFile = finActivityDocs.BookkepingBalanceAnnex.Id, NameFile = finActivityDocs.BookkepingBalanceAnnex.FullName };
                    }
                }

                return new GetFinEconDocsManOrgResponse { DocFeAct = result, Result = Result.NoErrors };
            }

            return new GetFinEconDocsManOrgResponse { DocFeAct = null, Result = Result.DataNotFound };
        }
    }
}
