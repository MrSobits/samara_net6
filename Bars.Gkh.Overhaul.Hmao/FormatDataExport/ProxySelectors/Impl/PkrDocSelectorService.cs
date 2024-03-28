namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Селектор Документы ПКР (pkrdoc.csv)
    /// </summary>
    public class PkrDocSelectorService : BaseProxySelectorService<PkrDocProxy>
    {
        protected override IDictionary<long, PkrDocProxy> GetCache()
        {
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();
            var dpkrDocument = this.Container.ResolveRepository<DpkrDocument>();

            using (this.Container.Using(programCrRepository, dpkrDocument))
            {
                var objectCr = this.FilterService.GetFiltredQuery<ObjectCr>()
                    .Select(x => x.ProgramCr.Id).ToList();

                var programCrQuery = programCrRepository.GetAll()
                    .Where(x => objectCr.Contains(x.Id));

                var dpkrDocumentsQuery = dpkrDocument.GetAll();

                return this.GetProxies(programCrQuery, dpkrDocumentsQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PkrDocProxy> GetProxies(IQueryable<ProgramCr> programCrQuery, IQueryable<DpkrDocument> dpkrDocuments)
        {
            var programCrs = programCrQuery.Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Active
                    || x.TypeProgramStateCr == TypeProgramStateCr.New
                    || x.TypeProgramStateCr == TypeProgramStateCr.Open)
                .Where(w => w.NormativeDoc != null)
                .Select(x => new
                {
                    x.ExportId,
                    GovCustomerName = x.GovCustomer.Name,
                    NormativeDocId = x.NormativeDoc.Id,
                    NormativeDocName = x.NormativeDoc.Name,
                    x.DocumentNumber,
                    x.DocumentDate,
                    x.File
                })
                .AsEnumerable()
                .Select(x => new PkrDocProxy
                {
                    PkrId = x.ExportId,
                    Id = x.NormativeDocId,
                    DocType = "26",
                    DocName = x.NormativeDocName,
                    DocNum = x.DocumentNumber,
                    DocDate = x.DocumentDate,
                    AcceptedGoverment = x.GovCustomerName,
                    DocState = 1,
                    File = x.File
                });

            return dpkrDocuments
                .Where(x => x.State.StartState || x.State.FinalState)
                .AsEnumerable()
                .Select(x => new PkrDocProxy
                {
                    Id = x.GetId(),
                    PkrId = 1,
                    DocType = x.DocumentKind.GetId().ToString(),
                    DocName = x.DocumentName,
                    DocNum = x.DocumentNumber,
                    DocDate = x.DocumentDate,
                    AcceptedGoverment = x.DocumentDepartment,
                    DocState = x.State.StartState ? 1 : 2,
                    File = x.File
                })
                .Union(programCrs)
                .ToList();
        }
    }
}