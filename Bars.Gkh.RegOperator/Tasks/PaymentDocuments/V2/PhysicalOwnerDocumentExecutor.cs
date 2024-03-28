namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4;
    using Bars.B4.DataAccess;
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

    using Bars.Gkh.RegOperator.Enums;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Assembly;
    using Bars.Gkh.RegOperator.PersonalAccountGroup;

    using Microsoft.Extensions.Logging;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обработчик задачи генерации документов на оплату для юрлиц
    /// </summary>
    internal class PhysicalOwnerDocumentExecutor : BaseDocumentExecutor
    {
        /// <summary>
        /// Идентификатор обработчика
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly IDomainService<ChargePeriod> periodDmn;
        private readonly IRepository<PaymentDocumentTemplate> templateCopyRepo;
        private readonly ISessionProvider sessionProv;
        private readonly RegOperatorConfig config;

        /// <summary>
        /// Конструктор обработчика
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="configProv">Провайдер конфигурации</param>
        /// <param name="periodDmn">Домен-сервис периодов</param>
        /// <param name="templateCopyRepo">Репозиторий шаблонов документов на оплату</param>
        /// <param name="sessionProv">Провайдер сессии</param>
        public PhysicalOwnerDocumentExecutor(
            IWindsorContainer container,
            IGkhConfigProvider configProv,
            IDomainService<ChargePeriod> periodDmn,
            IRepository<PaymentDocumentTemplate> templateCopyRepo,
            ISessionProvider sessionProv)
            : base(container)
        {
            this.container = container;
            this.periodDmn = periodDmn;
            this.templateCopyRepo = templateCopyRepo;
            this.sessionProv = sessionProv;

            this.config = configProv.Get<RegOperatorConfig>();
        }

        /// <summary>
        /// Выполнение обработки
        /// </summary>
        /// <param name="baseParams">Входные параметры отчета</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор выполнения</param>
        /// <param name="ct">Обработчик отмены</param>
        /// <returns>Результат обработки</returns>
        public override IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var primarySources = baseParams.Params.GetAs<List<PayDocPrimarySource>>("primarySources");
            var isZeroPaymentDoc = baseParams.Params.GetAs<bool>("isZeroPaymentDoc");
            var periodDto = baseParams.Params.GetAs<PeriodDto>("periodDto");
            var path = baseParams.Params.GetAs<string>("path");
            var uid = baseParams.Params.GetAs<string>("uid");

            var filesPath = this.GetFilesPath(periodDto, path);
            var periodEntity = this.periodDmn.Get(periodDto.Id);          

            indicator.Report(null, 0, "Получение данных (Количество: {0})".FormatUsing(primarySources.Count));
            using (var dataSource = new PaymentDocumentDataSource(this.container, periodEntity, primarySources, PaymentDocumentType.Individual))
            {
                indicator.Report(null, 20, "Создание слепков (Количество: {0})".FormatUsing(primarySources.Count));
                dataSource.CreateSnapshots(isZeroPaymentDoc);

                var accountData = dataSource.DocumentData.GetAccountData();
                //todo загадка. Зачем это?
                accountData = primarySources.Select(x => x.AccountId).Join(accountData, id => id, x => x.HolderId, (id, obj) => obj).ToList();
          
                var assemblyService = this.container.Resolve<ICompositionService>(
                    this.config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.CompositionOptions.CompositionType.ToString());

                try
                {
                    using (var reportMan = new PaymentDocReportManager(this.templateCopyRepo))
                    {
                        uint percent = 60;

                        foreach (var portion in assemblyService.GetAccountPortion(accountData, this.config))
                        {
                            string filename = assemblyService.GetFileName(portion);

                            var portionSize = portion.Count();
                            indicator.Report(null, percent, "Генерация отчета (Количество: {0} из {1})".FormatUsing(portionSize, primarySources.Count));
                            percent += (uint)(portionSize * 20 / primarySources.Count);

                            var args = new ReportArgs
                            {
                                DocumentsPath = filesPath,
                                Snapshots = portion,
                                FileName = filename
                            };

                            this.GenerateReport(
                                this.config.PaymentDocumentConfigContainer,
                                reportMan,
                                args,
                                new BaseInvoiceReport(args.Snapshots),
                                periodEntity);

                            this.SavePaymentDocumentLog(uid, path, portion);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.appLogger.LogError(exception.ToString());
                    throw;
                }
                finally
                {
                    this.container.Release(assemblyService);
                }

                this.sessionProv.CloseCurrentSession();

                indicator.Report(null, 80, "Сохранение слепков (Идентификаторы: {0})".FormatUsing(primarySources.Count));
                dataSource.Save();

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
        public override string ExecutorCode { get { return PhysicalOwnerDocumentExecutor.Id; } }
    }
}