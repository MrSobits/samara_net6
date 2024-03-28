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
    using Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создания из основания проверки по поручению руководства документа Распоряжения
    /// </summary>
    public class TomskBaseDispHeadActVisualRule : IInspectionGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ActVisual> ActVisualDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }


        #region Внутренние переменные
        private long RealityObjectId { get; set; }
        #endregion

        public string CodeRegion
        {
            get { return "Tomsk"; }
        }

        public string Id
        {
            get { return "BaseDispHeadActVisualRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа Акта визуального осмотра из основания проверки по поручению руководства"; }
        }

        public string ActionUrl
        {
            get { return "B4.controller.ActVisual"; }
        }

        public string ResultName
        {
            get { return "Акт визуального осмотра"; }
        }

        public TypeBase TypeInspectionInitiator
        {
            get { return TypeBase.DisposalHead; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActVisual; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            RealityObjectId = baseParams.Params.GetAs<long>("realityId");

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
                TypeStage = TypeStage.ActVisual
            };
            #endregion

            #region Формируем нового документа
            var actVisual = new ActVisual()
            {
                Inspection = inspection,
                TypeDocumentGji = TypeDocumentGji.ActVisual,
                Stage = stage,
                RealityObject = new RealityObject { Id = RealityObjectId }
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    this.InspectionStageDomain.Save(stage);

                    this.ActVisualDomain.Save(actVisual);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actVisual.Id, inspectionId = inspection.Id });
        }

        public IDataResult ValidationRule(InspectionGji inspection)
        {
            return new BaseDataResult();
        }
    }
}
