namespace Bars.GkhGji.Interceptors.SurveyPlan
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using DomainService.SurveyPlan;
    using Entities;
    using Entities.SurveyPlan;
    /// <summary>
    /// Перехватчик для контрагент плана проверки
    /// </summary>
    public class SurveyPlanContragentInterceptor : EmptyDomainInterceptor<SurveyPlanContragent>
    {
    
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(
            IDomainService<SurveyPlanContragent> service,
            SurveyPlanContragent entity)
        {
            TryDeleteInspection(service, entity);

            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(
            IDomainService<SurveyPlanContragent> service,
            SurveyPlanContragent entity)
        {
            if (!CanHaveInspection(entity))
            {
                TryDeleteInspection(service, entity);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        private bool CanHaveInspection(SurveyPlanContragent contragent)
        {
            if (contragent.IsExcluded)
            {
                return false;
            }

            var service = Container.ResolveDomain<SurveyPlan>();
            try
            {
                return
                    service.GetAll()
                           .Where(x => x.Id == contragent.SurveyPlan.Id)
                           .Select(x => x.State.FinalState)
                           .FirstOrDefault();
            }
            finally
            {
                Container.Release(service);
            }
        }


        private void TryDeleteInspection(IDomainService<SurveyPlanContragent> service, SurveyPlanContragent surveyPlanContragent )
        {
            var contragents = service.GetAll()
                .Where(x => x.Contragent.Id == surveyPlanContragent.Contragent.Id )
                .Where(x => x.Id != surveyPlanContragent.Id)
                .Where(x => surveyPlanContragent.SurveyPlan.Id == x.SurveyPlan.Id)
                .Where(x => !x.IsExcluded);

            if (!contragents.Any())
            {
                var inspection =
                    service.GetAll().Where(x => x.Id == surveyPlanContragent.Id).Select(x => x.Inspection).FirstOrDefault();
                if (inspection == null)
                {
                    return;
                }

                var documentDomain = Container.ResolveDomain<DocumentGji>();
                var inspectionDomain = Container.ResolveDomain<InspectionGji>();
                try
                {
                    if (documentDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspection.Id)
                        .Any(x => x.State.FinalState))
                    {
                        throw new ValidationException(
                            "Для контрагента создана проверка, имеющая связанные документы в конечном статусе. Смените их статус и повторите попытку.");
                    }

                    surveyPlanContragent.Inspection = null;
                    inspectionDomain.Delete(inspection.Id);
                }
                finally
                {
                    Container.Release(documentDomain);
                    Container.Release(inspectionDomain);
                }
            }
        }
    }
}