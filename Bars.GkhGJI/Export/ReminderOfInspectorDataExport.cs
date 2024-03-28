namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.DomainService;

    public class ReminderOfInspectorDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var reminderDomainService = Container.Resolve<IReminderService>();
            
            try
            {
                var loadParams = GetLoadParam(baseParams);
                var activeOperator = userManager.GetActiveOperator();
                var inspector = activeOperator != null && activeOperator.Inspector != null
                                    ? activeOperator.Inspector
                                    : null;

                var loadParam = baseParams.GetLoadParam();
                var result = reminderDomainService.GetQueryable(baseParams, servReminder)
                    .WhereIf(inspector != null, x => x.Inspector.Id == inspector.Id)
                    .WhereIf(inspector == null, x => x.Id < 0)
                    .Select(x => new
                        {
                            x.Id,
                            Contragent = x.Contragent != null ? x.Contragent.Name : string.Empty,
                            x.Actuality,
                            x.TypeReminder,
                            x.CategoryReminder,
                            x.Num,
                            x.CheckDate
                        })
                    .Filter(loadParams, Container)
                    .Order(loadParam)
                    .ToList();

                return result;
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servReminder);
                Container.Release(reminderDomainService);
            }
        }
    }
}