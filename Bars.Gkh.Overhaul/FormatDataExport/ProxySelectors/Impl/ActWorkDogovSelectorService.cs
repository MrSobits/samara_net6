namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения <see cref="ActWorkDogovProxy"/>
    /// </summary>
    public class ActWorkDogovSelectorService : BaseProxySelectorService<ActWorkDogovProxy>
    {
        /// <inheritdoc />
        protected override ICollection<ActWorkDogovProxy> GetAdditionalCache()
        {
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            using (this.Container.Using(performedWorkActRepository))
            {
                return this.GetProxies(performedWorkActRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, ActWorkDogovProxy> GetCache()
        {
            var performedWorkActRepository = this.Container.ResolveRepository<PerformedWorkAct>();

            using (this.Container.Using(performedWorkActRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var performedWorkActPaymentkQuery = this.FilterService.ObjectCrIds.Any()
                    ? performedWorkActRepository.GetAll()
                        .WhereContainsBulked(x => x.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : performedWorkActRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.ObjectCr));

                return this.GetProxies(performedWorkActPaymentkQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<ActWorkDogovProxy> GetProxies(IQueryable<PerformedWorkAct> performedWorkActQuery)
        {
            var buildContractRepository = this.Container.ResolveRepository<BuildContractTypeWork>();
            using (this.Container.Using(buildContractRepository))
            {
                return performedWorkActQuery
                    .Join(buildContractRepository.GetAll(),
                        x => x.TypeWorkCr,
                        x => x.TypeWork,
                        (x, c) => new
                        {
                            x.Id,
                            x.State,
                            ContractId = c.BuildContract.GetNullableId(),
                            WorkName = x.TypeWorkCr.Work.Name,
                            FinanceSourceName = x.TypeWorkCr.FinanceSource.Name,
                            x.DocumentNum,
                            x.DateFrom,
                            x.Sum,
                            x.RepresentativeSurname,
                            x.RepresentativeName,
                            x.RepresentativePatronymic,
                            WorkDogovId = c.GetNullableId(),
                            x.RepresentativeSigned,
                            x.Volume,
                            x.ExploitationAccepted,
                            x.WarrantyStartDate,
                            x.WarrantyEndDate,
                            x.AdditionFile
                        })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var nameParts = this.Converter.ParseFullName(x.RepresentativeName);
                        return new ActWorkDogovProxy
                        {
                            Id = x.Id,
                            DogovorPkrId = x.ContractId,
                            Status = 1,
                            Name = x.WorkName,
                            Number = x.DocumentNum,
                            Date = x.DateFrom,
                            Sum = x.Sum,
                            IsSigned = x.RepresentativeSigned == YesNo.Yes ? 1 : 2,
                            AgentSurname = x.RepresentativeSurname ?? nameParts.Item1,
                            AgentName = nameParts.Item2 ?? x.RepresentativeName,
                            AgentPatronymic = x.RepresentativePatronymic ?? nameParts.Item3,
                            IsInstallments = 2,

                            // ACTWORK
                            WorkDogovId = x.WorkDogovId,
                            Cost = x.Sum,
                            Volum = x.Volume,
                            ExploitationAccepted = x.ExploitationAccepted == YesNo.Yes ? 1 : 2,
                            WarrantyStartDate = x.WarrantyStartDate,
                            WarrantyEndDate = x.WarrantyEndDate,

                            // ACTWORKDOGOVFILES
                            File = x.AdditionFile,
                            Type = 1
                        };
                    })
                    .ToList();
            }
        }
    }
}