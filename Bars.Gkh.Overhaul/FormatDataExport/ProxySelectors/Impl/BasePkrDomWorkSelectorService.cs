namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Базовый селектор Работ дома ПКР (pkrdomwork.csv)
    /// </summary>
    public abstract class BasePkrDomWorkSelectorService : BaseProxySelectorService<PkrDomWorkProxy>
    {
        // <inheritdoc />
        protected override ICollection<PkrDomWorkProxy> GetAdditionalCache()
        {
            var typeWorkCrRepos = this.Container.ResolveRepository<TypeWorkCr>();
            using (this.Container.Using(typeWorkCrRepos))
            {
                return this.GetProxies(typeWorkCrRepos.GetAll()
                    .Where(x => x.Work.TypeWork == TypeWork.Work)
                    .WhereContainsBulked(x => x.Id, this.AdditionalIds)
                    .Select(x => new TypeWorkCrDto
                    {
                        Id = x.Id,
                        PkrDomId = x.ObjectCr.GetNullableId(),
                        WorkKprTypeId = x.Work.GetNullableId(),
                        StartDate = x.DateStartWork,
                        EndDate = x.DateEndWork
                    })
                    .ToList());
            }
        }

        protected IList<PkrDomWorkProxy> GetProxies(ICollection<TypeWorkCrDto> typeWorks)
        {
            var uniqueTypeWorks = new HashSet<TypeWorkCrDto>(typeWorks, EntityEqComparer.ById<TypeWorkCrDto>());
            var financeResourseDict = this.GetFinanceSourceResourceDto(uniqueTypeWorks);
            return uniqueTypeWorks
                .Select(x =>
                {
                    var financeResourse = financeResourseDict.Get(x.Id);

                    return new PkrDomWorkProxy
                    {
                        Id = x.Id,
                        PkrDomId = x.PkrDomId,
                        WorkKprTypeId = x.WorkKprTypeId,
                        StartDate = x.IsLongTerm ? x.StartDate : default(DateTime?),
                        EndDate = x.EndDate,
                        FundResourses = x.IsLongTerm ? default(decimal?) : financeResourse?.FundResource,
                        SubjectBudget = x.IsLongTerm ? default(decimal?) : financeResourse?.BudgetSubject,
                        LocalBudget = x.IsLongTerm ? default(decimal?) : financeResourse?.BudgetMu,
                        OwnerBudget = x.IsLongTerm ? default(decimal?) : financeResourse?.OwnerResource
                    };
                })
                .ToList();
        }

        private IDictionary<long, FinanceSourceResourceDto> GetFinanceSourceResourceDto(ICollection<TypeWorkCrDto> typeWorks)
        {
            var financeSourceResourceRepository = this.Container.ResolveRepository<FinanceSourceResource>();
            using (this.Container.Using(financeSourceResourceRepository))
            {
                var take = 5000;
                if (typeWorks.Count < take)
                {
                    return financeSourceResourceRepository.GetAll()
                        .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                        .WhereContainsBulked(x => x.TypeWorkCr.Id, typeWorks.Select(x => x.Id), take)
                        .Select(x => new FinanceSourceResourceDto
                        {
                            Id = x.Id,
                            TypeWorkId = x.TypeWorkCr.GetId(),
                            FundResource = x.FundResource,
                            BudgetSubject = x.BudgetSubject,
                            BudgetMu = x.BudgetMu,
                            OwnerResource = x.OwnerResource
                        })
                        .ToDictionary(x => x.TypeWorkId);
                }

                var dtoList = new List<FinanceSourceResourceDto>(typeWorks.Count);
                for (int skip = 0; skip < typeWorks.Count; skip += take)
                {
                    var typeWorkPart = typeWorks.Select(x => x.Id)
                        .Skip(skip)
                        .Take(take)
                        .ToArray();
                    dtoList.AddRange(financeSourceResourceRepository.GetAll()
                        .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                        .WhereContainsBulked(x => x.TypeWorkCr.Id, typeWorkPart, take)
                        .Select(x => new FinanceSourceResourceDto
                        {
                            Id = x.Id,
                            TypeWorkId = x.TypeWorkCr.GetId(),
                            FundResource = x.FundResource,
                            BudgetSubject = x.BudgetSubject,
                            BudgetMu = x.BudgetMu,
                            OwnerResource = x.OwnerResource
                        })
                    );
                }

                return dtoList.ToDictionary(x => x.TypeWorkId);
            }
        }

        protected class TypeWorkCrDto : IHaveId
        {
            public long Id { get; set; }
            public long? PkrDomId { get; set; }
            public long? WorkKprTypeId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool IsLongTerm { get; set; }
        }

        private class FinanceSourceResourceDto
        {
            public long Id { get; set; }
            public long TypeWorkId { get; set; }
            public decimal? FundResource { get; set; }
            public decimal? BudgetSubject { get; set; }
            public decimal? BudgetMu { get; set; }
            public decimal? OwnerResource { get; set; }
        }
    }
}