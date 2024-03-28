namespace Bars.GisIntegration.Smev.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.Exporters;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class ErknmIntegrationService : BaseIntegrationService, IErknmIntegrationService
    {
        /// <inheritdoc />
        public IDataResult DocumentList(BaseParams baseParams)
        {
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var risTaskDomain = this.Container.ResolveDomain<RisTask>();
            var decisionDomain = this.Container.ResolveDomain<Decision>();
            var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
            var warningDocDomain = this.Container.ResolveDomain<WarningDoc>();

            using (this.Container.Using(risTaskDomain, decisionDomain,
                preventiveActionDomain, warningDocDomain))
            {
                var extendedEntitiesDict= decisionDomain.GetAll()
                    .Where(x => x.ErknmRegistrationDate.HasValue)
                    .ToDictionary(x => x.Id, y => new
                    {
                        y.ErknmRegistrationDate,
                        y.ErknmRegistrationNumber
                    });

                preventiveActionDomain.GetAll()
                    .Where(x => x.ErknmRegistrationDate.HasValue)
                    .ToDictionary(x => x.Id, y => new
                    {
                        y.ErknmRegistrationDate,
                        y.ErknmRegistrationNumber
                    })
                    .AddTo(extendedEntitiesDict);

                warningDocDomain.GetAll()
                    .Where(x => x.ErknmRegistrationDate.HasValue)
                    .ToDictionary(x => x.Id, y => new
                    {
                        y.ErknmRegistrationDate,
                        y.ErknmRegistrationNumber
                    })
                    .AddTo(extendedEntitiesDict);

                var availableDocTypes = new []
                {
                    TypeDocumentGji.Decision,
                    TypeDocumentGji.PreventiveAction,
                    TypeDocumentGji.WarningDoc
                };

                var availableMethods = new []
                {
                    new KnmExporter().Name,
                    new KnmCorrectionExporter().Name
                };

                return risTaskDomain.GetAll()
                    .Where(x => availableDocTypes.Contains(x.DocumentGji.TypeDocumentGji))
                    .Where(x => availableMethods.Contains(x.Description))
                    .AsEnumerable()
                    .GroupBy(x => x.DocumentGji)
                    .Select(x =>
                    {
                        var extendedEntity = extendedEntitiesDict.Get(x.Key.Id);

                        return new ErknmRegistryDocumentDto
                        {
                            Id = x.Key.Id,
                            DocumentNumber = x.Key.DocumentNumber,
                            DocumentDate = x.Key.DocumentDate,
                            ErknmGuid = x.Key.ErknmGuid,
                            ErknmRegistrationDate = extendedEntity?.ErknmRegistrationDate,
                            ErknmRegistrationNumber = extendedEntity?.ErknmRegistrationNumber,
                            InspectionId = x.Key.Inspection.Id,
                            DocumentType = x.Key.TypeDocumentGji,
                            DocumentTypeBase = x.Key.Inspection.TypeBase,
                            LastMethodStartTime = x.Max(y => y.StartTime)
                        };
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePaging: !isExport);
            }
        }

        /// <inheritdoc />
        public IDataResult SendToErknm(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            
            var decisionDomain = this.Container.ResolveDomain<Decision>();
            var erknmIntegrationConfig = this.Container.GetGkhConfig<ErknmIntegrationConfig>();
            var gisIntegrationService = this.Container.Resolve<IGisIntegrationService>();
            var risTaskDomain = this.Container.ResolveDomain<RisTask>();
            var decisionInspectionBaseDomain = this.Container.ResolveDomain<DecisionInspectionBase>();

            using (this.Container.Using(decisionDomain, gisIntegrationService, risTaskDomain, decisionInspectionBaseDomain))
            {
                if (!erknmIntegrationConfig.IsEnabled)
                {
                    return new BaseDataResult(false, "Интеграция с ЕРКНМ отключена в настройках приложения");
                }
                
                var decision = decisionDomain.Get(id);
                var exporterName = string.IsNullOrEmpty(decision.ErknmGuid) ? nameof(KnmExporter) : nameof(KnmCorrectionExporter);

                if (!decisionInspectionBaseDomain.GetAll().Any(x => x.Decision.Id == id))
                {
                    return new BaseDataResult(false, "Необходимо заполнить основание проведения КНМ");
                }

                if (string.IsNullOrEmpty(decision.ErknmGuid))
                {
                    if (!(decision.DateStart >= DateTime.Today))
                    {
                        return new BaseDataResult(false, "Дата в поле \"Период обследования с\" не должна быть раньше текущей даты");
                    }

                    if (!(decision.DateEnd >= decision.DateStart))
                    {
                        return new BaseDataResult(false,
                            "Дата в поле \"Период обследования по\" не должна быть раньше текущей даты и даты начала обследования");
                    }
                    
                    var exporter = this.Container.Resolve<IDataExporter>(exporterName);
                    
                    using (this.Container.Using(exporter))
                    {

                        // Контрольная дата начала отправки распоряжения
                        var compareTime = DateTime.Now.AddSeconds(-exporter.Interval * (exporter.MaxRepeatCount + 1));

                        if (risTaskDomain.GetAll()
                            .Any(x => x.DocumentGji.Id == id
                                && x.TaskState != TaskState.Error
                                && x.TaskState != TaskState.CompleteSuccess
                                && x.TaskState != TaskState.CompleteWithErrors
                                && x.StartTime > compareTime))
                        {
                            return new BaseDataResult(false, "Первичное размещение уже выполняется");
                        }
                    }
                }

                var par = new DynamicDictionary { { "0", new DynamicDictionary { { "id", id }, { "documentGji", decision } } } };

                var gisParams = new BaseParams();

                gisParams.Params.SetValue("exporter_Id", exporterName);

                gisParams.Params.SetValue("params", par);

                var prepareDataResult = gisIntegrationService.SchedulePrepareData(gisParams);

                return prepareDataResult;
            }
        }

        /// <inheritdoc />
        public IDataResult BeforeSendCheck(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId();
            var errors = new List<string>();
            
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();
            var docGjiDomain = this.Container.ResolveDomain<DocumentGji>();
            var inspGjiRealityObjService = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var decisionControlObjInfoService = this.Container.ResolveDomain<DecisionControlObjectInfo>();

            using (this.Container.Using(inspectionRiskDomain, docGjiDomain, inspGjiRealityObjService, decisionControlObjInfoService))
            {
                var inspectionId = docGjiDomain.Get(documentId).Inspection.Id;
                
                // Проверка наличия категории риска
                var risk = inspectionRiskDomain.GetAll().Where(x => x.Inspection.Id == inspectionId)
                    .FirstOrDefault(x => !x.EndDate.HasValue);

                if (risk is null)
                {
                    errors.Add("Проверьте заполненость поля <b>Категория риска</b>");
                }
                
                // Проверка на наличие проверяемых домов
                var inspRealityObjectIds = inspGjiRealityObjService.GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();
                
                var existRObjectCount = decisionControlObjInfoService.GetAll()
                    .Where(x => x.Decision.Id == documentId)
                    .WhereIf(inspRealityObjectIds.Any(), x => inspRealityObjectIds.Contains(x.InspGjiRealityObject.RealityObject.Id))
                    .Count();

                if (existRObjectCount != inspRealityObjectIds.Length)
                {
                    errors.Add("Проверьте наличие домов во вкладке <b>Сведения об объектах контроля</b>");
                }

                return errors.Any() ? new BaseDataResult(false, string.Join(";", errors)) : new BaseDataResult();
            }
        }

        /// <inheritdoc />
        public ReportStreamResult ExcelFileExport(BaseParams baseParams)
        {
            baseParams.Params.Add("isExport", true);
            var docList = (IList)this.DocumentList(baseParams).Data;

            var reportGenerator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");
            var dataExportReport = this.Container.Resolve<IDataExportReport>("ErknmIntegrationRegistryReport", new
            {
                Data = docList,
                BaseParams = baseParams
            });

            using (this.Container.Using(reportGenerator, dataExportReport))
            {
                var reportParams = new ReportParams();
                var memoryStream = new MemoryStream();
                dataExportReport.PrepareReport(reportParams);

                var template = dataExportReport.GetTemplate();
                reportGenerator.Open(template);
                reportGenerator.Generate(memoryStream, reportParams);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return new ReportStreamResult(memoryStream, "export.xlsx");
            }
        }
    }
}