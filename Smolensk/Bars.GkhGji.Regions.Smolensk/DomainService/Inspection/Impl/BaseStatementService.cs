﻿namespace Bars.GkhGji.Regions.Smolensk.DomainService.Inspection.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class BaseStatementService : GkhGji.DomainService.BaseStatementService
    {
        protected override void GetAppealCitizenInfo(ref string appealCitizensNames, ref string appealCitizensIds, long inspectionId)
        {
            var service = Container.Resolve<IDomainService<InspectionAppealCits>>();

            try
            {
                // Пробегаемся по обращениям и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors =
                    service.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => new { x.AppealCits.Id, x.AppealCits.DocumentNumber })
                        .ToList();

                appealCitizensNames = string.Join(", ", dataInspectors.Select(x => x.DocumentNumber));
                appealCitizensIds = string.Join(", ", dataInspectors.Select(x => x.Id.ToStr()));
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}