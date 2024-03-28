namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using System.Linq;
    using Entities;
    using Bars.GkhGji.Enums;
    using System;
    using System.Collections.Generic;

    public interface ILogEntityHistoryService
    {
        void UpdateLog(object entity, TypeEntityLogging typeEL, long entityId, Type entityType, Dictionary<string, string> valuesDict, string baseValue);

        IDataResult GetAppealHistory(BaseParams baseParams);
    }
}