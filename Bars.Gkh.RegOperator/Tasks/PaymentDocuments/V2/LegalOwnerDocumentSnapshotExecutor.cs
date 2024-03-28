namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Castle.Windsor;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Entities;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обработчик задачи создания слепков данных для юрлиц
    /// </summary>
    internal class LegalOwnerDocumentSnapshotExecutor : BaseDocumentExecutor
    {
        /// <summary>
        /// Идентификатор обработчика
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly IDomainService<ChargePeriod> periodDomainService;

        /// <summary>
        /// Конструктор обработчика
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="periodDomainService">Домен-сервис периодов</param>
        public LegalOwnerDocumentSnapshotExecutor(IWindsorContainer container, IDomainService<ChargePeriod> periodDomainService)
            : base(container)
        {
            this.container = container;
            this.periodDomainService = periodDomainService;
        }

        /// <summary>
        /// Выполнение обработки
        /// </summary>
        /// <param name="baseParams">Входные параметры отчета</param>
        /// <param name="executorContext">Контекст выполнения</param>
        /// <param name="indicator">Индикатор выполнения</param>
        /// <param name="cancellationToken">Обработчик отмены</param>
        /// <returns>Результат обработки</returns>
        public override IDataResult Execute(BaseParams baseParams,
            ExecutionContext executorContext,
            IProgressIndicator indicator,
            CancellationToken cancellationToken)
        {
            var isZeroPaymentDoc = baseParams.Params.GetAs<bool>("isZeroPaymentDoc");
            var periodDto = baseParams.Params.GetAs<PeriodDto>("periodDto");
            var primarySources = baseParams.Params.GetAs<List<PayDocPrimarySource>>("primarySources");
            var periodEntity = this.periodDomainService.Get(periodDto.Id);

            PaymentDocumentType type = primarySources.Count == 1 ? PaymentDocumentType.Legal : PaymentDocumentType.RegistryLegal;

            indicator.Report(null, 0, "Получение данных (Количество: {0})".FormatUsing(primarySources.Count));
            using (var docSource = new PaymentDocumentDataSource(this.container, periodEntity, primarySources, type))
            {
                indicator.Report(null, 30, "Создание слепков (Количество: {0})".FormatUsing(primarySources.Count));
                if (type == PaymentDocumentType.Legal)
                {
                    docSource.CreateSnapshots(isZeroPaymentDoc);
                }
                else
                {
                    docSource.CreateRegistrySnapshots(isZeroPaymentDoc);
                }

                indicator.Report(null, 60, "Сохранение слепков (Количество: {0})".FormatUsing(primarySources.Count));
                docSource.Save();
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