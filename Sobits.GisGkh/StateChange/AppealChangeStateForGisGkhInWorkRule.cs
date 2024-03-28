namespace Sobits.GisGkh.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;
    using Sobits.GisGkh.DomainService;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Enums;

    public class AppealChangeStateForGisGkhInWorkRule : IRuleChangeStatus
    {
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Статус успешного завершения
        /// </summary>
        protected bool IsSuccess { get; }

        public string Id => "gji_appeal_citizens_for_gis_gkh_in_work_rule";

        public string Name => "Формирование запроса в ГИС ЖКХ при принятии обращения в работу";

        public string TypeId => "gji_appeal_citizens";

        public string Description => this.Name;

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            //IDataResult result = null;
            var appeal = statefulEntity as AppealCits;
            if (appeal.GisWork)
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.GisGkhContragent == null)
                {
                    return ValidateResult.No("К учётной записи текущего пользователя не привязана организация для работы с ГИС ЖКХ");
                }
                if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                {
                    IExportOrgRegistryService exportOrgRegistryService = Container.Resolve<IExportOrgRegistryService>();
                    try
                    {
                        exportOrgRegistryService.SaveRequest(null, new List<long>(){ thisOperator.GisGkhContragent.Id});
                        return ValidateResult.No("У контрагента ГИС ЖКХ текущего пользователя отсутствует идентификатор. Создан запрос на получение информации об организацции");
                    }
                    catch (Exception e)
                    {
                        return ValidateResult.No("У контрагента ГИС ЖКХ текущего пользователя отсутствует идентификатор. Не удалось создать запрос на получение информации об организацции");
                    }
                    finally
                    {
                        Container.Release(exportOrgRegistryService);
                    }
                }
                var req = new GisGkhRequests
                {
                    TypeRequest = GisGkhTypeRequest.importAnswer,
                    //ReqDate = DateTime.Now,
                    RequestState = GisGkhRequestState.NotFormed
                };

                GisGkhRequestsDomain.Save(req);
                IGisGkhRegionalService gisGkhRegionalService = Container.Resolve<IGisGkhRegionalService>();
                IImportAnswerService importAnswerService = Container.Resolve<IImportAnswerService>();
                try
                {
                    Inspector inspector = gisGkhRegionalService.GetAppealPerformerForGisGkh(appeal);
                    if (inspector == null)
                    {
                        return ValidateResult.No($"Перед переводом обращения в работу выберите инстектора-исполнителя");
                    }
                    if (string.IsNullOrEmpty(inspector.GisGkhGuid))
                    {
                        return ValidateResult.No($"У указанного инспектора-исполнителя отсутствует идентификатор ГИС ЖКХ");
                    }
                    if (string.IsNullOrEmpty(appeal.NumberGji))
                    {
                        return ValidateResult.No($"У обращения не заполнен номер ГЖИ");
                    }

                    try
                    {
                       // importAnswerService.SaveAppealRequest(req, appeal, inspector);
                        try
                        {
                            //формируем печатную форму
                          
                            var reportFile = GetPOSPrintForm(appeal.Id, appeal.NumberGji);
                            if (reportFile != null)
                            {
                                appeal.File = reportFile;
                                AppealCitsDomain.Update(appeal);
                            }
                        }
                        catch
                        {
                            
                        }
                        return ValidateResult.Yes();
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return ValidateResult.No($"Не удалось создать запрос в ГИС ЖКХ на назначение обращению инспектора: {e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                    }
                }
                finally
                {
                    Container.Release(gisGkhRegionalService);
                    Container.Release(importAnswerService);
                }
            }
            else
            {
                return ValidateResult.Yes();
            }
        }

        private Bars.B4.Modules.FileStorage.FileInfo GetPOSPrintForm(long appealId, string filename)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var gkhBaseReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = gkhBaseReportDomain.FirstOrDefault(x => x.Id == "GisGkhAppeal");
                var userParam = new UserParamsValues();
                userParam.AddValue("Id", appealId);
                report.SetUserParams(userParam);
                MemoryStream stream;
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
                    //Вот такой вот костыльный этот метод Все над опеределывать
                    stream = (report as StimulReport).GetGeneratedReport();
                }
                else
                {
                    var reportParams = new ReportParams();
                    report.PrepareReport(reportParams);

                    // получаем Генератор отчета
                    var generatorName = report.GetReportGenerator();

                    stream = new MemoryStream();
                    var generator = Container.Resolve<IReportGenerator>(generatorName);
                    reportProvider.GenerateReport(report, stream, generator, reportParams);

                }
                var file = fileManager.SaveFile(stream, $"{filename}.pdf");
                return file;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.Container.Release(fileManager);

            }
        }
    }
}