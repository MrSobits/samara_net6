namespace Bars.GisIntegration.Smev.Dto
{
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// DTO <see cref="Decision"/> для создания тела запроса
    /// </summary>
    internal class DecisionCorrectionDto
    {
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// Идентификатор документа в ЕРКНМ
        /// </summary>
        public string IGuid { get; set; }

        /// <summary>
        /// Инспекция
        /// </summary>
        public InspectionUpdate InspectionUpdate { get; set; }

        /// <summary>
        /// Инспекция КНМ
        /// </summary>
        public InspectionKnmUpdate InspectionKnmUpdate { get; set; }

        /// <summary>
        /// Решение
        /// </summary>
        public IDecisionUpdate DecisionUpdate { get; set; }

        /// <summary>
        /// Необходимость согласования
        /// </summary>
        public IApproveUpdate ApproveUpdate { get; set; }

        /// <summary>
        /// Места проведения КНМ (не размещенные в ЕРКНМ)
        /// </summary>
        public IPlacesInsert[] DocumentPlacesToInsert { get; set; }

        /// <summary>
        /// Места проведения КНМ (размещенные в ЕРКНМ)
        /// </summary>
        public IPlacesUpdate[] DocumentPlacesToUpdate { get; set; }

        /// <summary>
        /// Объекты контроля
        /// </summary>
        public IObjectsInsert[] ObjectsInsert { get; set; }

        /// <summary>
        /// Типы объектов контроля
        /// </summary>
        public IObjectObjectTypeUpdate[] ObjectObjectTypeUpdate { get; set; }

        /// <summary>
        /// Виды объектов контроля
        /// </summary>
        public IObjectObjectKindUpdate[] ObjectObjectKindUpdate { get; set; }

        /// <summary>
        /// Категории риска объектов контроля
        /// </summary>
        public IObjectRiskCategoryUpdate[] ObjectRiskCategoryUpdate { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public IInspectorsInsert[] InspectorsInsert { get; set; }

        /// <summary>
        /// Должности инспекторов
        /// </summary>
        public IInspectorPositionUpdate[] InspectorPositionUpdate { get; set; }

        /// <summary>
        /// Организации
        /// </summary>
        public IOrganizationsUpdate[] OrganizationsUpdate { get; set; }

        /// <summary>
        /// Документы организаций (не размещенные в ЕРКНМ)
        /// </summary>
        public IOrganizationDocumentsInsert[] OrganizationDocumentsInsert { get; set; }

        /// <summary>
        /// Документы организаций (размещенные в ЕРКНМ)
        /// </summary>
        public IOrganizationDocumentsUpdate[] OrganizationDocumentsUpdate { get; set; }

        /// <summary>
        /// Основания проведения (не размещенные в ЕРКНМ)
        /// </summary>
        public IReasonsInsert[] ReasonsInsert { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public ReasonWithRiskReasonUpdate[] ReasonWithRiskReasonUpdate { get; set; }

        /// <summary>
        /// Индикаторы риска
        /// </summary>
        public ReasonWithRiskRiskIndikatorsUpdate[] ReasonWithRiskRiskIndikatorsUpdate { get; set; }

        /// <summary>
        /// Действия (не размещенные в ЕРКНМ)
        /// </summary>
        public IEventsInsert[] IEventsInsert { get; set; }

        /// <summary>
        /// Действия (размещенные в ЕРКНМ)
        /// </summary>
        public IEventsUpdate[] IEventsUpdate { get; set; }

        /// Эксперты (не размещенные в ЕРКНМ)
        /// </summary>
        public IExpertsInsert[] ExpertsInsert { get; set; }

        /// <summary>
        ///  Эксперты (размещенные в ЕРКНМ)
        /// </summary>
        public IExpertsUpdate[] ExpertsUpdate { get; set; }

        /// <summary>
        /// Основания проведения КНМ (не размещенные в ЕРКНМ)
        /// </summary>
        public IReasonDocumentsInsert[] ReasonDocumentsInsert { get; set; }

        /// <summary>
        /// Основания проведения КНМ (размещенные в ЕРКНМ)
        /// </summary>
        public IReasonDocumentsUpdate[] ReasonDocumentsUpdate { get; set; }

        /// <summary>
        /// Приложения к документу (не размещенные в ЕРКНМ)
        /// </summary>
        public InspectionDocumentsInsert[] InspectionDocumentAttachmentsToInsert { get; set; }
        
        /// <summary>
        /// Приложения к документу (размещенные в ЕРКНМ)
        /// </summary>
        public InspectionDocumentsUpdate[] InspectionDocumentAttachmentsToUpdate { get; set; }

        /// <summary>
        /// Акт проверки (не размещенный в ЕРКНМ)
        /// </summary>
        public ISubjectActInsert SubjectActInsert { get; set; }

        /// <summary>
        /// Акт проверки (размещенный в ЕРКНМ)
        /// </summary>
        public ISubjectActUpdate SubjectActUpdate { get; set; }

        /// <summary>
        /// Должностное лицо, подписавшее акт проверки
        /// </summary>
        public IActTitleSignerUpdate ActTitleSignerUpdate { get; set; }

        /// <summary>
        /// Инспекторы из акта проверки
        /// </summary>
        public IActKnoInspectorsInsert[] ActKnoInspectorsInsert { get; set; }

        /// <summary>
        /// Документ со вкладки "Приложения" связанного акта
        /// </summary>
        public IActDocumentInsert ActDocumentInsert { get; set; }

        /// <summary>
        /// Предписания и Протоколы (не размещенный в ЕРКНМ)
        /// </summary>
        public ISubjectResultDecisionsInsert[] SubjectResultDecisionsInsert { get; set; }

        /// <summary>
        /// Предписания и Протоколы (размещенные в ЕРКНМ)
        /// </summary>
        public ISubjectResultDecisionsUpdate[] SubjectResultDecisionsUpdate { get; set; }

        /// <summary>
        /// Документ со вкладки "Приложения" предписания или протокола
        /// </summary>
        public IResultDecisionDocumentInsert[] ResultDecisionDocumentInsert { get; set; }
        
        /// <summary>
        /// Предписания (размещенные в ЕРКНМ)
        /// </summary>
        public IResultDecisionInjunctionUpdate[] ResultDecisionInjunctionUpdate { get; set; }
        
        /// <summary>
        /// Идентифкаторы должностей инспекторов из размещённых в ЕРКНМ Протоколов и Предписаний
        /// </summary>
        public IResultDecisionTitleSignerUpdate[] ResultDecisionTitleSignerUpdate { get; set; }
        
        /// <summary>
        /// Инспекторы (не размещенные в ЕРКНМ) с их должностямми из размещённых докуметов Протокола и Предписания
        /// </summary>
        public IResultDecisionInspectorsInsert[] ResultDecisionInspectorsInsert { get; set; }
        
        /// <summary>
        /// Постановления (не размещенные в ЕРКНМ)
        /// </summary>
        public IResultDecisionResponsibleEntitiesInsert[] ResultDecisionResponsibleEntitiesInsert { get; set; }

        /// <summary>
        /// Постановления (размещенные в ЕРКНМ)
        /// </summary>
        public IResultDecisionResponsibleEntitiesUpdate[] ResultDecisionResponsibleEntitiesUpdate { get; set; }
        
        /// <summary>
        /// Статья закона со вкладки Протокол / Статьи закона (размещенные в ЕРКНМ)
        /// </summary>
        public IResponsibleSubjectStructuresNPAUpdate[] ResponsibleSubjectStructuresNpaUpdate { get; set; }
        
        /// <summary>
        /// Статья закона со вкладки Протокол / Статьи закона (не размещенные в ЕРКНМ)
        /// </summary>
        public IResponsibleSubjectStructuresNPAInsert[] ResponsibleSubjectStructuresNpaInsert { get; set; }
    }
}