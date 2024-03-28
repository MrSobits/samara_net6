namespace Bars.GisIntegration.Inspection.DataExtractors.InspectionPlan
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Экстрактор данных по плановым проверкам юридического лица
    /// </summary>
    public class InspectionPlanExaminationExtractor : BaseDataExtractor<Examination, BaseJurPerson>
    {
        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        private List<InspectionPlan> plans;
        private Dictionary<long, InspectionPlan> plansById;
        private IDictionary baseNsi;
        private IDictionary formNsi;
        private Dictionary<long, string> parentDispTypeSurvGoalByInspId;
        private Dictionary<long, RisContragent> risContragentsByGkhIds;
        private Dictionary<long, Disposal> disposalByInspectionId;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.formNsi = this.DictionaryManager.GetDictionary("ExaminationJurPersonFormDictionary");
            this.baseNsi = this.DictionaryManager.GetDictionary("TypeBaseJurPersonDictionary");

            this.plans = parameters.GetAs<List<InspectionPlan>>("selectedPlans");

            this.plansById = this.plans?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            var disposalDomain = this.Container.ResolveDomain<Disposal>();
            var disposalTypeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var childDocDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var typeSurvGoalInspGjiDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var contragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                this.disposalByInspectionId = disposalDomain.GetAll()
                    .Where(x => x.Inspection != null)
                    .Select(x => new
                    {
                        InspectionId = x.Inspection.Id,
                        Disposal = x
                    })
                    .ToList()
                    .GroupBy(x => x.InspectionId)
                    .ToDictionary(x => x.Key, x => x.First().Disposal);

                this.risContragentsByGkhIds = contragentDomain.GetAll()
                    .GroupBy(x => x.GkhId)
                    .ToDictionary(x => x.Key, x => x.First());

                var childDisposalIds = childDocDomain.GetAll()
                         .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                         .Select(x => x.Children.Id)
                         .Distinct();

                this.parentDispTypeSurvGoalByInspId = disposalTypeSurveyDomain.GetAll()
                   .Where(x => childDisposalIds.All(y => y != x.Disposal.Id))
                   .Join(typeSurvGoalInspGjiDomain.GetAll(),
                       dispTypeSurv => dispTypeSurv.TypeSurvey.Id,
                       typeSurvGoal => typeSurvGoal.TypeSurvey.Id,
                       (dispTypeSurv, typeSurvGoal) => new
                       {
                           InspectionId = dispTypeSurv.Disposal.Inspection.Id,
                           Goal = typeSurvGoal.SurveyPurpose.Name
                       })
                   .ToList()
                   .GroupBy(x => x.InspectionId)
                   .ToDictionary(x => x.Key, x => x.Select(y => y.Goal).First());
            }
            finally 
            {
                this.Container.Release(contragentDomain);
                this.Container.Release(childDocDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(typeSurvGoalInspGjiDomain);
            }
        }
       
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<BaseJurPerson> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedPlanIds = this.plans?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[] { };
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();

            try
            {
                return baseJurPersonDomain.GetAll()
                    .Where(x => x.Plan != null && x.Contragent != null)
                    .Where(x => selectedPlanIds.Contains(x.Plan.Id))
                    .Where(x => x.TypeFact == TypeFactInspection.NotDone) //Передавать нужно только те проверки, у которых значение поля "Факт проверки" = "Не проведена"
                    .ToList();
            }
            finally
            {
                this.Container.Release(baseJurPersonDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(BaseJurPerson externalEntity, Examination risEntity)
        {
            var examinationBase = this.baseNsi.GetDictionaryRecord((long)externalEntity.TypeBaseJuralPerson);
            var examinationForm = this.formNsi.GetDictionaryRecord((long)externalEntity.TypeForm);
            var contragent = this.risContragentsByGkhIds?.Get(externalEntity.Contragent.Id);
            var disposal = this.disposalByInspectionId?.Get(externalEntity.Id);

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.IsScheduled = true;
            risEntity.InspectionPlan = this.plansById?.Get(externalEntity.Plan.Id);
            risEntity.InspectionNumber = (externalEntity.InspectionNumber.Split("-")?[1] ?? "0").ToInt();
            risEntity.UriRegistrationNumber = externalEntity.UriRegistrationNumber;
            risEntity.UriRegistrationDate = externalEntity.UriRegistrationDate;
            risEntity.Objective = this.parentDispTypeSurvGoalByInspId.Get(externalEntity.Id);
            risEntity.BaseCode = examinationBase?.GisCode;
            risEntity.BaseGuid = examinationBase?.GisGuid;
            risEntity.From = externalEntity.DateStart;
            risEntity.Duration = externalEntity.CountDays.ToDouble();
            risEntity.ExaminationFormCode = examinationForm?.GisCode;
            risEntity.ExaminationFormGuid = examinationForm?.GisGuid;
            risEntity.GisContragent = contragent;
            risEntity.ProsecutorAgreementInformation = disposal?.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement
                ? disposal.TypeAgreementResult.GetDisplayName()
                : string.Empty;
        }
    }
}
