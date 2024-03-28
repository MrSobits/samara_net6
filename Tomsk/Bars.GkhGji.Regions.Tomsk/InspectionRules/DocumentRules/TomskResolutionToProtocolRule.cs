namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Протокола' из документа 'Постановление'
    /// </summary>
    public class TomskResolutionToProtocolRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ProtocolViolation> ProtocolViolationDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "ResolutionToProtocolRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Протокола' из документа 'Постановление'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.ProtocolGji"; }
        }

        public string ResultName
        {
            get { return "Протокол"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Resolution; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Protocol; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var resolution = ResolutionDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new { x.Executant, x.Contragent, x.PhysicalPerson, x.PhysicalPersonInfo })
                                    .FirstOrDefault();

            if (resolution == null)
            {
                throw new Exception("Неудалось получить постановление");
            }

            var protocol = new Protocol()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = resolution.Contragent,
                Executant = resolution.Executant,
                PhysicalPerson = resolution.PhysicalPerson,
                PhysicalPersonInfo = resolution.PhysicalPersonInfo
            };
            #endregion

            #region Формируем этап протокола
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Protocol);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Protocol,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап чтобы потом незабыть сохранить 
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

            #region Формируем Инспекторов тянем их из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
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

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
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

        public IDataResult ValidationRule(DocumentGji document)
        {
            // протокол можно сформировать из постановления только в том случае, если
            // Постановление сформировано из АД и если вид санкции = Прекращено

            var sanction = ResolutionDomain.GetAll().Where(x => x.Id == document.Id).Select(x => x.Sanction).FirstOrDefault();

            if (sanction == null)
            {
                return new BaseDataResult(false, "Неуказана санкция");
            }

            if (sanction.Code != "2")
            {
                return new BaseDataResult(false, string.Format("Невозможно сформировать административное дело из постановления, у которого вид санкции {0}. Необходимо указать вид санкции: Прекращено", sanction.Name));
            }

            if (!ChildrenDomain.GetAll()
                              .Any(
                                  x =>
                                  x.Children.Id == document.Id
                                  && x.Parent.TypeDocumentGji == TypeDocumentGji.AdministrativeCase))
            {
                return new BaseDataResult(false, "Невозможно сформировать административное дело из постановления не врамках административного дела");
            }
            
            return new BaseDataResult();
        }
    }
}
