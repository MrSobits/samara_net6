namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Entities.Dict;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Селектор Программ капитального ремонта (pkr.csv)
    /// </summary>
    public class PkrSelectorService : BaseProxySelectorService<PkrProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PkrProxy> GetAdditionalCache()
        {
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();

            using (this.Container.Using(programCrRepository))
            {
                return this.GetProxies(programCrRepository.GetAll()
                        .WhereContainsBulked(x => x.ExportId, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, PkrProxy> GetCache()
        {
            var programCrRepository = this.Container.ResolveRepository<ProgramCr>();

            using (this.Container.Using(programCrRepository))
            {
                var programCrIdList = this.FilterService.ObjectCrFilter.Filter.GetAs<long[]>("programCrIdList");
                var programCrQuery = programCrRepository.GetAll()
                    .WhereContainsBulked(x => x.Id, programCrIdList);

                return this.GetProxies(programCrQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<PkrProxy> GetProxies(IQueryable<ProgramCr> programCrQuery)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var contragentRepos = this.Container.ResolveRepository<Contragent>();
            var basisOverhaulDocKindRepos = this.Container.ResolveRepository<BasisOverhaulDocKind>();

            using (this.Container.Using(contragentRepos, basisOverhaulDocKindRepos))
            {
                var startDate = new DateTime(config.ProgrammPeriodStart, 1, 1);
                var endDate = new DateTime(config.ProgrammPeriodEnd, 12, 1);
                var govCustomerId = config.GovCustomer?.Id;
                var executorName = config.Executor?.Name;
                var programmName = config.ProgrammName;

                govCustomerId = govCustomerId > 0
                    ? contragentRepos.GetAll().Where(x => x.Id == govCustomerId).Select(x => (long?) x.ExportId).FirstOrDefault()
                    : default(long?);

                var programCrs = programCrQuery
                    .Where(x => x.TypeProgramStateCr == TypeProgramStateCr.Active
                        || x.TypeProgramStateCr == TypeProgramStateCr.New
                        || x.TypeProgramStateCr == TypeProgramStateCr.Open)
                    .Fetch(x => x.File)
                    .Select(x => new
                    {
                        x.ExportId,
                        x.Name,
                        StartDate = (DateTime?) x.Period.DateStart,
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
                        State = this.GetState(x.TypeProgramStateCr)
                    });

                var dpkr = new[]
                    {
                        new PkrProxy
                        {
                            Id = 1,
                            Type = 1, // При передаче данных по ДПКР передаем значение = 1
                            Level = 1, // всегда 1
                            Name = programmName,
                            StartDate = startDate,
                            EndDate = endDate,
                            StateCustomer = govCustomerId,
                            Executor = executorName,
                            State = 1
                        }
                    }
                    .AsEnumerable();

                if (!(programCrs is null))
                    dpkr = dpkr.Union(programCrs);

                return dpkr.ToList();
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