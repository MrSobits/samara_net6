namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class TaskActionToActActionRule: IDocumentGjiRule
    {
        public IDomainService<ActActionIsolated> ActActionDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<TaskActionIsolatedRealityObject> TaskRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(TaskActionToActActionRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Акт по КНМ без взаимодействия с контролируемыми лицами' из документа 'Задание КНМ без взаимодействия с контролируемыми лицами'";

        /// <inheritdoc />
        public string ResultName => "Акт по КНМ без взаимодействия с контролируемыми лицами";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.actionisolated.actaction.ActActionIsolated";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.TaskActionIsolated;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.ActActionIsolated;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр Id выбранных пользователем домов
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            this.RealityObjectIds = realityIds.ToLongArray();

            if (this.RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.ActActionIsolated);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActActionIsolated,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }
            #endregion

            #region Формируем документ
            var state = this.StateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "gji_document_act_actionisolated");
            var actCheck = new ActActionIsolated
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActActionIsolated,
                TypeDocumentGji = TypeDocumentGji.ActActionIsolated,
                Stage = currentStage,
                State = state
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actCheck
            };
            #endregion

            #region Формируем дома акта
            var listRo = new List<ActCheckRealityObject>();

            foreach (var id in this.RealityObjectIds)
            {
                var actRo = new ActCheckRealityObject
                {
                    ActCheck = actCheck,
                    RealityObject = new RealityObject { Id = id },
                    HaveViolation = YesNoNotSet.NotSet
                };

                listRo.Add(actRo);
            }
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ActActionDomain.Save(actCheck);

                    this.ChildrenDomain.Save(parentChildren);
                    
                    listRo.ForEach(x => this.ActCheckRoDomain.Save(x));

                    tr.Commit();
            }
            #endregion

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = actCheck.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.ChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.ActActionIsolated))
            {
                return new BaseDataResult(false, "У задания может быть только 1 акт");
            }

            if (!this.TaskRoDomain.GetAll().Any(x => x.Task.Id == document.Id))
            {
                return new BaseDataResult(false, "Не выбраны дома");
            }

            return new BaseDataResult();
        }

        private long[] RealityObjectIds { get; set; }
    }
}
