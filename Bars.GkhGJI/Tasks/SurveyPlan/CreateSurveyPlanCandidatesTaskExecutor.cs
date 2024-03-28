namespace Bars.GkhGji.Tasks.SurveyPlan
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService.SurveyPlan;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Entities.SurveyPlan;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class CreateSurveyPlanCandidatesTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SurveyPlanCandidate> CandidateDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        public IDataResult Execute(
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var code = @params.Params.GetAs("code", string.Empty);
            if (string.IsNullOrEmpty(code))
            {
                return BaseDataResult.Error("Не передан код цели");
            }

            var strategies = Container.ResolveAll<ISurveyPlanStrategy>();
            try
            {
                var strategy = strategies.FirstOrDefault(x => x.Code == code);
                if (strategy == null)
                {
                    indicator.Indicate(null, 100, "Не найдена стратегия для цели");
                    return new BaseDataResult();
                }

                if (strategy.Purpose == null)
                {
                    indicator.Indicate(null, 100, "Не найдена цель для стратегии");
                    return new BaseDataResult();
                }

                indicator.Indicate(null, 0, "Очистка старых записей");
                Cleanup(strategy.Purpose);

                indicator.Indicate(null, 0, "Формирование кандидатов для цели");
                CreateCandidates(strategy);

                indicator.Indicate(null, 100, "Формирование кандидатов для цели завершено");
                return new BaseDataResult();
            }
            finally
            {
                foreach (var strategy in strategies)
                {
                    Container.Release(strategy);
                }
            }
        }

        public string ExecutorCode { get; set; }

        private void Cleanup(AuditPurposeGji purpose)
        {
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
            session.CreateSQLQuery("DELETE FROM GJI_SURV_PLAN_CAND WHERE AUDIT_PURPOSE_ID = :purpose_id")
                   .SetInt64("purpose_id", purpose.Id)
                   .ExecuteUpdate();
        }

        private void CreateCandidates(ISurveyPlanStrategy strategy)
        {
            TransactionHelper.InsertInManyTransactions(
                Container,
                strategy.CreatePlanItems()
                        .Select(
                            item =>
                            new SurveyPlanCandidate
                                {
                                    AuditPurpose = item.AuditPurpose,
                                    Contragent = item.Contragent,
                                    PlanMonth = item.PlanMonth,
                                    PlanYear = item.PlanYear,
                                    Reason = item.Reason
                                }));
        }
    }
}