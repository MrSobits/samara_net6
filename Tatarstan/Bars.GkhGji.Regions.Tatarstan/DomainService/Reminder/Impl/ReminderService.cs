namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    public class ReminderService : Bars.GkhGji.DomainService.ReminderService
    {
        public override TypeReminder[] ReminderTypes()
        {
            // В РТ другие типы 
            return new []
                {
                    TypeReminder.Statement,
                    TypeReminder.BaseInspection, 
                    TypeReminder.Disposal, 
                    TypeReminder.Prescription,
                    TypeReminder.Resolution
                };
        }

        public override IQueryable<Reminder> GetQueryable(BaseParams baseParams, IDomainService<Reminder> service)
        {
            var query = service.GetAll();

            var dopFilter = baseParams.Params.GetAs("dopFilter", 10);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isStatement = baseParams.Params.GetAs("isStatement", true);
            var isDisposal = baseParams.Params.GetAs("isDisposal", true);
            var isPrescription = baseParams.Params.GetAs("isPrescription", true);
            var isResolution = baseParams.Params.GetAs("isResolution", true);
            var isBaseInspection = baseParams.Params.GetAs("isBaseInspection", true);

            switch (dopFilter)
            {
                case 20:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection)
                            .Where(x => x.CheckDate.HasValue)
                            .Where(x => DateTime.Now.Date < x.CheckDate)
                            .Where(x => DateTime.Now.AddDays(5) >= x.CheckDate.Value);
                    }
                    break;

                case 30:
                    {
                        query = query
                            .Where(x => x.TypeReminder != TypeReminder.BaseInspection)
                            .Where(x => (x.CheckDate.HasValue && DateTime.Now.Date >= x.CheckDate) || !x.CheckDate.HasValue);
                    }
                    break;
                case 40:
                    {
                        query = query
                            .Where(x => x.TypeReminder == TypeReminder.BaseInspection);
                    }
                    break;

            }

            return query.WhereIf(dateStart != DateTime.MinValue, x => x.CheckDate >= dateStart)
                     .WhereIf(dateEnd != DateTime.MinValue, x => x.CheckDate <= dateEnd)
                     .WhereIf(!isStatement, x => x.TypeReminder != TypeReminder.Statement)
                     .WhereIf(!isDisposal, x => x.TypeReminder != TypeReminder.Disposal)
                     .WhereIf(!isPrescription, x => x.TypeReminder != TypeReminder.Prescription)
                     .WhereIf(!isResolution, x => x.TypeReminder != TypeReminder.Resolution)
                     .WhereIf(!isBaseInspection, x => x.TypeReminder != TypeReminder.BaseInspection);
        }
    }
}