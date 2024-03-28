namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    public class ReminderService : Bars.GkhGji.DomainService.ReminderService
    {

        public override IQueryable<Reminder> GetQueryable(BaseParams baseParams, IDomainService<Reminder> service)
        {
            var query = service.GetAll();

            var dopFilter = baseParams.Params.GetAs("dopFilter", 10);
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var isStatement = baseParams.Params.GetAs("isStatement", true);
            var isDisposal = baseParams.Params.GetAs("isDisposal", true);
            var isPrescription = baseParams.Params.GetAs("isPrescription", true);
            var isBaseInspection = baseParams.Params.GetAs("isBaseInspection", true);
            var isActCheck = baseParams.Params.GetAs("isActCheck", true);
            var isNoticeOfInspection = baseParams.Params.GetAs("isNoticeOfInspection", true);

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
                     .WhereIf(!isBaseInspection, x => x.TypeReminder != TypeReminder.BaseInspection)
                     .WhereIf(!isActCheck, x => x.TypeReminder != TypeReminder.ActCheck)
                     .WhereIf(!isNoticeOfInspection, x => x.TypeReminder != TypeReminder.NoticeOfInspection);
        }

        public override TypeReminder[] ReminderTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new TypeReminder[]
                {
                    TypeReminder.Statement,
                    TypeReminder.BaseInspection, 
                    TypeReminder.Disposal, 
                    TypeReminder.Prescription,
                    TypeReminder.ActCheck, 
                    TypeReminder.NoticeOfInspection
                };
        }

        /// <summary>
        /// Список напоминалок в Виджете для Инспектора
        /// </summary>
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

                var inspectorId =
                    activeOperator.Inspector != null
                        ? activeOperator.Inspector.Id
                        : 0;

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality)
                    .WhereIf(inspectorId > 0, x => x.Inspector.Id == inspectorId)
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeReminder,
                        x.CheckDate,
                        Num = x.Num ?? "",
                        AppealId = x.AppealCits != null ? x.AppealCits.Id: 0,
                        AppealNum =  x.AppealCits != null ? x.AppealCits.DocumentNumber : "",
                        AppealNumGji =  x.AppealCits != null ? x.AppealCits.NumberGji : "",
                        AppealCorr = x.AppealCits != null ? x.AppealCits.Correspondent : "",
                        AppealCorrAddress = x.AppealCits != null ? x.AppealCits.CorrespondentAddress : "",
                        AppealDescription = x.AppealCits != null ? x.AppealCits.Description : "",
                        ContragentName = x.Contragent != null ? x.Contragent.Name : "",
                        ColorTypeReminder = "red",
                        x.CategoryReminder
                    })
                    .OrderBy(x => x.CheckDate);

                var totalCount = data.Count();

                var result = data
                    .Paging(loadParam)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeReminder,
                        CheckDate = x.CheckDate.HasValue ? x.CheckDate.ToDateTime() : DateTime.MinValue,
                        Num = x.TypeReminder == TypeReminder.Statement ? (!string.IsNullOrEmpty(x.AppealNum) ? x.AppealNum : x.AppealNumGji) : x.Num,
                        NumText = x.TypeReminder == TypeReminder.Statement ? "Номер обращения" : "№ ГЖИ",
                        x.AppealCorr,
                        x.AppealCorrAddress,
                        AppealDescription = x.AppealDescription.Length > 80 ? x.AppealDescription.Substring(0, 80).Trim() + "..." : x.AppealDescription,
                        x.ContragentName,
                        ColorTypeReminder = GetColorTypeReminder(x.TypeReminder),
                        x.CategoryReminder,
                        Color = DateTime.Now >= x.CheckDate.GetValueOrDefault()
                            ? "red"
                            : x.CheckDate.GetValueOrDefault().AddDays(-5) <= DateTime.Now
                                ? "yellow"
                                : "green"
                    }).ToList();

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