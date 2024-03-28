namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Entities.Dict;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Селектор  Программ капитального ремонта (pkr.csv)
    /// </summary>
    public class PkrSelectorService : BaseProxySelectorService<PkrProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PkrProxy> GetAdditionalCache()
        {
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();
            var programVersionRepository = this.Container.ResolveRepository<ProgramVersion>();

            using (this.Container.Using(programCrRepository, programVersionRepository))
            {
                return this.GetProxies(programCrRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds),
                        programVersionRepository.GetAll().WhereContainsBulked(x => x.ExportId, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PkrProxy> GetCache()
        {
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();
            var programVersionRepository = this.Container.ResolveRepository<ProgramVersion>();

            using (this.Container.Using(programCrRepository, programVersionRepository))
            {
                var municipalityIds = this.FilterService.ProgramVersionFilter.Filter.GetAs<long[]>("municipalityId");
                var programCrIdList = this.FilterService.ObjectCrFilter.Filter.GetAs<long[]>("programCrIdList");

                var versionQuery = this.FilterService.ProgramVersionIds.Any()
                    ? programVersionRepository.GetAll()
                        .WhereContainsBulked(x => x.Id, this.FilterService.ProgramVersionIds)
                    : programVersionRepository.GetAll()
                        .WhereIfContainsBulked(municipalityIds.Any(), x => x.Municipality.Id, municipalityIds)
                    .Filter(this.FilterService.ProgramVersionFilter, this.Container);
                var programCrQuery = programCrRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, programCrIdList);

                return this.GetProxies(programCrQuery, versionQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PkrProxy> GetProxies(IQueryable<ProgramCr> programCrQuery, IQueryable<ProgramVersion> programVersionQuery)
        {
            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();
            var basisOverhaulDocConfig = this.Container.GetGkhConfig<BasisOverhaulDocConfig>();
            var contragentRepos = this.Container.ResolveRepository<Contragent>();
            var basisOverhaulDocKindRepos = this.Container.ResolveRepository<BasisOverhaulDocKind>();
            using (this.Container.Using(contragentRepos, basisOverhaulDocKindRepos))
            {
                var startDate = new DateTime(config.ProgrammPeriodStart, 1, 1);
                var endDate = new DateTime(config.ProgrammPeriodEnd, 12, 1);
                var govCustomerId = config.GovCustomer?.Id;
                var executorName = config.Executor?.Name;

                var basisDocId = basisOverhaulDocConfig.Kind?.Id ?? 0;
                var docId = basisDocId > 0
                    ? UniqueIdTool.GetId(1, basisDocId)
                    : default(long?);
                var docType = basisDocId > 0
                    ? basisOverhaulDocKindRepos.GetAll()
                        .Where(x => x.Id == basisDocId)
                        .Select(x => x.Code)
                        .FirstOrDefault()
                    : null;
                var docName = basisOverhaulDocConfig.Name;
                var docNum = basisOverhaulDocConfig.Number;
                var docDate = basisOverhaulDocConfig.Date;
                var acceptedGoverment = basisOverhaulDocConfig.DecisionMaker;
                var docState = basisOverhaulDocConfig.State == DocumentState.Active ? 1 : 2;

                govCustomerId = govCustomerId > 0
                    ? contragentRepos.GetAll().Where(x => x.Id == govCustomerId).Select(x => (long?) x.ExportId).FirstOrDefault()
                    : default(long?);

                var programCrs = programCrQuery.Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Active
                        || x.TypeProgramStateCr == TypeProgramStateCr.New
                        || x.TypeProgramStateCr == TypeProgramStateCr.Open)
                    .Select(x => new
                    {
                        x.ExportId,
                        x.Name,
                        StartDate = x.Period.DateStart,
                        EndDate = x.Period.DateEnd,
                        GovCustomerId = (long?) x.GovCustomer.ExportId,
                        GovCustomerName = x.GovCustomer.Name,
                        x.TypeProgramStateCr,
                        NormativeDocId = (long?) x.NormativeDoc.Id,
                        NormativeDocName = x.NormativeDoc.Name,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.File
                    })
                    .AsEnumerable()
                    .Select(x => new PkrProxy
                    {
                        Id = x.ExportId,
                        Type = 2, // При передаче данных по КПКР передаем значение = 2
                        Level = 1, // всегда 1
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        StateCustomer = x.GovCustomerId,
                        Executor = executorName,
                        State = this.GetState(x.TypeProgramStateCr),
                        DocId = x.NormativeDocId,
                        DocType = "26",
                        DocName = x.NormativeDocName,
                        DocNum = x.DocumentNumber,
                        DocDate = x.DocumentDate,
                        AcceptedGoverment = x.GovCustomerName,
                        DocState = 1,
                        File = x.File
                    });

                return programVersionQuery.Where(x => x.IsMain)
                    .Select(x => new
                    {
                        x.ExportId,
                        x.Name,
                        State = x.State.FinalState ? 2 : 1
                    })
                    .AsEnumerable()
                    .Select(x => new PkrProxy
                    {
                        Id = x.ExportId,
                        Type = 1, // При передаче данных по ДПКР передаем значение = 1
                        Level = 1, // всегда 1
                        Name = x.Name,
                        StartDate = startDate,
                        EndDate = endDate,
                        StateCustomer = govCustomerId,
                        Executor = executorName,
                        State = x.State,

                        DocId = docId,
                        DocType = docType,
                        DocName = docName,
                        DocNum = docNum,
                        DocDate = docDate,
                        AcceptedGoverment = acceptedGoverment,
                        DocState = docState
                    })
                    .Union(programCrs)
                    .ToList();
            }
        }

        private int? GetState(TypeProgramStateCr? state)
        {
            switch (state)
            {
                case TypeProgramStateCr.New:
                case TypeProgramStateCr.Open:
                case TypeProgramStateCr.Active:
                    return 1;
                default:
                    return 2;
            }
        }
    }
}