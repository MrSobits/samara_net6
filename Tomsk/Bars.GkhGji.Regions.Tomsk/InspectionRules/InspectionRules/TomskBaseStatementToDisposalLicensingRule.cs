namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

	/// <summary>
	/// Правило создания документа Распоряжения лицензирование из основания проверки по обращению
	/// </summary>
	public class TomskBaseStatementToDisposalLicensingRule : IInspectionGjiRule
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Интерфейс для описания тектовых значений Распоряжения
		/// </summary>
		public IDisposalText DisposalTextService { get; set; }

		/// <summary>
		/// Домен сервис для Рапоряжение ГЖИ
		/// </summary>
		public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Домен сервис для Этап проверки ГЖИ
		/// </summary>
		public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

		/// <summary>
		/// Домен сервис для Инспектора в проверке
		/// </summary>
		public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

		/// <summary>
		/// Домен сервис для Инспекторы документа ГЖИ
		/// </summary>
		public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

		/// <summary>
		/// Домен сервис для Обращение граждан проверки по обращениям граджан
		/// </summary>
		public IDomainService<InspectionAppealCits> StatementDomain { get; set; }

		/// <summary>
		/// Домен сервис для Связь обращения граждан с проверкой
		/// </summary>
		public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

		/// <summary>
		/// Код региона
		/// </summary>
        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

		/// <summary>
		/// Идентификатор правила
		/// </summary>
        public string Id
        {
            get { return "BaseStatementToDisposalLicensingRule"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public string Description
        {
            get { return "Правило создание документа Распоряжения лицензирование из основания проверки по обращению"; }
        }

		/// <summary>
		/// Контроллер
		/// </summary>
        public string ActionUrl
        {
            get { return "B4.controller.Disposal"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public string ResultName
        {
            get { return "Приказ лицензирование"; }
        }

		/// <summary>
		/// Инициатор типа инспекции
		/// </summary>
        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.CitizenStatement; }
        }

		/// <summary>
		/// Результат типа документа
		/// </summary>
        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Disposal; }
        }

		/// <summary>
		/// Установить параметры
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		public void SetParams(BaseParams baseParams)
        {
			// тут надо принять параметры если таковые имеютя
			// обработка пользовательских параметров не требуется
		}

		/// <summary>
		/// Создать документ
		/// </summary>
		/// <param name="inspection">Инспекция</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем распоряжение распоряжение
            var disposal = new Disposal()
            {
                Inspection = inspection,
                TypeDisposal = TypeDisposalGji.Licensing,
                TypeDocumentGji = TypeDocumentGji.Disposal,
                TypeAgreementProsecutor = TypeAgreementProsecutor.NotSet,
                TypeAgreementResult = TypeAgreementResult.NotSet
            };
            #endregion

            #region Формируем этап проверки
            var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var stage = new InspectionGjiStage
            {
                Inspection = inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = TypeStage.Disposal
            };

            disposal.Stage = stage;
            #endregion

            #region Проставляем вид проверки
            var rules = this.Container.ResolveAll<IKindCheckRule>().OrderBy(x => x.Priority);

            foreach (var rule in rules)
            {
                if (rule.Validate(disposal))
                {
                    var replace = this.Container.Resolve<IDomainService<KindCheckRuleReplace>>().GetAll()
                                     .FirstOrDefault(x => x.RuleCode == rule.Code);

                    var serviceKindCheck = this.Container.Resolve<IDomainService<KindCheckGji>>();

                    disposal.KindCheck = replace != null
                                           ? serviceKindCheck.GetAll().FirstOrDefault(x => x.Code == replace.Code)
                                           : serviceKindCheck.GetAll().FirstOrDefault(x => x.Code == rule.DefaultCode);
                }
            }
            #endregion

            #region Забираем инспекторов из основания и переносим в Распоряжение

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.InspectionInspectorDomain.GetAll().Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var inspector in inspectorIds)
            {
                var newInspector = new DocumentGjiInspector
                {
                    DocumentGji = disposal,
                    Inspector = new Inspector { Id = inspector }
                };

                listInspectors.Add(newInspector);
            }

            // инспектора из обращения добавляем к инспекторам в Распоряжение
            var primaryAppealCitsData = this.PrimaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == disposal.Inspection.Id)
                .Select(x => x.BaseStatementAppealCits.AppealCits.Executant)
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (primaryAppealCitsData != null)
            {
                disposal.ResponsibleExecution = primaryAppealCitsData;
                listInspectors.Add(new DocumentGjiInspector { DocumentGji = disposal, Inspector = primaryAppealCitsData });
            }

            inspectorIds.AddRange(this.StatementDomain.GetAll()
                             .Where(x => x.Inspection.Id == inspection.Id && x.AppealCits.Tester != null)
                             .Select(x => x.AppealCits.Tester.Id)
                             .AsEnumerable().Distinct().ToArray());

            var head = this.StatementDomain
                             .GetAll()
                             .Where(x => x.Inspection.Id == inspection.Id)
                             .Select(x => x.AppealCits.Surety)
                             .FirstOrDefault();

            disposal.IssuedDisposal = head;
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.DisposalDomain.Save(disposal);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = disposal.Id, inspectionId = inspection.Id });
        }

		/// <summary>
		/// Провалидировать правило
		/// </summary>
		/// <param name="inspection">Инспекция</param>
		/// <returns>Результа выполнения запроса</returns>
        public IDataResult ValidationRule(InspectionGji inspection)
        {
            /*
             тут проверяем, если у проверки уже есть дкоумент Распоряжение, то нельзя больше создавать
            */
            if (inspection != null)
            {
                if (this.DisposalDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
                {
                    return new BaseDataResult(false, "По плановой проверке юр. лиц уже создано распоряжение");
                }
            }

            return new BaseDataResult();
        }
    }
}
