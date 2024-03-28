namespace Bars.GkhGji.Regions.Smolensk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Протокола' из документа 'Постановление' для Региона Смоленск
    /// </summary>
    public class SmolenskResolutionToProtocolRule : Bars.GkhGji.InspectionRules.ResolutionToProtocolRule
    {
        /// <summary>
        /// Это код статьи который должен в Смоленске по умолчанию создатся если мы формируем из Постановления протокол
        /// </summary>
        protected readonly string defaultAticleCode = "20.25";

        public IDomainService<ArticleLawGji> ArticleLawDomain { get; set; }

        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public override IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем протокол

            var resolution =
                ResolutionDomain.GetAll()
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

            var currentStage =
                InspectionStageDomain.GetAll()
                                     .FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Protocol);

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
                var stageMaxPosition =
                    InspectionStageDomain.GetAll()
                                         .Where(x => x.Inspection.Id == document.Inspection.Id)
                                         .OrderByDescending(x => x.Position)
                                         .FirstOrDefault();

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

            var parentChildren = new DocumentGjiChildren { Parent = document, Children = protocol };

            #endregion

            #region Формируем Инспекторов тянем их из родительского документа

            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds =
                this.DocumentInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id)
                    .Distinct()
                    .ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(
                    new DocumentGjiInspector { DocumentGji = protocol, Inspector = new Inspector { Id = id } });
            }

            #endregion

            #region Формируем статью по умолчанию 

            ProtocolArticleLaw protocolArticle = null;

            var article = ArticleLawDomain.GetAll().FirstOrDefault(x => x.Name.Contains(defaultAticleCode));

            if (article != null)
            {
                protocolArticle = new ProtocolArticleLaw { ArticleLaw = article, Protocol = protocol };
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

                    if (protocolArticle != null)
                    {
                        ProtocolArticleLawDomain.Save(protocolArticle);
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
    }
}
