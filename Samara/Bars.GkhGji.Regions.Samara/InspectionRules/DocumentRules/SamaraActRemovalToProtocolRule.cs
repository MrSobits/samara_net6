namespace Bars.GkhGji.Regions.Samara.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    public class SamaraActRemovalToProtocolRule : ActRemovalToProtocolRule
    {
        public IDomainService<ArticleLawGji> ArticleLawDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        protected const string articleLawCode = "1";

        public override IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var protocol = new Protocol()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = document.Inspection.Contragent
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

            #region Формируем нарушения (по выбранным пользователями)
            var listProtocolViol = new List<ProtocolViolation>();
            var violationList = InspectionViolDomain.GetAll().Where(x => ViolationIds.Contains(x.Id));

            foreach (var viol in violationList)
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

            #region Добаляем Статью закона 19.5
            ProtocolArticleLaw protocolArtLaw = null;
            var artLaw = ArticleLawDomain.FirstOrDefault(x => x.Code == articleLawCode);

            if (artLaw != null)
            {
                protocolArtLaw = new ProtocolArticleLaw { Protocol = protocol, ArticleLaw = artLaw };
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

                    listProtocolViol.ForEach(x => this.ProtocolViolationDomain.Save(x));

                    if (protocolArtLaw != null)
                    {
                        this.ProtocolArticleLawDomain.Save(protocolArtLaw);
                    }

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


        public override string ResultName
        {
            get { return "Протокол (ст.19.5 ч.1)"; }
        }
    }
}
