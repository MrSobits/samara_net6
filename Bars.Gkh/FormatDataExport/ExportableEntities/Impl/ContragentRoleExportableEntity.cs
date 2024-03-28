namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контрагенты
    /// </summary>
    public class ContragentRoleExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CONTRAGENTROLE";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var contragentRoleRepository = this.Container.ResolveRepository<ContragentAdditionRole>();
            using (this.Container.Using(contragentRoleRepository))
            {
                var contragents = this.ProxySelectorFactory.GetSelector<ContragentProxy>()
                    .ExtProxyListCache;

                var additionRoles = contragentRoleRepository.GetAll()
                    .WhereContainsBulked(x => x.Contragent.ExportId, contragents.Select(x => x.Id))
                    .Select(x => new
                    {
                        x.Contragent.ExportId,
                        x.Role.Code
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        ContragentId = x.ExportId,
                        Role = x.Code,
                        IsMainRole = 2
                    });

                var contragentMainRoles = contragents
                    .Select(x => new
                    {
                        ContragentId = x.Id,
                        Role = x.MainRoleCode,
                        IsMainRole = 1
                    });

                var i = 1;
                return additionRoles
                    .Union(contragentMainRoles)
                    .Select(x => new ExportableRow(i++,
                        new List<string>
                        {
                            x.ContragentId.ToStr(),
                            x.Role,
                            x.IsMainRole.ToStr()
                        }))
                    .ToList();
            }
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Полномочие /роль/тип контрагента",
                "Роль является основной"
            };
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();
    }
}