namespace Bars.GkhGji.InspectionRules
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
  
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    // ToDo ГЖИ данное правило нужно только в регионах, необходимо перенести его в нужный регион
    // ToDo Данное правило перекрывается в некоторых регионах поэтому если будете править 
    /// <summary>
    /// Правило создание документа 'Акт проверки' из документа 'Распоряжение' (общий)
    /// </summary>
    public class DisposalToActCheckRule : IDocumentGjiRule
    {
        #region injection
        public IWindsorContainer Container { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        public IDomainService<InspectionGjiRealityObject> InspectionRoDomain { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "DisposalToActCheckRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт проверки' из документа 'Распоряжение' (общий)"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт проверки (общий)"; }
        }

        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            // никаких параметров не ожидаем от пользователей
        }

        public virtual IDataResult CreateDocument(DocumentGji document)
        {

            #region Формируем Акт Проверки
            var actCheck = new ActCheck
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActCheckGeneral,
                TypeDocumentGji = TypeDocumentGji.ActCheck
            };
            #endregion

            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id 
                                                        && x.TypeStage == TypeStage.ActCheckGeneral);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActCheckGeneral,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition =
                    InspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }

            actCheck.Stage = currentStage;

            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actCheck
            };
            #endregion

            #region Формируем Инспекторов
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = actCheck,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Формируем дома акта
            
            // Поскольку акт общий , то дома просто переносятся из Основания проверки
            var listRo = new List<ActCheckRealityObject>();
            var roIds = InspectionRoDomain.GetAll()
                                  .Where(x => x.Inspection.Id == document.Inspection.Id)
                                  .Select(x => x.RealityObject.Id)
                                  .Distinct()
                                  .ToList();

            foreach (var id in roIds)
            {
                var actRo = new ActCheckRealityObject
                {
                    ActCheck = actCheck,
                    RealityObject = new RealityObject { Id = id },
                    HaveViolation = YesNoNotSet.NotSet
                };

                listRo.Add(actRo);
            }

            actCheck.Area = RoDomain.GetAll().Where(x => roIds.Contains(x.Id)).Sum(x => x.AreaMkd);
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

                    this.ActCheckDomain.Save(actCheck);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listRo.ForEach(x => this.ActCheckRoDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = document.Inspection.Id });
            
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
                    return new BaseDataResult(false, "Акт проверки можно сформирвоать только из основного распоряжения");
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
