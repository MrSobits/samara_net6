namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа 'Протокол' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToProtocolRule : BaseActCheckToProtocolRule<ActCheck, Protocol>
    {
    }

    /// <summary>
    /// Базовый класс правила создания документа 'Протокол' из документа 'Акт проверки'
    /// </summary>
    /// <typeparam name="TActCheck"></typeparam>
    /// <typeparam name="TProtocol"></typeparam>
    public class BaseActCheckToProtocolRule<TActCheck, TProtocol> : IDocumentGjiRule
        where TActCheck : ActCheck
        where TProtocol : Protocol, new()
    {
        #region Домен-сервисы
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для документа <see cref="ActCheck" />, или его наследника
        /// </summary>
        public IDomainService<TActCheck> ActCheckDomain { get; set; }

        /// <summary>
        /// Домен-сервис для документа <see cref="Protocol" />, или его наследника
        /// </summary>
        public IDomainService<TProtocol> ProtocolDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="InspectionGjiStage" />
        /// </summary>
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="DocumentGjiInspector" />
        /// </summary>
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="ActSurveyRealityObject" />
        /// </summary>
        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="DocumentGjiChildren" />
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="InspectionGjiViol" />
        /// </summary>
        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="ProtocolViolation" />
        /// </summary>
        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }
        #endregion

        #region Внутренние переменные
        private long[] ViolationIds { get; set; }
        #endregion

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => "ActCheckToProtocolRule";

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Протокол' из документа 'Акт проверки' (по выбранным нарушениям)";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.ProtocolGji";

        /// <inheritdoc />
        public string ResultName => "Протокол";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActCheck;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Protocol;

        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр violationIds выбранных пользователем нарушений
            var violationIds = baseParams.Params.GetAs("violationIds", "");

            this.ViolationIds = !string.IsNullOrEmpty(violationIds)
                                  ? violationIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : Array.Empty<long>();

            if (this.ViolationIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать нарушения");
            }
        }

        /// <inheritdoc />
        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var actCheck = this.ActCheckDomain.Get(document.Id);

            if (actCheck == null)
            {
                throw new Exception("Не удалось получить акт проверки");
            }

            var protocol = new TProtocol()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = document.Inspection.Contragent
            };

            this.ProtocolAdditionalProcessing(actCheck, protocol);
            #endregion

            #region Формируем этап протокола
            // Если у родительского документа есть этап, у которого есть родитель
            // то берем именно родительский этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage?.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Protocol);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Protocol,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап, чтобы потом не забыть сохранить 
                newStage = currentStage;
            }

            protocol.Stage = currentStage;
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = protocol
            };
            #endregion

            #region Формируем инспекторов, тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = protocol,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем нарушения (по выбранным пользователями)
            var listProtocolViol = new List<ProtocolViolation>();
            List<InspectionGjiViol> violationList = new List<InspectionGjiViol>();
            if (ViolationIds != null)
                violationList = this.InspectionViolDomain.GetAll().Where(x => this.ViolationIds.Contains(x.Id)).ToList();
            
            var violations = this.InspectionViolDomain.GetAll().Where(x => this.ViolationIds.Contains(x.Id));

            foreach (var viol in violations)
            {
                var newRecord = new ProtocolViolation
                {
                    Document = protocol,
                    InspectionViolation = viol,
                    DatePlanRemoval = viol.DatePlanRemoval,
                    DateFactRemoval = viol.DateFactRemoval,
                    TypeViolationStage = TypeViolationStage.Sentence
                };

                listProtocolViol.Add(newRecord);
            }
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ProtocolDomain.Save(protocol);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listProtocolViol.ForEach(x => this.ProtocolViolationDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = protocol.Id, inspectionId = document.Inspection.Id });
        }

        /// <summary>
        /// Дополнительная обработка протокола
        /// </summary>
        /// <param name="document">Родильский документ формируемого протокола</param>
        /// <param name="protocol">Протокол, который нужно дополнительно обработать</param>
        protected virtual void ProtocolAdditionalProcessing(TActCheck document, TProtocol protocol)
        {
        }

        /// <inheritdoc />
        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            if (document.Inspection.TypeBase == TypeBase.GjiWarning)
            {
                return new BaseDataResult(false, "");
            }

            return new BaseDataResult();
        }
    }
}
