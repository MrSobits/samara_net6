namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Сервис получения <see cref="WorkDogovProxy"/>
    /// </summary>
    public class WorkDogovSelectorService : BaseProxySelectorService<WorkDogovProxy>
    {
        /// <inheritdoc />
        protected override ICollection<WorkDogovProxy> GetAdditionalCache()
        {
            var buildContractTypeWorkRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractTypeWorkRepository))
            {
                return this.GetProxies(buildContractTypeWorkRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, WorkDogovProxy> GetCache()
        {
            var buildContractTypeWorkRepository = this.Container.ResolveRepository<BuildContractTypeWork>();

            using (this.Container.Using(buildContractTypeWorkRepository))
            {
                var objectCrQuery = this.FilterService.GetFiltredQuery<ObjectCr>();

                var bcTypeWorkQuery = this.FilterService.ObjectCrIds.Any()
                    ? buildContractTypeWorkRepository.GetAll()
                        .WhereContainsBulked(x => x.BuildContract.ObjectCr.Id, this.FilterService.ObjectCrIds)
                    : buildContractTypeWorkRepository.GetAll()
                        .Where(x => objectCrQuery.Any(y => y == x.BuildContract.ObjectCr));

                return this.GetProxies(bcTypeWorkQuery).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<WorkDogovProxy> GetProxies(IQueryable<BuildContractTypeWork> buildContractTypeQuery)
        {
            return buildContractTypeQuery
                .Select(x => new
                {
                    x.Id,
                    BuildContractId = x.BuildContract.GetNullableId(),
                    TypeWorkId = x.TypeWork.GetNullableId(),
                    x.TypeWork.DateStartWork,
                    x.TypeWork.DateEndWork,
                    x.Sum,
                    TypeWorkSum = x.TypeWork.Sum,
                    x.TypeWork.Volume,
                    UnitMeasureName = x.TypeWork.Work.UnitMeasure.Name,
                    x.TypeWork.Work.Description
                })
                .AsEnumerable()
                .Select(x => new WorkDogovProxy
                {
                    Id = x.Id,
                    DogovorPkrId = x.BuildContractId,
                    IsHouseWork = 1,
                    PkrDomWorkId = x.TypeWorkId,

                    //5,6,7 не передается
                    StartDate = x.DateStartWork,
                    EndDate = x.DateEndWork,
                    ContractAmount = x.Sum,
                    KprAmount = x.TypeWorkSum,
                    WorkVolume = x.Volume,
                    AnotherUnit = x.UnitMeasureName,
                    Description = x.Description
                })
                .ToList();
        }
    }
}