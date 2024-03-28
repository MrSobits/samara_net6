namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Постановления прокуратуры'
    /// </summary>
    public class ResolProsToResolutionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<ResolPros> ResolProsDomain { get; set; }

        public IDomainService<ResolutionDispute> ResolutionDisputeDomain { get; set; }
        
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "ResolProsToResolutionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Постановления' из документа 'Постановление прокуратуры'"; }
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
            get { return TypeDocumentGji.ResolutionProsecutor; }
        }

        public TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.Resolution; }
        }

        // тут надо принять параметры если таковые имеютя
        public void SetParams(BaseParams baseParams)
        {
            // никаких параметров неожидаем
        }

        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем предписание
            var resolPros = ResolProsDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new { x.Executant, x.Contragent, x.PhysicalPerson, x.PhysicalPersonInfo, x.UIN, x.PhysicalPersonPosition })
                                    .FirstOrDefault();

            if (resolPros == null)
            {
                throw new Exception("Неудалось получить постановление прокуратуры");
            }

            Resolution resolution = null;

            if (!string.IsNullOrEmpty(resolPros.PhysicalPerson))
            {
                resolution = new Resolution()
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Resolution,
                    Contragent = resolPros.Contragent,
                    Executant = resolPros.Executant,
                    Position = resolPros.PhysicalPersonPosition,
                    Surname = resolPros.PhysicalPerson.Split(' ').Length > 2 ? resolPros.PhysicalPerson.Split(' ')[0] : "",
                    FirstName = resolPros.PhysicalPerson.Split(' ').Length > 2 ? resolPros.PhysicalPerson.Split(' ')[1] : "",
                    Patronymic = resolPros.PhysicalPerson.Split(' ').Length > 2 ? resolPros.PhysicalPerson.Split(' ')[2] : "",
                    PhysicalPerson = resolPros.PhysicalPerson,
                    PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
                    TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                    Paided = YesNoNotSet.NotSet,
                    GisUin = resolPros.UIN
                };
            }
            else
            {
                resolution = new Resolution()
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Resolution,
                    Contragent = resolPros.Contragent,
                    Executant = resolPros.Executant,
                    PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
                    TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                    Paided = YesNoNotSet.NotSet,
                    GisUin = resolPros.UIN
                };
            }

   
            #endregion

            #region Формируем этап проверки
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

            resolution.Stage = currentStage;
            #endregion

            #region формируем связь с родителем
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
            // Короче в РТ такой замут который я досих пор понят ьнемогу
            // Если у последнего Постановления в субтабличке Рассмотрение есть запись с кодом '3'
            // То есть возвращено на новое рассмотрение то тогда формирвоат ьновое постановление можно
            // иначе нельзя

            // Получаем последнее постановление дочернее для данного Постановления прокуратуры
            var lastResolutionId = ChildrenDomain.GetAll()
                .Where(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                .OrderByDescending(x => x.Children.DocumentDate)
                .Select(x => x.Children.Id)
                .FirstOrDefault();

            if (lastResolutionId > 0)
            {
                // получив последнее постановление выясняем отправлено оно на новое рассмотрение или нет
                if (!ResolutionDisputeDomain.GetAll()
                    .Any(x => x.Resolution.Id == lastResolutionId && x.CourtVerdict.Code == "3"))
                {
                    return new BaseDataResult(false, "Чтобы сформировать новое постановление, необходимо чтобы текущее постановление было возвращено на новое рассмотрение");
                }
            }

            return new BaseDataResult();
        }
    }
}
