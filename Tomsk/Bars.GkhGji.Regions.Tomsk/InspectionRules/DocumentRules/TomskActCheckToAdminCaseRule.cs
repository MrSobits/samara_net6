﻿namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using Castle.Windsor;

    using TypeDocumentGji = Bars.GkhGji.Enums.TypeDocumentGji;
    using TypeStage = Bars.GkhGji.Enums.TypeStage;

    /// <summary>
    /// Правило создания документа 'Административного дела' из документа 'Акта проверки'
    /// </summary>
    public class TomskActCheckToAdminCaseRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "TomskActCheckToAdminCaseRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Административное дело' из документа 'Акт проверки'"; }
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
            get { return TypeDocumentGji.ActCheck; }
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
                TypeAdminCaseBase = TypeAdminCaseBase.ViolationNotRemoved,
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
            // В акте можно формировать протокол, только если есть нарушения  
            if (!this.ActCheckViolationDomain.GetAll().Any(x => x.Document.Id == document.Id))
            {
                return new BaseDataResult(false, "Для Акта (нарушения не выявлены) нельзя сформировать Административное дело");
            }

            return new BaseDataResult();
        }
    }
}
