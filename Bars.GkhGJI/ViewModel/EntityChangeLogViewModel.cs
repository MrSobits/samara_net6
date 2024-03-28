namespace Bars.GkhGji.ViewModel.Email
{
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Email;
    using System;
    using System.Linq;

    public class EntityChangeLogRecordViewModel : BaseViewModel<EntityChangeLogRecord>
    {
        public override IDataResult List(IDomainService<EntityChangeLogRecord> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var data = domain.GetAll()
                .Where(x=> dateStart <= x.ObjectCreateDate && x.ObjectCreateDate<= dateEnd.AddDays(1))
                .Select(x=> new
                {
                  x.Id,
                  AuditDate = x.ObjectCreateDate,
                  x.NewValue,
                  x.OldValue,
                  x.OperationType,
                  x.OperatorLogin,
                  x.DocumentValue,
                  x.EntityId,
                  x.OperatorId,
                  x.OperatorName,
                  x.PropertyType,
                  x.PropertyName,
                  x.TypeEntityLogging
                }).Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}