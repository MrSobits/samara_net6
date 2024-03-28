namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа 'Протокола' из документа 'Постановление'
    /// </summary>
    public class ResolutionToProtocolRule : ResolutionToProtocolRule<Resolution, Protocol>
    {
    }

    /// <summary>
    /// базовый класс правила создания документа 'Протокола' из документа 'Постановление'
    /// </summary>
    public class ResolutionToProtocolRule<TResolution, TProtocol> : IDocumentGjiRule
        where TResolution : Resolution
        where TProtocol : Protocol, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TResolution> ResolutionDomain { get; set; }

        public IDomainService<TProtocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => "ResolutionToProtocolRule";

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Протокола' из документа 'Постановление'";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.ProtocolGji";

        /// <inheritdoc />
        public string ResultName => "Протокол";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.Resolution;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Protocol;

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров неожидаем
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол
            var resolution = this.ResolutionDomain.Get(document.Id);

            if (resolution == null)
            {
                throw new Exception("Не удалось получить постановление");
            }

            var protocol = new TProtocol
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Protocol,
                Contragent = resolution.Contragent,
                Executant = resolution.Executant,
                PhysicalPerson = resolution.PhysicalPerson,
                PhysicalPersonInfo = resolution.PhysicalPersonInfo
            };

            this.ProtocolAdditionalProcessing(resolution, protocol);
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

        /// <summary>
        /// Дополнительная обработка протокола
        /// </summary>
        /// <param name="resolution">Постановление - родельский документ формируемого протокола</param>
        /// <param name="protocol">Протокол, который нужно дополнительно обработать</param>
        protected virtual void ProtocolAdditionalProcessing(TResolution resolution, TProtocol protocol)
        {
        }

        /// <inheritdoc />
        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}