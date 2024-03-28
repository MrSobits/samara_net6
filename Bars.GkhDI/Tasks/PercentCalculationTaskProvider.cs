namespace Bars.GkhDi.Tasks
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Задача по расчёту процентов на сервере расчётов
    /// </summary>
    public class PercentCalculationTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        public PercentCalculationTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            string description = string.Empty;

            var periodDiDomain = this.container.ResolveDomain<PeriodDi>();
            var municipalityDomain = this.container.ResolveDomain<Municipality>();
            var disclosureInfoDomain = this.container.ResolveDomain<DisclosureInfo>();

            using (this.container.Using(periodDiDomain, municipalityDomain, disclosureInfoDomain))
            {
                var municipalityIdList = baseParams.Params.GetAs("municipalityIds", string.Empty);
                var municipalityIds = !string.IsNullOrEmpty(municipalityIdList)
                    ? municipalityIdList.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var periodDiId = baseParams.Params.GetAs<long>("period");
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var period = periodDiDomain.Get(periodDiId);


                if (period != null && municipalityIds.Length != 0)
                {
                    description = "Массовый расчёт процентов";
                }
                else if (disclosureInfoId != 0)
                {
                    var disInfo = disclosureInfoDomain.Get(disclosureInfoId);

                    if (disInfo.InCalculation)
                    {
                        return new CreateTasksResult(new TaskDescriptor[0]);
                    }

                    var contragent = disInfo.ManagingOrganization.Contragent;

                    disInfo.IsCalculation = true;
                    disclosureInfoDomain.Update(disInfo);

                    description = $"{contragent.Name}, ИНН: {contragent.Inn}, КПП: {contragent.Kpp}";
                }
            }

            return new CreateTasksResult(new[]
            {
                new TaskDescriptor("Расчёт процентов Раскрытия информации", this.TaskCode, baseParams)
                {
                    Description = description,
                    Dependencies = new []
                    {
                        new Dependency
                        {
                            Key = this.TaskCode,
                            Scope = DependencyScope.InsideGlobalTasks
                        }
                    }
                }, 
            });
        }

        /// <inheritdoc />
        public string TaskCode => nameof(PercentCalculationTaskExecutor);
    }
}