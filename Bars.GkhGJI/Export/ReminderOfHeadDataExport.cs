namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.DomainService;

    public class ReminderOfHeadDataExport : BaseDataExportService
    {

        public override IList GetExportData(BaseParams baseParams)
        {

            var userManager = Container.Resolve<IGkhUserManager>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var reminderDomainService = Container.Resolve<IReminderService>();

            try
            {

                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new List<Reminder>();
                }

                var inspectorIds = servInsSub.GetAll()
                    .Where(x => x.SignedInspector.Id == activeOperator.Inspector.Id)
                    .Select(x => x.Inspector.Id);
                
                //Параметры с виджетов
                var inspectorId = baseParams.Params.GetAs<long>("inspectorId");
                var colorType = baseParams.Params.GetAs("colorType", string.Empty);
                var typeReminder = baseParams.Params.GetAs("typeReminder", TypeReminder.BaseInspection);
                var isTypeReminder = baseParams.Params.GetAs("isTypeReminder", false);

                Expression<Func<Reminder, bool>> func = x => true;
                switch (colorType)
                {
                    case "red":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && DateTime.Now >= x.CheckDate.GetValueOrDefault();
                        break;
                    case "yellow":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.GetValueOrDefault() > DateTime.Now && DateTime.Now.AddDays(5) > x.CheckDate.GetValueOrDefault();
                        break;
                    case "green":
                        func = x => x.TypeReminder != TypeReminder.BaseInspection && x.CheckDate.GetValueOrDefault() > DateTime.Now.AddDays(5);
                        break;
                    case "white":
                        func = x => x.TypeReminder == TypeReminder.BaseInspection;
                        break;
                }

                var cntInspectors = inspectorIds.Count();

                var result = reminderDomainService.GetQueryable(baseParams, servReminder)
                    .Where(x => x.Actuality)
                    .WhereIf(cntInspectors > 0, x => inspectorIds.Any(y => y == x.Inspector.Id)) //!!!!!!!!!
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .WhereIf(isTypeReminder, x => x.TypeReminder == typeReminder)
                    .WhereIf(!string.IsNullOrEmpty(colorType), func)
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                InspectionGji =
                            x.InspectionGji != null
                                ? new { x.InspectionGji.Id, x.InspectionGji.TypeBase }
                                : null,
                                DocumentGji =
                            x.DocumentGji != null
                                ? new { x.DocumentGji.Id, x.DocumentGji.TypeDocumentGji }
                                : null,
                                AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                                Contragent = x.Contragent.Name,
                                x.Actuality,
                                x.TypeReminder,
                                x.CategoryReminder,
                                x.Num,
                                x.CheckDate,
                                Inspector = x.Inspector.Fio
                            })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, Container)
                    .ToList();

                return result;
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servReminder);
                Container.Release(servInsSub);
                Container.Release(reminderDomainService);
            }
        }
    }
}
