namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания документа 'Постановления' из документа 'Административного дела'
    /// </summary>
    public class TomskAdminCaseToResolutionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<SanctionGji> SanctionDomain { get; set; } 

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "AdminCaseToResolutionRule"; }
        }

        public string Description
        {
            get { return "Правило создания документа 'Постановления' из документа 'Административного дела'"; }
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
            get { return TypeDocumentGji.AdministrativeCase; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Resolution; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            //никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап постановления
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
            #endregion

            #region Формируем Постановление
            var adminCase = AdminCaseDomain.GetAll().FirstOrDefault(x => x.Id == document.Id);

            if (adminCase == null)
            {
                return new BaseDataResult(false, "Не найден документ Административного дела");
            }

            // Получаем Санкцию = Прекращено
            var sanction = SanctionDomain.GetAll().FirstOrDefault(x => x.Code == "2");

            var resolution = new Resolution()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Resolution,
                Contragent = adminCase.Contragent,
                Sanction = sanction,
                TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                Paided = YesNoNotSet.NotSet,
                Stage = currentStage
            };
            #endregion

            #region Формируем связь с родителем
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
            /*Если у АД уже есть Постановление, то нельзя еще раз его создавать
            */
            if (ChildrenDomain.GetAll().Any(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution))
            {
                return new BaseDataResult(false, "Для Административного дела постановление может быть сформировано только 1 раз");
            }
            return new BaseDataResult();
        }
    }
}
