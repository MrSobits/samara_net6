namespace Bars.Gkh.ViewModel.Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    public class FormatDataExportTaskViewModel : BaseViewModel<FormatDataExportTask>
    {
        public IGkhUserManager GkhUserManager { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroup { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<FormatDataExportTask> domainService, BaseParams baseParams)
        {
            var activeUser = this.GkhUserManager.GetActiveUser()?.Id ?? 0;

            var isAdministrator = this.GkhUserManager.GetActiveUser().Roles.Any(x => x.Role.Name == "Администратор");

            var entityGroupDict = this.ExportableEntityGroup
                .GroupBy(g => g.Code)
                .Select(x => new
                    {
                        x.First().Code,
                        x.First().Description
                    }
                )
                .ToDictionary(x => x.Code, x => x.Description);

            return domainService.GetAll()
                .Where(x => !x.IsDelete)
                .WhereIf(!isAdministrator, x => x.User.Id == activeUser)
                .Select(x => new
                {
                    x.Id,
                    x.User.Login,
                    x.ObjectCreateDate,
                    x.StartDate,
                    x.EndDate,
                    x.StartTimeHour,
                    x.StartTimeMinutes,
                    x.PeriodType,
                    x.StartDayOfWeekList,
                    x.StartMonthList,
                    x.EntityGroupCodeList
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Login,
                    TriggerName = this.GetTriggerName(x.PeriodType, x.StartTimeHour, x.StartTimeMinutes, x.StartDayOfWeekList, x.StartMonthList),
                    CreateDate = x.ObjectCreateDate,
                    x.StartDate,
                    x.EndDate,
                    EntityGroupCodeList = this.GetGroupNameList(x.EntityGroupCodeList, entityGroupDict)
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<FormatDataExportTask> domainService, BaseParams baseParams)
        {
            var contragentRepos = this.Container.ResolveRepository<Contragent>();
            var municipalityRepos = this.Container.ResolveRepository<Municipality>();
            var chargePeriodRepos = this.Container.TryResolve<IChargePeriodRepository>();

            using (this.Container.Using(contragentRepos, municipalityRepos, chargePeriodRepos))
            {
                var task = domainService.Get(baseParams.Params.GetAsId());
                var entityGroupCodeList = this.ExportableEntityGroup
                    .GroupBy(g => g.Code)
                    .Where(y => task.EntityGroupCodeList.Contains(y.Key))
                    .Select(x => new
                    {
                        x.First().Code,
                        x.First().Description
                    })
                    .ToList();
                var displayParams = new DynamicDictionary
                {
                    ["Идентификатор задачи"] = task.Id,
                    ["Выбранные секции"] = entityGroupCodeList.Select(x => x.Description).AggregateWithSeparator(", "),
                };
                var taskParams = task.BaseParams;
                if (taskParams != null)
                {
                    var empty = new long[0];

                    var mainContragentId = taskParams.Params.GetAsId("MainContragent");
                    var municipalityIds = taskParams.Params.GetAs("MunicipalityList", empty);
                    var programVersionMunicipalityIds = taskParams.Params.GetAs("ProgramVersionMunicipalityList", empty);
                    var objectCrMunicipalityIds = taskParams.Params.GetAs("ObjectCrMunicipalityList", empty);
                    var contragentIds = taskParams.Params.GetAs("ContragentList", empty);
                    var chargePeriodId = taskParams.Params.GetAsId("ChargePeriod");
                    var mainContragent = contragentRepos.GetAll()
                        .Where(x => x.Id == mainContragentId)
                        .Select(x => x.Name)
                        .FirstOrDefault();

                    var municipalityAllIds = municipalityIds
                        .Union(programVersionMunicipalityIds)
                        .Union(objectCrMunicipalityIds)
                        .ToHashSet();

                    var municipalitiesAll = municipalityRepos.GetAll()
                        .WhereContainsBulked(x => x.Id, municipalityAllIds)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name
                        })
                        .ToList();

                    var periodName = chargePeriodRepos?.Get(chargePeriodId)?.Name;

                    var municipalities = municipalitiesAll
                        .Where(x => municipalityIds.Contains(x.Id))
                        .Select(x => x.Name)
                        .AggregateWithSeparator(", ");
                    var programVersionMunicipalities = municipalitiesAll
                        .Where(x => programVersionMunicipalityIds.Contains(x.Id))
                        .Select(x => x.Name)
                        .AggregateWithSeparator(", ");
                    var objectCrMunicipalities = municipalitiesAll
                        .Where(x => objectCrMunicipalityIds.Contains(x.Id))
                        .Select(x => x.Name)
                        .AggregateWithSeparator(", ");
                    var contragents = contragentRepos.GetAll()
                        .WhereContainsBulked(x => x.Id, contragentIds)
                        .Select(x => x.Name)
                        .AggregateWithSeparator(", ");

                    displayParams.Add("Головная организация", mainContragent);
                    this.AddIfHasValue(displayParams, municipalities, "Муниципальный район");
                    this.AddIfHasValue(displayParams, contragents, "Контрагенты для выгрузки информации");
                    this.AddIfHasValue(displayParams, programVersionMunicipalities, "Муниципальные районы версий ДПКР");
                    this.AddIfHasValue(displayParams, objectCrMunicipalities, "Муниципальные районы объектов КР");

                    this.AddIfHasValue<bool?>(displayParams, taskParams, "UseIncremental", "Инкрементальная выгрузка");
                    this.AddIfHasValue<DateTime?>(displayParams, taskParams, "StartEditDate", "Дата с");
                    this.AddIfHasValue<DateTime?>(displayParams, taskParams, "EndEditDate", "Дата по");
                    this.AddIfHasValue<int?>(displayParams, taskParams, "MaxFileSize", "Максимальный размер архива (МБ)");
                    this.AddIfHasValue<bool?>(displayParams, taskParams, "IsSeparateArch", "Отдельная выгрузка файлов");
                    this.AddIfHasValue<bool?>(displayParams, taskParams, "NoEmptyMandatoryFields", "Не выгружать секции с ошибками");
                    this.AddIfHasValue<bool?>(displayParams, taskParams, "OnlyExistsFiles", "Не выгружать ссылки на отсутствующие файлы");
                    this.AddIfHasValue<bool?>(displayParams, taskParams, "WithoutAttachment", "Не актуализировать вложения");
                    this.AddIfHasValue(displayParams, periodName, "Расчетный период");
                    this.AddIfHasValues(displayParams, taskParams, "PersAccList", "Id выбранных лицевых счетов");
                    this.AddIfHasValues(displayParams, taskParams, "ProgramVersionList", "Id выбранных версий ДПКР");
                    this.AddIfHasValues(displayParams, taskParams, "ProgramCrList", "Id выбранных программ КР");
                    this.AddIfHasValues(displayParams, taskParams, "ObjectCrList", "Id выбранных объектов КР");
                    this.AddIfHasValues(displayParams, taskParams, "RealityObjectList", "Id выбранных домов");
                }

                var taskResult = new
                {
                    task.Id,
                    EntityGroupCodeList = entityGroupCodeList.Select(x => x.Code).ToArray(),
                    task.BaseParams,
                    DisplayParams = displayParams
                };

                return new BaseDataResult(taskResult);
            }
        }

        private void AddIfHasValue(DynamicDictionary dict, string paramValue, string displayName)
        {
            if (!string.IsNullOrEmpty(paramValue))
            {
                dict.Add(displayName, paramValue);
            }
        }

        private void AddIfHasValue<T>(DynamicDictionary dict, BaseParams baseParams, string paramName, string displayName, T defaultValue = default (T))
        {
            var value = baseParams.Params.Get(paramName, defaultValue);
            if (value != null)
            {
                dict.Add(displayName, value);
            }
        }

        private void AddIfHasValues(DynamicDictionary dict, BaseParams baseParams, string paramName, string displayName)
        {
            var value = baseParams.Params.GetAs<long[]>(paramName);
            if (value.IsNotEmpty())
            {
                dict.Add(displayName, value.AggregateWithSeparator(x => x.ToStr(), ", "));
            }
        }

        private string GetTriggerName(TaskPeriodType periodType,
            int startTimeHour,
            int startTimeMinutes,
            IList<byte> startDayOfWeekList,
            IList<byte> startMonthList)
        {
            switch (periodType)
            {
                case TaskPeriodType.NoPeriodicity:
                    return $"Одноразовый запуск. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Daily:
                    return $"Ежедневно. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Weekly:
                    return $"Еженедельно: {startDayOfWeekList.AggregateWithSeparator(this.GetDayOfWeeName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Monthly:
                    return $"Ежемесячно: {startMonthList.AggregateWithSeparator(this.GetMonthName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                default:
                    throw new InvalidEnumArgumentException(nameof(periodType), (int)periodType, periodType.GetType());
            }
        }

        private string GetDayOfWeeName(byte dayOfWeek)
        {
            if (dayOfWeek > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }

            // 1 января 1 г. н.э. был понедельник
            return new DateTime(1, 1, dayOfWeek).ToString("ddd");
        }

        private string GetMonthName(byte monthNumber)
        {
            return new DateTime(1, monthNumber, 1).ToString("MMMM");
        }

        private IList<string> GetGroupNameList(IList<string> entityGroupCodeList, IDictionary<string, string> entityGroupDict)
        {
            return entityGroupCodeList
                .Where(entityGroupDict.ContainsKey)
                .Select(x => entityGroupDict[x])
                .ToList();
        }
    }
}