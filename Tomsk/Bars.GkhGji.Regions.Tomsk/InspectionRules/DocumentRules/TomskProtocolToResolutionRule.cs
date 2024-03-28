namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Протокол'
    /// </summary>
    public class TomskProtocolToResolutionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDomainService<ViolationActionsRemovGji> ViolationActionDomain { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "ProtocolToResolutionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Постановления' из документа 'Протокол'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.Resolution"; }
        }

        public string ResultName
        {
            get { return "Постановление"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Protocol; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Resolution; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем предписание
            var protocol = ProtocolDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new { x.Executant, x.Contragent, x.PhysicalPerson, x.PhysicalPersonInfo })
                                    .FirstOrDefault();

            if (protocol == null)
            {
                throw new Exception("Неудалось получить протокол");
            }

            var resolution = new Resolution()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Resolution,
                Contragent = protocol.Contragent,
                Executant = protocol.Executant,
                PhysicalPerson = protocol.PhysicalPerson,
                PhysicalPersonInfo = protocol.PhysicalPersonInfo,
                TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                Paided = YesNoNotSet.NotSet
            };
            #endregion

            #region Формируем этап проверки
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.Resolution,
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

            resolution.Stage = currentStage;
            #endregion

            #region формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = resolution
            };
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

                    this.ResolutionDomain.Save(resolution);

                    this.ChildrenDomain.Save(parentChildren);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = resolution.Id, inspectionId = document.Inspection.Id });
        }

        public IDataResult ValidationRule(DocumentGji document)
        {

            // Проверяем если у протокола уже есть постановление то тогда нельзя больше формироват ьпостановление
            if ( ChildrenDomain.GetAll()
                              .Any(
                                  x =>
                                  x.Parent.Id == document.Id 
                                  && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution))
            {
                return new BaseDataResult(false, "У протокола может быть только 1 постановление");
            }

            return new BaseDataResult();
        }
    }
}
