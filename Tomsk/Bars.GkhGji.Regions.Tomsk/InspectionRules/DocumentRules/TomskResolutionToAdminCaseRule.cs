namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
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
    /// Правило создания документа 'Административного дела' из документа 'Постановления'
    /// </summary>
    public class TomskResolutionToAdminCaseRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "ResolutionToAdminCaseRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Административное дело' из документа 'Постановление'"; }
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
            get { return TypeDocumentGji.Resolution; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.AdministrativeCase; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров не ожидаем
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
                TypeAdminCaseBase = TypeAdminCaseBase.ResolutionTermination,
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
            // Административное дело можно сформировать из постановления только в том случае, если
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

            if ( !ChildrenDomain.GetAll()
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
