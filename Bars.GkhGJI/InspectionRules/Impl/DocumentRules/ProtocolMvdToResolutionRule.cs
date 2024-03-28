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
    /// Правило создание документа 'Постановления' из документа 'Протокола МВД'
    /// </summary>
    public class ProtocolMvdToResolutionRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<ProtocolMvd> ProtocolMvdDomain { get; set; }

        public IDomainService<ResolutionDispute> ResolutionDisputeDomain { get; set; }
        
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
        
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        public IDomainService<ActSurveyRealityObject> ActSurveyRoDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<SanctionGji> SanctionGjiDomain { get; set; }

        public const string AdministrativePenaltyCod = "1"; 

        public string CodeRegion
        {
            get { return "Tat"; }
        }

        public string Id
        {
            get { return "ProtocolMvdToResolutionRule"; }
        }

        public string Description
        {
            get { return "Правило создание документа 'Постановления' из документа 'Протокола МВД'"; }
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
            get { return TypeDocumentGji.ProtocolMvd; }
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
            var protocolMvd = this.ProtocolMvdDomain.GetAll()
                                    .Where(x => x.Id == document.Id)
                                    .Select(x => new { x.TypeExecutant, x.PhysicalPerson, x.PhysicalPersonInfo })
                                    .FirstOrDefault();

            var sanctionGji = this.SanctionGjiDomain.GetAll().FirstOrDefault(x => x.Code == AdministrativePenaltyCod);

            if (protocolMvd == null)
            {
                throw new Exception("Неудалось получить пртокол МВД");
            }

            var resolution = new Resolution()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Resolution,
                Executant = this.GetExecutantDocGji(protocolMvd.TypeExecutant), // т.к. в Протоколе МВД и Постановлении в полях "Тип исполнителя" хранятся значения разных типов
                PhysicalPerson = protocolMvd.PhysicalPerson,
                PhysicalPersonInfo = protocolMvd.PhysicalPersonInfo,
                TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                Paided = YesNoNotSet.NotSet,
                OffenderWas = YesNoNotSet.NotSet,
                PenaltyAmount = 500,
                Sanction = sanctionGji
            };
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

            var currentStage = this.InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

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
                var stageMaxPosition = this.InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == document.Inspection.Id)
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
            using (var tr = this.Container.Resolve<IDataTransaction>())
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
            // Если у последнего Постановления в субтабличке Рассмотрение есть запись с кодом '3'
            // То есть возвращено на новое рассмотрение то тогда формирвоат ьновое постановление можно
            // иначе нельзя

            // Получаем последнее постановление дочернее для данного Протокола МВД
            var lastResolutionId = this.ChildrenDomain.GetAll()
                .Where(x => x.Parent.Id == document.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
                .OrderByDescending(x => x.Children.DocumentDate)
                .Select(x => x.Children.Id)
                .FirstOrDefault();

            if (lastResolutionId > 0)
            {
                // получив последнее постановление выясняем отправлено оно на новое рассмотрение или нет
                if (!this.ResolutionDisputeDomain.GetAll()
                    .Any(x => x.Resolution.Id == lastResolutionId && x.CourtVerdict.Code == "3"))
                {
                    return new BaseDataResult(false, "Чтобы сформировать новое постановление, необходимо чтобы текущее постановление было возвращено на новое рассмотрение");
                }
            }

            return new BaseDataResult();
        }

        private ExecutantDocGji GetExecutantDocGji(TypeExecutantProtocolMvd typeExecutant)
        {
            ExecutantDocGji result = null;

            switch (typeExecutant)
            {
                case TypeExecutantProtocolMvd.Individual:
                    result = this.Container.Resolve<IDomainService<ExecutantDocGji>>().GetAll().FirstOrDefault(x => x.Code == "14"); // код 14 - Физическое лицо
                    break;
            }

            return result;
        }
    }
}
