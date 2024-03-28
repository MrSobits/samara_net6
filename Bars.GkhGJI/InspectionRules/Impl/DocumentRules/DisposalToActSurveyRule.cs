namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;


    /// <summary>
    /// Правило создание документа 'Акт обследования' из документа 'Распоряжение' (по выбранным домам)
    /// </summary>
    public class DisposalToActSurveyRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActSurvey> ActSurveyDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        #region Внутренние переменные
        private long[] RealityObjectIds { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DisposalToActSurveyRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт обследования' из документа 'Распоряжение' (по выбранным домам)"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActSurvey"; }
        }

        public virtual string ResultName
        {
            get { return "Акт обследования"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActSurvey; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр realityIds выбранных дома
            var realityIds = baseParams.Params.GetAs("realityIds", "");

            RealityObjectIds = !string.IsNullOrEmpty(realityIds)
                                  ? realityIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (RealityObjectIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать дома");
            }
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем акт обследования
            var actSurvey = new ActSurvey()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.ActSurvey,
                FactSurveyed = SurveyResult.NotSurveyed
            };
            #endregion

            #region Формируем этап проверки
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if ( parentStage != null && parentStage.Parent != null )
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.ActSurvey);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActSurvey,
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
            
            actSurvey.Stage = currentStage;
            #endregion

            #region Формируем связь с Родительским документом
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actSurvey
            };
            #endregion

            #region Формируем Инспекторов из родительского документа
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = this.DocumentInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == document.Id)
                    .Select(x => x.Inspector.Id).Distinct().ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = actSurvey,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем Дома указанные пользователем
            var listRo = new List<ActSurveyRealityObject>();
            foreach (var id in RealityObjectIds)
            {
                listRo.Add(new ActSurveyRealityObject
                {
                    ActSurvey = actSurvey,
                    RealityObject = new RealityObject { Id = id }
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
                    
                    this.ActSurveyDomain.Save(actSurvey);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listRo.ForEach(x => this.ActSurveyRoDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actSurvey.Id, inspectionId = document.Inspection.Id });
        }

        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            /*
             тут проверяем, Если Распоряжение не Основное то недаем выполнить формирование
            */

            if (document != null)
            {
                var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                if (disposal == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить распоряжение {0}", document.Id));
                }

                if (disposal.TypeDisposal != TypeDisposalGji.Base)
                {
                    return new BaseDataResult(false, "Акт обследования можно сформировать только из основного распоряжения");
                }

                if (!this.InspectionRoDomain.GetAll().Any(x => x.Inspection.Id == document.Inspection.Id))
                {
                    return new BaseDataResult(false, "Данный акт можно сформирвоать только для проверок с домами");
                }
            }

            return new BaseDataResult();
        }
    }
}
