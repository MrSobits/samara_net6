namespace Bars.GkhGji.ViewModel.SurveyPlan
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities.SurveyPlan;
    using Castle.MicroKernel.ModelBuilder.Descriptors;

    /// <summary>
    /// Вью модели предрассчитанныы кандидатов на добавление в план
    /// </summary>
    public class SurveyPlanCandidateViewModel : BaseViewModel<SurveyPlanCandidate>
    {
        /// <summary>
        /// Доме сервис Контрагент плана проверки
        /// </summary>
        public IDomainService<SurveyPlanContragent> surveyPlan { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<SurveyPlanCandidate> domainService, BaseParams baseParams)
        {
            var purposeId = baseParams.Params.GetAsId("purposeId");
            var moId = baseParams.Params.GetAsId("moId");
            var okopfId = baseParams.Params.GetAsId("okopfId");
            var year = baseParams.Params.GetAs("year", 0);
            var surveyPlanId = baseParams.Params.GetAsId("surveyPlanId");
            var loadParams = GetLoadParam(baseParams);

            var existingContragents = surveyPlan.GetAll()
                .Where(x => x.SurveyPlan.Id == surveyPlanId)
                .Select(x => new
                {
                    ContragentId = x.Contragent.Id,
                    AuditPurposeId = x.AuditPurpose.Id
                });

            var result =
                domainService.GetAll()
                             .WhereIf(purposeId > 0, x => x.AuditPurpose.Id == purposeId)
                             .WhereIf(moId > 0, x => x.Contragent.Municipality.Id == moId)
                             .WhereIf(okopfId > 0, x => x.Contragent.OrganizationForm.Id == okopfId)
                             .WhereIf(year > 0, x => x.PlanYear == year)
                             .Where(x => !existingContragents.Any(y => y.ContragentId == x.Contragent.Id && y.AuditPurposeId == x.AuditPurpose.Id))
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         Municipality = x.Contragent.Municipality.Name,
                                         x.Contragent.JuridicalAddress,
                                         x.Contragent.Name,
                                         x.Contragent.Phone,
                                         x.Contragent.Inn,
                                         x.Contragent.Kpp,
                                         x.PlanMonth,
                                         x.PlanYear,
                                         AuditPurpose = x.AuditPurpose.Name,
                                         x.Reason
                                     })
                             .Filter(loadParams, Container)
                             .Order(loadParams);

            return new ListDataResult(result.Paging(loadParams), result.Count());
        }
    }
}