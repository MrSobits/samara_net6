namespace Bars.GisIntegration.UI.ViewModel.Result
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Entities;

    using Castle.Windsor;

    /// <summary>
    /// View - модель результатов обработки объектов
    /// </summary>
    public class ObjectProcessingResultViewModel: IObjectProcessingResultViewModel
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список записей протокола
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// triggerId - идентификатор триггера,
        /// или
        /// packageId - идентификатор связки триггера и пакета</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список записей протокола</returns>
        public IDataResult List(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");
            var packageId = baseParams.Params.GetAs<long>("packageId");

            if (triggerId == 0 && packageId == 0)
            {
                return new BaseDataResult(false, "Пустой идентификатор триггера и пакета.");
            }

            List<RisPackageTrigger> triggerPackages = new List<RisPackageTrigger>();

            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                if (packageId > 0)
                {
                    var packageTrigger = taskManager.GetTriggerPackage(packageId);

                    if (packageTrigger == null)
                    {
                        return new BaseDataResult(false, string.Format("Не найден пакет триггера с идентификатором {0}", packageId));
                    }

                    triggerPackages.Add(packageTrigger);
                }
                else
                {
                    triggerPackages.AddRange(taskManager.GetTriggerPackages(triggerId));
                }
            }
            finally
            {
                this.Container.Release(taskManager);
            }

            var resultData = this.GetObjectProcessingResults(triggerPackages);

            if (resultData.Count > 0)
            {
                var loadParams = baseParams.GetLoadParam();

                var data = resultData
                   .AsQueryable()
                   .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }

            return new ListDataResult(null, 0);
        }

        private List<ObjectProcessingResultView> GetObjectProcessingResults(List<RisPackageTrigger> packageTriggers)
        {
            var result = new List<ObjectProcessingResultView>();

            foreach (var packageTrigger in packageTriggers.OrderBy(x => x.Id))
            {
                var objectProcessingResults = packageTrigger.GetProcessingResult();

                result.AddRange(objectProcessingResults.OrderBy(x => x.RisId).Select(x => new ObjectProcessingResultView
                {
                    PackageName = packageTrigger.Package.Name ?? string.Empty,
                    RisId = x.RisId,
                    ExternalId = x.ExternalId,
                    GisId = x.GisId ?? string.Empty,
                    Description = x.Description ?? string.Empty,
                    State = x.State,
                    Message = x.Message ?? string.Empty
                }));
            }

            return result;
        }
    }
}
