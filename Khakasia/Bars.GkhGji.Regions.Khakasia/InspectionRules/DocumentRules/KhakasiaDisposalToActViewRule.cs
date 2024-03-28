namespace Bars.GkhGji.Regions.Khakasia.InspectionRules.DocumentRules
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.InspectionRules;
	using Castle.Windsor;

	/// <summary>
	/// Правило создание документа 'Акт осмотра' из документа 'Распоряжение' 
	/// </summary>
	public class KhakasiaDisposalToActViewRule : IDocumentGjiRule
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

        #region Внутренние переменные
        private long[] RealityObjectIds { get; set; }
        #endregion

        public virtual string CodeRegion
        {
            get { return "Tat"; }
        }

        public virtual string Id
        {
            get { return "KhakasiaDisposalToActViewRule"; }
        }

        public virtual string Description
        {
            get { return "Правило создание документа 'Акт осмотра' из документа 'Распоряжение' (по выбранным домам)"; }
        }

        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        public virtual string ResultName
        {
            get { return "Акт осмотра"; }
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
            // В данном методе принимаем параметр Id выбранных пользователем домов
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

            #region Формируем Акт Проверки
            var actCheck = new ActCheck
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActView,
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
                                                        && x.TypeStage == TypeStage.ActCheck);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActView,
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

            // Поскольку пользователи сами выбирают дома то их и переносим
            var listRo = new List<ActCheckRealityObject>();

            foreach (var id in RealityObjectIds)
            {
                var actRo = new ActCheckRealityObject
                {
                    ActCheck = actCheck,
                    RealityObject = new RealityObject { Id = id },
                    HaveViolation = YesNoNotSet.NotSet
                };

                listRo.Add(actRo);
            }

            actCheck.Area = RoDomain.GetAll().Where(x => RealityObjectIds.Contains(x.Id)).Sum(x => x.AreaMkd);
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

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = actCheck.Inspection.Id });
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

                if (ActCheckDomain.GetAll().Any(x => x.Inspection.Id == disposal.Inspection.Id && (x.TypeActCheck == TypeActCheckGji.ActCheckGeneral || x.TypeActCheck == TypeActCheckGji.ActCheckIndividual)))
                {
                    return new BaseDataResult(false, "Нельзя создавать акт осмотра, если в проверке уже создан акт проверки");
                }
            }

            return new BaseDataResult();
        }
    }
}
