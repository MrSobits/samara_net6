namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;

    using Castle.Windsor;

    public class TatProtocolGjiToTatResolutionGjiRule : IDocumentGjiRule
    {
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(TatProtocolGjiToTatResolutionGjiRule);

        /// <inheritdoc />
        public string Description => "Правило создание документа 'Постановление' из документа 'Протокол ГЖИ РТ'";

        /// <inheritdoc />
        public string ResultName => "Постановление";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.tatarstanresolutiongji.Edit";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.Protocol2061;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Resolution2061;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
            //
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            var protocolGjiDomain = this.Container.ResolveDomain<TatarstanProtocolGjiContragent>();
            var inspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>();
            var resolutionDomain = this.Container.ResolveDomain<TatarstanResolutionGji>();

            using (this.Container.Using(protocolGjiDomain, inspectionStageDomain))
            {
                var protocolGji = protocolGjiDomain.FirstOrDefault(x => x.Id == document.Id);

                if (protocolGji == null)
                {
                    throw new Exception("Не удалось получить протокол ГЖИ РТ");
                }

                var resolution = new TatarstanResolutionGji
                {
                    Inspection = document.Inspection,
                    TypeDocumentGji = TypeDocumentGji.Resolution2061,
                    OffenderWas = YesNoNotSet.NotSet,
                    TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                    Sanction = protocolGji.Sanction,
                    Paided = protocolGji.Paided,
                    PenaltyAmount = protocolGji.PenaltyAmount,
                    DateTransferSsp = protocolGji.DateTransferSsp,
                    TypeTerminationBasement = protocolGji.TerminationBasement,
                    TerminationDocumentNum = protocolGji.TerminationDocumentNum,
                    TypeExecutant = protocolGji.Executant,
                    SurName = protocolGji.SurName,
                    Name = protocolGji.Name,
                    Patronymic = protocolGji.Patronymic,
                    BirthDate = protocolGji.BirthDate,
                    BirthPlace = protocolGji.BirthPlace,
                    Address = protocolGji.Address,
                    MaritalStatus = protocolGji.MaritalStatus,
                    DependentCount = protocolGji.DependentCount,
                    CitizenshipType = protocolGji.CitizenshipType,
                    Citizenship = protocolGji.Citizenship,
                    IdentityDocumentType = protocolGji.IdentityDocumentType,
                    SerialAndNumberDocument = protocolGji.SerialAndNumberDocument,
                    IssueDate = protocolGji.IssueDate,
                    IssuingAuthority = protocolGji.IssuingAuthority,
                    Company = protocolGji.Company,
                    RegistrationAddress = protocolGji.RegistrationAddress,
                    Salary = protocolGji.Salary,
                    ResponsibilityPunishment = protocolGji.ResponsibilityPunishment,
                    Contragent = protocolGji.Contragent,
                    DelegateFio = protocolGji.DelegateFio,
                    DelegateCompany = protocolGji.DelegateCompany,
                    ProcurationNumber = protocolGji.ProcurationNumber,
                    ProcurationDate = protocolGji.ProcurationDate,
                    DelegateResponsibilityPunishment = protocolGji.DelegateResponsibilityPunishment
                };

                #region Формируем этап проверки
                // Если у родительского документа есть этап у которого есть родитель
                // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
                var parentStage = document.Stage;
                if (parentStage?.Parent != null)
                {
                    parentStage = parentStage.Parent;
                }

                InspectionGjiStage newStage = null;

                var currentStage = inspectionStageDomain.FirstOrDefault(x => x.Parent == parentStage 
                    && x.TypeStage == TypeStage.ResolutionGji);

                if (currentStage == null)
                {
                    // Если этап не найден, то создаем новый этап
                    currentStage = new InspectionGjiStage
                    {
                        Inspection = document.Inspection,
                        TypeStage = TypeStage.ResolutionGji,
                        Parent = parentStage,
                        Position = 1
                    };

                    var stageMaxPosition = inspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

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

                this.Container.InTransaction(() =>
                {
                    if (newStage != null)
                    {
                        inspectionStageDomain.Save(newStage);
                    }

                    resolutionDomain.Save(resolution);

                    this.ChildrenDomain.Save(parentChildren);
                    this.CopyEyewitness(protocolGji, resolution);
                });

                return new BaseDataResult(new { documentId = resolution.Id, inspectionId = document.Inspection.Id });
            }
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            var child = this.ChildrenDomain.FirstOrDefault(x => x.Parent.Id == document.Id);
            return child == null ? new BaseDataResult() : new BaseDataResult(false, "");
        }

        /// <summary>
        /// Копирует "сведения о свидетелях и потерпевших" протокола
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="resolution"></param>
        private void CopyEyewitness(TatarstanProtocolGji protocol, TatarstanResolutionGji resolution)
        {
            var eyewitnessDomain = this.Container.ResolveDomain<TatarstanDocumentWitness>();
            using (this.Container.Using(eyewitnessDomain))
            {
                var protocolEyewitnesses = eyewitnessDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == protocol.Id)
                    .ToList();

                protocolEyewitnesses.ForEach(x => eyewitnessDomain.Save(new TatarstanDocumentWitness
                {
                    DocumentGji = resolution,
                    WitnessType = x.WitnessType,
                    Fio = x.Fio,
                    FactAddress = x.FactAddress,
                    Phone = x.Phone
                }));
            }
        }
    }
}
