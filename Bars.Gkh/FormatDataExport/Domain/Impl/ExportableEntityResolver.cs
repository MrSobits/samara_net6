namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    public class ExportableEntityResolver : IExportableEntityResolver
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<IExportableEntity> ExportableEntities { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroups { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }
        public ILogger LogManager { get; set; }

        /// <inheritdoc />
        public IExportableEntity GetEntity(string entityCode, FormatDataExportProviderType providerType)
        {
            var entity = this.ExportableEntities
                .SingleOrDefault(x => x.Code == entityCode);

            if (entity == null)
            {
                throw new InstanceNotFoundException($"Не найдена сущность с кодом '{entityCode}' доступная {providerType.GetDisplayName()}");
            }

            return entity;
        }

        /// <inheritdoc />
        public IList<IExportableEntity> GetEntityList(FormatDataExportProviderType providerType)
        {
            return this.Sort(this.ExportableEntities);
        }

        /// <inheritdoc />
        public IList<IExportableEntity> GetInheritedEntityList(IList<string> entityGroupCodes, FormatDataExportProviderType providerType)
        {
            var allResolvedEntityCodes = new HashSet<string>();

            var codes = this.ExportableEntityGroups
                .GroupBy(g => g.Code)
                .Where(x => entityGroupCodes.Contains(x.Key))
                .SelectMany(group =>
                {
                    var inheritedEntityCodeList = group.First().InheritedEntityCodeList.ToList();
                    var trigger = true;

                    foreach (var data in group)
                    {
                        if (trigger)
                        {
                            trigger = !trigger;
                            continue;
                        }

                        inheritedEntityCodeList.AddRange(data.InheritedEntityCodeList);
                    }

                    return inheritedEntityCodeList;
                })
                .Distinct();

            foreach (var code in codes)
            {
                this.FindInheritedEntities(code, allResolvedEntityCodes, providerType);
            }

            return this.Sort(this.ExportableEntities.Where(x => allResolvedEntityCodes.Contains(x.Code)));
        }

        private IList<IExportableEntity> Sort(IEnumerable<IExportableEntity> exportableEntities)
        {
            var dict = exportableEntities
                .ToDictionary(x => x.Code);

            var sortedEntities = dict.Values
                .TopoSort(x => x.GetInheritedEntityCodeList().Where(dict.ContainsKey).Select(code => dict[code]))
                .Reverse()
                .ToList();

            dict.Clear();
            dict = null;

            return sortedEntities;
        }

        private void FindInheritedEntities(string entityCode, HashSet<string> entitySet, FormatDataExportProviderType providerType)
        {
            var entity = this.ExportableEntities.SingleOrDefault(x => x.Code == entityCode);

            if (entity == null)
            {
                this.LogManager.LogWarning($"Не найдена экспортируемая сущность: '{entityCode}'");
                return;
            }

            if (!entitySet.Add(entityCode))
            {
                return;
            }

            foreach (var code in entity.GetInheritedEntityCodeList())
            {
                this.FindInheritedEntities(code, entitySet, providerType);
            }
        }
    }
}