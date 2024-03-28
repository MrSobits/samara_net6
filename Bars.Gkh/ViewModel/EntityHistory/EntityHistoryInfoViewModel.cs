namespace Bars.Gkh.ViewModel.EntityHistory
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    public class EntityHistoryInfoViewModel : BaseViewModel<EntityHistoryInfo>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<EntityHistoryInfo> domainService, BaseParams baseParams)
        {
            var groupType = baseParams.Params.GetAs<EntityHistoryType>("groupType");
            var parentId = baseParams.Params.GetAsId("parentId");
            var parentName = baseParams.Params.GetAs<string>("parentName");
            var entityId = baseParams.Params.GetAsId("entityId");
            var entityName = baseParams.Params.GetAs<string>("entityName");

            if (parentId == 0 && entityId == 0)
            {
                return new ListDataResult(null, 0)
                {
                    Success = false,
                    Message = "Не указан параметр 'parentId' / 'entityId'"
                };
            }

            return domainService.GetAll()
                .WhereIf(groupType != default(EntityHistoryType), x => x.GroupType == groupType)
                .WhereIf(parentId > 0, x => x.ParentEntityId == parentId)
                .WhereIf(!string.IsNullOrEmpty(parentName), x => x.ParentEntityName == parentName)
                .WhereIf(entityId > 0, x => x.EntityId == entityId)
                .WhereIf(!string.IsNullOrEmpty(parentName), x => x.EntityName == entityName)
                .Select(x => new
                {
                    x.Id,
                    x.EditDate,
                    x.ActionKind,
                    x.IpAddress,
                    x.EntityId,
                    x.EntityName,
                    x.ParentEntityId,
                    x.ParentEntityName,
                    x.Username,
                    x.User.Login
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}