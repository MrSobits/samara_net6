namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="WorkKprTypeProxy"/>
    /// </summary>
    public class WorkKprTypeSelectorService : BaseProxySelectorService<WorkKprTypeProxy>
    {
        /// <inheritdoc />
        protected override bool CanGetFullData()
        {
            return false;
        }

        /// <inheritdoc />
        protected override ICollection<WorkKprTypeProxy> GetAdditionalCache()
        {
            var workRepository = this.Container.ResolveRepository<Work>();
            var structuralElementRepository = this.Container.ResolveRepository<StructuralElement>();

            using (this.Container.Using(workRepository, structuralElementRepository))
            {
                return this.GetProxies(workRepository.GetAll()
                        .WhereContainsBulked(x => x.ExportId, this.AdditionalIds),
                    structuralElementRepository.GetAll()
                        .WhereContainsBulked(x => x.ExportId, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, WorkKprTypeProxy> GetCache()
        {
            var workRepository = this.Container.ResolveRepository<Work>();
            var structuralElementRepository = this.Container.ResolveRepository<StructuralElement>();

            using (this.Container.Using(workRepository, structuralElementRepository))
            {
                return this.GetProxies(workRepository.GetAll(), structuralElementRepository.GetAll()).ToDictionary(x => x.Id);
            }
        }

        protected virtual IList<WorkKprTypeProxy> GetProxies(IQueryable<Work> workQuery, IQueryable<StructuralElement> structuralElements)
        {
            var coObjects = structuralElements
                .Select(x => new WorkKprTypeProxy
                {
                    Id = x.ExportId,
                    GroupCode = x.Group.CommonEstateObject.GisCode,
                    Name = x.Name
                })
                .ToList();

            return workQuery
                .Select(x => new WorkKprTypeProxy
                {
                    Id = x.ExportId,
                    GroupCode = x.GisCode,
                    Name = x.Name
                })
                .AsEnumerable()
                .Union(coObjects)
                .ToList();
        }
    }
}