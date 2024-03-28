namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class RoleDomainService : BaseDomainService<Role>
    {
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var saveResult = base.Save(baseParams);

            if (saveResult.Success)
            {
                this.Container.InTransaction(() =>
                {
                    var adminRole = (saveResult.Data as List<Role>)[0];
                    var records = baseParams.Params.GetAs<List<DynamicDictionary>>("records")[0];
                    var childIds = records.GetAs<long[]>("RoleList");

                    this.ThrowIfParentInChilds(adminRole.Id, childIds);
                    this.DeleteRecords(adminRole);

                    if (childIds.IsNotEmpty())
                    {
                        this.LocalAdminRoleRelationsDomain.Save(this.GetSaveParams(adminRole, childIds));
                    }
                });
            }

            return saveResult;
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var updateResult = base.Update(baseParams);

            if (updateResult.Success)
            {
                this.Container.InTransaction(() =>
                {
                    var adminRole = (updateResult.Data as List<Role>)[0];
                    var records = baseParams.Params.GetAs<List<DynamicDictionary>>("records")[0];
                    var childIds = records.GetAs<long[]>("RoleList");

                    this.ThrowIfParentInChilds(adminRole.Id, childIds);
                    this.DeleteRecords(adminRole);

                    if (childIds.IsNotEmpty())
                    {
                        this.LocalAdminRoleRelationsDomain.Save(this.GetSaveParams(adminRole, childIds));
                    }
                });
            }

            return updateResult;
        }

        private void DeleteRecords(Role adminRole)
        {
            this.LocalAdminRoleRelationsDomain.GetAll()
                .Where(x => x.ParentRole.Id == adminRole.Id)
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => this.LocalAdminRoleRelationsDomain.Delete(x));
        }

        private StoreSaveParams<LocalAdminRoleRelations> GetSaveParams(Role adminRole, IEnumerable<long> childIds)
        {
            var storeSaveParams = new StoreSaveParams<LocalAdminRoleRelations>();
            var records = childIds
                .Select(x => new DynamicDictionary
                {
                    { "ParentRole", adminRole.Id },
                    { "ChildRole", x }
                })
                .ToList();

            storeSaveParams.Params.Add("records", records);

            return storeSaveParams;
        }

        private void ThrowIfParentInChilds(long parentId, long[] childIds)
        {
            if (childIds.Contains(parentId))
            {
                throw new ValidationException("Выбранная роль локального администратора присутствует в списке настраиваемых ролей");
            }
        }
    }
}