namespace Bars.GkhGji.Tasks.SurveyPlan
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.DomainService.SurveyPlan;

    using Castle.Windsor;

    public class CreateSurveyPlanCandidatesTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer _container;

        public CreateSurveyPlanCandidatesTaskProvider(IWindsorContainer container)
        {
            _container = container;
        }

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var strategies = _container.ResolveAll<ISurveyPlanStrategy>();
            try
            {
                var tasks = new List<TaskDescriptor>();

                foreach (var strategy in strategies)
                {
                    var args = new BaseParams();
                    args.Params.SetValue("code", strategy.Code);
                    tasks.Add(
                        new TaskDescriptor(
                            string.Format(
                                "Формирование кандидатов для плановой проверки ГЖИ по цели с кодом \"{0}\"",
                                strategy.Code),
                            CreateSurveyPlanCandidatesTaskExecutor.Id,
                            args));
                }

                return new CreateTasksResult(tasks.ToArray());
            }
            finally
            {
                foreach (var strategy in strategies)
                {
                    _container.Release(strategy);
                }
            }
        }

        public string TaskCode
        {
            get
            {
                return "CreateSurveyPlanCandidates";
            }
        }
    }
}