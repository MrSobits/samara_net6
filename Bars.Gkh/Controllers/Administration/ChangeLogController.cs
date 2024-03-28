namespace Bars.Gkh.Controllers
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>Лог изменений</summary>
    [ActionPermission("List", "B4.NHibernateChangeLog.View")]
    [ActionPermission("ListProperies", "B4.NHibernateChangeLog.View")]
    public class ChangeLogController : BaseController
    {
        /// <summary> Список изменений </summary>
        public ActionResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var startDate = baseParams.Params["startDate"].ToDateTime();
            var endDate = baseParams.Params["endDate"].ToDateTime();
            var entityId = baseParams.Params["entityId"].ToLong();
            var entityType = baseParams.Params["entityType"].ToStr();

            var query = this.Container.Resolve<IDomainService<LogEntity>>()
                .GetAll()
                .Where(x => x.ChangeDate.Date >= startDate && x.ChangeDate.Date <= endDate)
                .Select(x => new
                {
                    Id = x.Id,
                    ChangeDate = x.ChangeDate.ToLocalTime(),
                    ActionKind = x.ActionKind,
                    UserName = x.UserName,
                    EntityType = x.EntityType,
                    EntityId = x.EntityId,
                    EntityDescription = x.EntityDescription,
                    UserIpAddress = x.UserIpAddress
                })
                .WhereIf(entityId > 0, x => x.EntityId == entityId)
                .WhereIf(!string.IsNullOrWhiteSpace(entityType), x => x.EntityType == entityType)
                .Filter(loadParams, this.Container);

            var totalCount = query.Count();
            var data = query.Order(loadParams).Paging(loadParams).ToArray();

            return BaseGkhControllerHelper.GetJsonListResult(data, totalCount);
        }

        /// <summary> список измененных свойств указанной сущности </summary>
        public ActionResult ListProperies(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var entityId = baseParams.Params["entityId"].ToLong();

            var preData = this.Container.Resolve<IDomainService<LogEntityProperty>>().GetAll()
                .Where(x => x.LogEntity.Id == entityId)
                .Select(x => new
                {
                    x.Id,
                    x.LogEntity.EntityType,
                    x.OldValue,
                    x.NewValue,
                    PropertyName = x.PropertyCode
                })
                .ToArray();

            if (preData.Length == 0)
            {
                return JsonListResult.EmptyList;
            }

            var auditLogMap = this.Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .FirstOrDefault(x => x.EntityType == preData.Select(y => y.EntityType).First());

            if (auditLogMap == null)
            {
                throw new ValidationException("Не удалось определить тип логируемой сущности. Обратитесь к администратору.");
            }

            var data = preData
                .Select(x => new 
                {
                    Id = x.Id,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue,
                    PropertyName = auditLogMap.GetProperty(x.PropertyName) != null ? auditLogMap.GetProperty(x.PropertyName).DisplayName : x.PropertyName
                })
                .AsQueryable()
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .ToArray();

            return new JsonListResult(data);
        }
    }
}