namespace Bars.GkhGji.Regions.Saha.DomainService.Reminder.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    public class ReminderService : GkhGji.DomainService.ReminderService
    {
        public override IDataResult ListWidgetInspector(BaseParams baseParams)
        {
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var userManager = Container.Resolve<IGkhUserManager>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var inspectorId = activeOperator.Inspector != null ? activeOperator.Inspector.Id : 0;

                var data =
                    servReminder.GetAll()
                        .Where(x => x.Actuality)
                        .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                        .Select(
                            x =>
                            new
                                {
                                    x.Id,
                                    x.TypeReminder,
                                    x.CheckDate,
                                    Num = x.Num ?? string.Empty,
                                    DocNum = x.AppealCits.DocumentNumber,
                                    NumGji = x.AppealCits.NumberGji,
                                    ContragentName = x.Contragent != null ? x.Contragent.Name : string.Empty,
                                    ColorTypeReminder = "red",
                                    x.CategoryReminder
                                })
                        .OrderBy(x => x.CheckDate);

                var totalCount = data.Count();

                var result =
                    data.Paging(loadParam)
                        .AsEnumerable()
                        .Select(
                            x =>
                            new
                                {
                                    x.Id,
                                    x.TypeReminder,
                                    CheckDate = x.CheckDate.HasValue ? x.CheckDate.ToDateTime() : DateTime.MinValue,
                                    Num = x.TypeReminder == TypeReminder.Statement ? x.NumGji : x.Num,
                                    NumText = x.TypeReminder == TypeReminder.Statement ? "Номер обращения" : "№ ГЖИ",
                                    x.ContragentName,
                                    ColorTypeReminder = GetColorTypeReminder(x.TypeReminder),
                                    x.CategoryReminder,
                                    Color = DateTime.Now >= x.CheckDate.GetValueOrDefault() ? "red" : x.CheckDate.GetValueOrDefault().AddDays(-5) <= DateTime.Now ? "yellow" : "green"
                                })
                        .ToList();

                return new ListDataResult(result, totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servReminder);
                Container.Release(servInsSub);
            }
        }
    }
}