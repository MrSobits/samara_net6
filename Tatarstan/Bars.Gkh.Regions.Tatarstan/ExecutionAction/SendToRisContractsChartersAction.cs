namespace Bars.Gkh.Regions.Tatarstan.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.FormatDataExport.Tasks;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Действия по отправке ДУ/уставов в РИС ЖКХ
    /// </summary>
    public class SendToRisContractsChartersAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "РИС ЖКХ - Экспорт договоров управления и уставов в РИС ЖКХ";

        /// <inheritdoc />
        public override string Description => "В рамках действия имеется возможность отправки всех договоров и уставов по УК / ТСЖ / ЖСК , либо измененных за указанный в настройках период";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Период изменения сущности, в пределах которого получать данные
        /// </summary>
        private int period;
        
        /// <summary>
        /// Флаг очистки файлов после загрузки
        /// </summary>
        private bool cleanFiles;

        /// <summary>
        /// Тип экспорта ДУ/Уставов в РИС
        /// </summary>
        private ChartersContractsRisExportType exportType;
        
        private BaseDataResult Execute()
        {
            var records = new List<DynamicDictionary>();
            try
            {
                this.exportType = ExecutionParams.Params.GetAs<ChartersContractsRisExportType>("ChartersContractsRisExportType");
                this.period = ExecutionParams.Params.GetAs<int>("Period");
                this.cleanFiles = ExecutionParams.Params.GetAs<bool>("CleanFiles");

                var taskExecutor = this.Container.Resolve<ITaskExecutor>(FormatDataExportTaskExecutor.Id);
                var formatDataExportTask = this.Container.Resolve<IDomainService<FormatDataExportTask>>();
                using (this.Container.Using(taskExecutor, formatDataExportTask))
                {
                    GetEntityIds(true)
                        .ForEach(x => { records.Add(PrepareParams(x, "DuEntityGroup")); });

                    GetEntityIds(false)
                        .ForEach(x => { records.Add(PrepareParams(x, "UstavEntityGroup")); });

                    ExecutionParams.Params["records"] = records;
                    formatDataExportTask.Save(ExecutionParams);
                }

                return new BaseDataResult(true, $"Поставлено {records.Count} задач по экспорту ДУ/уставов в РИС ЖКХ.");
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, $"Произошла ошибка при постановке задач: {ex.Message} {ex.InnerException?.Message ?? null}");
            }
        }

        private DynamicDictionary PrepareParams(KeyValuePair<long, List<long>> exportData, string entityGroup)
        {
            DynamicDictionary additionalParams = new DynamicDictionary(),
                BaseParams = new DynamicDictionary(),
                Params = new DynamicDictionary(),
                InspectionFilter = new DynamicDictionary(),
                PersAccFilter = new DynamicDictionary(),
                ProgramVersionFilter = new DynamicDictionary(),
                ObjectCrFilter = new DynamicDictionary(),
                DuUstavFilter = new DynamicDictionary(),
                RealityObjectFilter = new DynamicDictionary();
            var entityGroupList = new List<string> { entityGroup };


            Params.Add("MainContragent", exportData.Key);
            Params.Add("DuUstavList", exportData.Value);
            Params.Add("InspectionFilter", InspectionFilter);
            Params.Add("PersAccFilter", PersAccFilter);
            Params.Add("ProgramVersionFilter", ProgramVersionFilter);
            Params.Add("ObjectCrFilter", ObjectCrFilter);
            Params.Add("DuUstavFilter", DuUstavFilter);
            Params.Add("RealityObjectFilter", RealityObjectFilter);
            Params.Add("CleanFiles", this.cleanFiles);
            BaseParams.Add("Params", Params);

            additionalParams.Add("StartNow", true);
            additionalParams.Add("EntityGroupCodeList", entityGroupList);
            additionalParams.Add("BaseParams", BaseParams);
            additionalParams.Add("MainContragent", exportData.Key);

            return additionalParams;
        }

        /// <summary>
        /// Получить словарь Id контрагента - Id ДУ/Уставов для экспорта
        /// </summary>
        /// <param name="extractContracts">Флаг типа сущности (true - ДУ, false - Уставы)</param>
        /// <returns></returns>
        private Dictionary<long, List<long>> GetEntityIds(bool extractContracts)
        {
            var manOrgBaseContractService = this.Container.ResolveDomain<ManOrgBaseContract>();
            var formatExportTaskResultService = this.Container.ResolveDomain<FormatDataExportResult>();
            var allowedTypeManagementArray = extractContracts
                ? new[] { TypeManagementManOrg.UK }
                : new[] { TypeManagementManOrg.TSJ, TypeManagementManOrg.JSK };

            using (this.Container.Using(manOrgBaseContractService, formatExportTaskResultService))
            {
                var processedObjectsDict = new Dictionary<long, DateTime>();

                if (this.exportType == ChartersContractsRisExportType.ChangedChartersContracts)
                {
                    processedObjectsDict = formatExportTaskResultService.GetAll()
                        .Where(x =>
                            x.Status == FormatDataExportStatus.Successfull &&
                            x.EndDate.HasValue &&
                            x.EndDate.Value > DateTime.Now.AddDays(-this.period) &&
                            x.Task.EntityGroupCodeList != null
                        )
                        .AsEnumerable()
                        .Where(x => x.Task.EntityGroupCodeList.Contains(extractContracts ? "DuEntityGroup" : "UstavEntityGroup"))
                        .SelectMany(x =>
                        {
                            return x.Task.BaseParams.Params.GetAs<long[]>("DuUstavList")
                                .Select(y => new
                                {
                                    Id = y,
                                    EndDate = x.EndDate.Value
                                });    
                        })
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => x.Max(y => y.EndDate));
                }

                return manOrgBaseContractService.GetAll()
                    .WhereIf(exportType == ChartersContractsRisExportType.ChangedChartersContracts,
                        x => DateTime.Now.AddDays(-this.period) < x.ObjectEditDate && DateTime.Now > x.ObjectEditDate)
                    .Where(x =>
                        allowedTypeManagementArray.Contains(x.ManagingOrganization.TypeManagement))
                    .Select(x => new
                    {
                        ContragentId = x.ManagingOrganization.Contragent.Id,
                        x.ObjectEditDate,
                        x.Id
                    })
                    .AsEnumerable()
                    .WhereIf(processedObjectsDict.Any(), x => !(processedObjectsDict.ContainsKey(x.Id) && processedObjectsDict[x.Id] > x.ObjectEditDate))
                    .GroupBy(x => x.ContragentId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());
            }
        }
    }
}