namespace Bars.GkhGji.ViewModel.SurveyPlan
{
    using System.Linq;

    using B4;
    using Gkh.Domain;
    using Entities.SurveyPlan;

    /// <summary>
    /// Вью модель контрагент плана проверки
    /// </summary>
    public class SurveyPlanContragentViewModel : BaseViewModel<SurveyPlanContragent>
    {
        /// <summary>
        /// Получение данных для контрагента плана проверки
        /// </summary>
        /// <param name="domainService">Домен сервис</param>
        /// <param name="baseParams"> Базовые параметры</param>
        /// <returns></returns>
        public override IDataResult Get(IDomainService<SurveyPlanContragent> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var contragent = domainService.Get(id);
            if (contragent == null)
            {
                return BaseDataResult.Error("Передан неверный идентификатор контрагента");
            }

            return
                new BaseDataResult(
                    new
                        {
                            contragent.Id,
                            contragent.Contragent.Inn,
                            contragent.Contragent.Name,
                            contragent.Contragent.Ogrn,
                            contragent.Contragent.JuridicalAddress,
                            contragent.PlanMonth,
                            contragent.PlanYear,
                            contragent.IsExcluded,
                            contragent.ExclusionReason
                        });
        }

        /// <summary>
        /// Список плана проверки контрагентов
        /// </summary>
        /// <param name="domainService">Домен сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<SurveyPlanContragent> domainService, BaseParams baseParams)
        {
            var surveyPlanId = baseParams.Params.GetAsId("surveyPlanId");
            if (surveyPlanId < 1)
            {
                return BaseDataResult.Error("Не передан идентификатор плана проверки");
            }

            var loadParams = GetLoadParam(baseParams);
            var result =
                domainService.GetAll()
                             .Where(x => x.SurveyPlan.Id == surveyPlanId)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         Municipality = x.Contragent.Municipality.Name,
                                         x.IsExcluded,
                                         x.Contragent.JuridicalAddress,
                                         x.Contragent.Name,
                                         x.Contragent.Phone,
                                         x.Contragent.Inn,
                                         x.Contragent.Kpp,
                                         x.PlanMonth,
                                         x.PlanYear,
                                         AuditPurpose = x.AuditPurpose.Name,
                                         x.Reason,
                                         x.ObjectEditDate
                                     })
                             .Filter(loadParams, Container)
                             .Order(loadParams);

            return new ListDataResult(result.Paging(loadParams), result.Count());
        }
    }
}