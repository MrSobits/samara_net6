namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис получения <see cref="DogovorPkrProxy"/>
    /// </summary>
    public class DogovorPkrSelectorService : BaseProxySelectorService<DogovorPkrProxy>
    {
        /// <inheritdoc />
        protected override ICollection<DogovorPkrProxy> GetAdditionalCache()
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContract>();

            using (this.Container.Using(buildContractRepository))
            {
                return this.GetProxies(buildContractRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, DogovorPkrProxy> GetCache()
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContract>();

            using (this.Container.Using(buildContractRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var buildContractQuery = this.FilterService.ObjectCrIds.Any()
                    ? buildContractRepository.GetAll()
                        .WhereContainsBulked(x => x.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : buildContractRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.ObjectCr));

                return this.GetProxies(buildContractQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<DogovorPkrProxy> GetProxies(IQueryable<BuildContract> buildContractRepository)
        {
            var estimateCalculationRepository = this.Container.ResolveRepository<EstimateCalculation>();

            using (this.Container.Using(estimateCalculationRepository))
            {
                var estimateSet = estimateCalculationRepository.GetAll()
                    .WhereNotNull(x => x.EstimateFile)
                    .Select(x => x.ObjectCr.Id)
                    .ToHashSet();

                return buildContractRepository
                    .Fetch(x => x.DocumentFile)
                    .Select(x => new
                    {
                        x.Id,
                        BuildContractId = x.Id,
                        PkrId = x.ObjectCr.ProgramCr.GetNullableId(),
                        DocumentNumber = x.DocumentNum,
                        DocumentDate = x.DocumentDateFrom,
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork,
                        x.Sum,
                        CustomerContragentId = x.Contragent.GetNullableId(),
                        ExecutantContragentId = x.Builder.Contragent.GetNullableId(),
                        x.GuaranteePeriod,
                        IsLawProvided = x.IsLawProvided == YesNo.Yes ? 1 : 2,
                        InfoUrl = x.WebSite,
                        Status = (BuildContractState?) x.BuildContractState,
                        RevocationReason = x.TerminationDictReason.GetNullableId(),
                        RevocationDocumentNumber = x.TerminationDocumentNumber,
                        RevocationDate = x.TerminationDate,
                        File = x.DocumentFile,

                        ObjectCrId = x.ObjectCr.Id
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var hasEstimate = estimateSet.Contains(x.ObjectCrId);
                        var status = this.GetState(x.Status);
                        return new DogovorPkrProxy
                        {
                            Id = x.Id,
                            BuildContractId = x.Id,
                            PkrId = x.PkrId,
                            DocumentNumber = x.DocumentNumber,
                            DocumentDate = x.DocumentDate,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            Sum = x.Sum,
                            IsCustomer = 2,
                            CustomerContragentId = x.CustomerContragentId,
                            ExecutantContragentId = x.ExecutantContragentId,
                            IsGuaranteePeriod = x.GuaranteePeriod.HasValue ? 1 : 2,
                            GuaranteePeriod = x.GuaranteePeriod.HasValue ? x.GuaranteePeriod * 12 : default(int?),
                            IsBudgetDocumentation = hasEstimate ? 1 : 2,
                            IsLawProvided = x.IsLawProvided,
                            InfoUrl = x.InfoUrl,
                            Status = status,
                            RevocationReason = status == 2 ? x.RevocationReason : default(long?),
                            RevocationDocumentNumber = status == 2 ? x.RevocationDocumentNumber : default(string),
                            RevocationDate = status == 2 ? x.RevocationDate : default(DateTime?),
                            File = x.File,
                            FileType = hasEstimate ? 2 : 1,
                            ObjectCrId = x.ObjectCrId
                        };
                    })
                    .ToList();
            }
        }

        private int? GetState(BuildContractState? state)
        {
            switch (state)
            {
                case BuildContractState.Active:
                    return 1;
                case BuildContractState.Cancelled:
                    return 3;
                case BuildContractState.Terminated:
                    return 2;
                default:
                    return null;
            }
        }
    }
}