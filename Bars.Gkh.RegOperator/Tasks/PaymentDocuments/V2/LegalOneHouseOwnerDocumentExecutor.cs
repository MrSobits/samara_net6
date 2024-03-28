namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Enums;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;

    using Microsoft.Extensions.Logging;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обработчик задачи генерации документов на оплату для юрлиц
    /// </summary>
    internal class LegalOneHouseOwnerDocumentExecutor : BaseDocumentExecutor
    {
        /// <summary>
        /// Идентификатор обработчика
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly IDomainService<ChargePeriod> periodDmn;
        private readonly IRepository<PaymentDocumentTemplate> templatePeriodRepo;
        private readonly RegOperatorConfig config;
        private readonly ISessionProvider sessionProv;

        /// <summary>
        /// Конструктор обработчика
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="configProv">Провайдер конфигурации</param>
        /// <param name="periodDmn">Домен-сервис периодов</param>
        /// <param name="templatePeriodRepo">Репозиторий шаблонов документов на оплату</param>
        /// <param name="sessionProv">Провайдер сессии</param>
        public LegalOneHouseOwnerDocumentExecutor(
            IWindsorContainer container,
            IGkhConfigProvider configProv,
            IDomainService<ChargePeriod> periodDmn,
            IRepository<PaymentDocumentTemplate> templatePeriodRepo,
            ISessionProvider sessionProv)
            : base(container)
        {
            this.container = container;
            this.periodDmn = periodDmn;
            this.templatePeriodRepo = templatePeriodRepo;
            this.config = configProv.Get<RegOperatorConfig>();

            this.sessionProv = sessionProv;
        }     
        
        /// <summary>
        /// Выполнение обработки
        /// </summary>
        /// <param name="baseParams">Входные параметры отчета</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор выполнения</param>
        /// <param name="ct">Обработчик отмены</param>
        /// <returns>Результат обработки</returns>
        public override IDataResult Execute(
            BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var isZeroPaymentDoc = baseParams.Params.GetAs<bool>("isZeroPaymentDoc");
            var periodDto = baseParams.Params.GetAs<PeriodDto>("periodDto");
            var primarySources = baseParams.Params.GetAs<List<PayDocPrimarySource>>("primarySources");
            var path = baseParams.Params.GetAs<string>("path");
            var uid = baseParams.Params.GetAs<string>("uid");
            
            var filesPath = this.GetFilesPath(periodDto, path);
            var periodEntity = this.periodDmn.Get(periodDto.Id);

            indicator.Report(null, 0, "Получение данных (Количество: {0})".FormatUsing(primarySources.Count));
            using (var docSource = new PaymentDocumentDataSource(this.container, periodEntity, primarySources, PaymentDocumentType.Legal))
            {
                indicator.Report(null, 0, "Создание слепков (Количество: {0})".FormatUsing(primarySources.Count));
                docSource.CreateSnapshots(isZeroPaymentDoc);

                var options = this.config.PaymentDocumentConfigContainer;
                var docData = docSource.DocumentData;

                var ownerData = docData.GetOwnerData();

                var snapshotedOwners = ownerData.Select(x => x.Owner);

                var prefix = (options.PaymentDocumentConfigLegal.OrgFormGroup == OrgFormGroup.FolderGroup 
                        && options.PaymentDocumentConfigLegal.GroupingForLegalWithOneOpenRoom == GroupingForLegalWithOneOpenRoom.IndividualFolder)
                    ? "ЮЛ "
                    : string.Empty;

                var args = new ReportArgs
                {
                    DocumentsPath = filesPath,
                    FileName = "{0}1-{1}".FormatUsing(prefix, primarySources.Count),
                    Snapshots = snapshotedOwners,
                    AccountInfos = ownerData.SelectMany(x => x.Accounts)
                };

                using (var reportMan = new PaymentDocReportManager(this.templatePeriodRepo))
                {
                    try
                    {
                        var report = new InvoiceAndActReport(args.Snapshots) as ICodedReport;
                        this.GenerateReport(
                            options,
                            reportMan,
                            args,
                            report,
                            periodEntity);
                        this.SavePaymentDocumentLog(uid, path, snapshotedOwners);
                    }
                    catch (Exception exception)
                    {
                        this.appLogger.LogError(exception.ToString());
                        throw;
                    }
                }

                this.sessionProv.CloseCurrentSession();

                indicator.Report(null, 60, "Сохранение слепков (Количество: {0})".FormatUsing(primarySources.Count));
                docSource.Save();

                if (!isZeroPaymentDoc)
                {
                    indicator.Report(null, 90, "Добавление в системную группу по открытому периоду (если необходимо)");
                    this.AddToGroupInOpenPeriod(periodDto, primarySources);
                }

                this.sessionProv.GetCurrentSession().Clear();
            }
            indicator.Report(null, 100, "Завершено");
            return new BaseDataResult();
        }

        /// <summary>
        /// Код (идентификатор) обработчика
        /// </summary>
        public override string ExecutorCode
        {
            get { return Id; }
        }
    }
}