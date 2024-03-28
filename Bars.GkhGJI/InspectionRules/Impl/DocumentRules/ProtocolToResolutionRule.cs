namespace Bars.GkhGji.InspectionRules
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

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Протокол'
    /// </summary>
    public class ProtocolToResolutionRule : ProtocolToResolutionRule<Protocol, Resolution>
    {
    }

    /// <summary>
    /// Базовый класс правила создания документа 'Постановления' из документа 'Протокол'
    /// </summary>
    public class ProtocolToResolutionRule<TProtocol, TResolution> : IDocumentGjiRule
        where TProtocol : Protocol
        where TResolution : Resolution, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TResolution> ResolutionDomain { get; set; }

        public IDomainService<TProtocol> ProtocolDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => "ProtocolToResolutionRule";

        /// <inheritdoc />
        public string Description => "Правило создание документа 'Постановления' из документа 'Протокол'";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.Resolution";

        /// <inheritdoc />
        public string ResultName => "Постановление";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.Protocol;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.Resolution;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
            // никаких параметров неожидаем
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем предписание

            var protocol = this.ProtocolDomain.Get(document.Id);

            if (protocol == null)
            {
                throw new Exception("Не удалось получить протокол");
            }

            var resolution = new TResolution
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.Resolution,
                Contragent = protocol.Contragent,
                Executant = protocol.Executant,
                PhysicalPerson = protocol.PhysicalPerson,
                PhysicalPersonInfo = protocol.PhysicalPersonInfo,
                TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
                Paided = YesNoNotSet.NotSet,
                Description = protocol.Description
            };

            this.ResolutionAdditionalProcessing(protocol, resolution);
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

                    this.CopyingPhysicalPersons(document.Id, ref resolution);

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
        
        /// <summary>
        /// Дополнительная обработка постановления
        /// </summary>
        /// <param name="protocol">Протокол - родельский документ формируемого постановления</param>
        /// <param name="resolution">Постановление, который нужно дополнительно обработать</param>
        protected virtual void ResolutionAdditionalProcessing(TProtocol protocol, TResolution resolution)
        {
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {

            // Проверяем если у протокола уже есть постановление то тогда нельзя больше формироват ьпостановление
            //if ( ChildrenDomain.GetAll()
            //                  .Any(
            //                      x =>
            //                      x.Parent.Id == document.Id 
            //                      && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution))
            //{
            //    return new BaseDataResult(false, "У протокола может быть только 1 постановление");
            //}

            return new BaseDataResult();
        }

        // Ради переопределения в регионах. Для копирования Реквизитов физ. лица, которому выдан документ ГЖИ (ННовгород)
        public virtual void CopyingPhysicalPersons(long protocolId, ref TResolution resolution) { }
    }
}
