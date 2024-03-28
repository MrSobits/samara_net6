namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    using Utils;

    public class RequirementDocumentService : IRequirementDocumentService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDomainService<RequirementDocument> ReqDocDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<Requirement> ReqDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        /// <summary>
        /// метод создания протокола из требования
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult CreateProtocol(BaseParams baseParams)
        {

            var requirementId = baseParams.Params.GetAs<long>("requirementId", 0);

            var reqDoc = ReqDomain.GetAll().FirstOrDefault(x => x.Id == requirementId);

            if (reqDoc == null)
            {
                return new BaseDataResult(false, "Не удалось определить требование");
            }
            
            #region Формируем этап проверки
            var parentStage = reqDoc.Document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.Protocol);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = reqDoc.Document.Inspection,
                    TypeStage = TypeStage.Protocol,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition =
                    InspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == reqDoc.Document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }

            #endregion

            #region Формируем Протокол
            var protocol = new Protocol()
            {
                Inspection = reqDoc.Document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Stage = currentStage
            };
            #endregion

            #region Формируем связь с родителем и с Требованием
            var parentChildren = new DocumentGjiChildren
            {
                Parent = reqDoc.Document,
                Children = protocol
            };

            var reqChildren = new RequirementDocument { Requirement = reqDoc, Document = protocol };
            #endregion

            #region Формируем Инспекторов
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == reqDoc.Document.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

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

                    this.ReqDocDomain.Save(reqChildren);

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

            return new BaseDataResult(new { documentId = protocol.Id, inspectionId = protocol.Inspection.Id });
        }
    }
}