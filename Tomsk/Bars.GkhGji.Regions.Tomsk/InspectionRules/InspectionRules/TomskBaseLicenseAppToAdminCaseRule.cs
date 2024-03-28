namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using Castle.Windsor;

    using TypeDocumentGji = Bars.GkhGji.Enums.TypeDocumentGji;
    using TypeStage = Bars.GkhGji.Enums.TypeStage;

    /// <summary>
    /// Правило создания документа Распоряжения из основания проверки по обращению
    /// </summary>
    public class TomskBaseLicenseAppToAdminCaseRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        #region Внутренние переменные
        private long RealityObjectId { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "BaseLicenseAppToAdminCaseRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа Администраивного дела из основания проверки соискателей лицензии"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.admincase.Edit"; }
        }

        public string ResultName
        {
            get { return "Административное дело"; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.LicenseApplicants; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.AdministrativeCase; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            RealityObjectId = baseParams.Params.GetAs<long>("realityId", 0);

            if (RealityObjectId == 0)
            {
                throw new Exception("Необходимо выбрать дом");
            }
        }

        public IDataResult CreateDocument(InspectionGji inspection)
        {
            #region Формируем этап проверки
            var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

            // Создаем Этап Проверки (Который показывается слева в дереве)
            var stage = new InspectionGjiStage
            {
                Inspection = inspection,
                Position = stageMaxPosition != null ? stageMaxPosition.Position + 1 : 1,
                TypeStage = TypeStage.AdministrativeCase
            };
            #endregion

            #region Формируем АД
            var adminCase = new AdministrativeCase()
            {
                Inspection = inspection,
                TypeDocumentGji = TypeDocumentGji.AdministrativeCase,
                TypeAdminCaseBase = TypeAdminCaseBase.DecitionInitiate,
                RealityObject = new RealityObject { Id = RealityObjectId },
                Stage = stage
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.AdminCaseDomain.Save(adminCase);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = adminCase.Id, inspectionId = inspection.Id });
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            return new BaseDataResult();
        }
    }
}
