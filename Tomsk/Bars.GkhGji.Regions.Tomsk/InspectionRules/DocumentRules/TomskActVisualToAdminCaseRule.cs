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
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;


    /// <summary>
    /// Правило создание документа 'Административного дела' из документа 'Акта визуального осмотра'
    /// </summary>
    public class TomskActVisualToAdminCaseRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "ActVisualToAdminCaseRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Административного дела' из документа 'Акт визуального осмотра'"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.admincase.Edit"; }
        }

        public string ResultName
        {
            get { return "Административное дело"; }
        }

        public TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.ActVisual; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.AdministrativeCase; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var newStage = new InspectionGjiStage
            {
                Inspection = document.Inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = TypeStage.AdministrativeCase
            };
            #endregion

            #region Формируем АД
            var adminCase = new AdministrativeCase()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.AdministrativeCase,
                TypeAdminCaseBase = Bars.GkhGji.Regions.Tomsk.Enums.TypeAdminCaseBase.VisualInspection,
                Stage = newStage
                
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = adminCase
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(newStage);

                    this.AdminCaseDomain.Save(adminCase);

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

            return new BaseDataResult(new { documentId = adminCase.Id, inspectionId = document.Inspection.Id });
        }

        public IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}
