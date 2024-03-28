namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using Microsoft.Extensions.Logging;
    using B4.Modules.FileStorage;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    using Dapper;

    using DomainService.PersonalAccount;
    using Gkh.Domain;
    using Log;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Выполняет задачу формирования реестра должников
    /// </summary>
    public class DebtorsTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        private readonly IWindsorContainer container;
        private readonly IDebtorCalcService calcService;
        private readonly ILogger logger;
        private readonly IProcessLog processLog;

        public DebtorsTaskExecutor(
            IWindsorContainer container,
            IDebtorCalcService calcService,
            ILogger logger,
            IProcessLog processLog)
        {
            this.container = container;
            this.calcService = calcService;
            this.logger = logger;
            this.processLog = processLog;
        }

        public IDataResult Execute(BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            this.processLog.SetLogName("Формирование неплательщиков");

            this.Clear(@params);

            var sessionProvider = this.container.Resolve<ISessionProvider>();

            FileInfo file;

            try
            {
                var endIndex = this.calcService.GetQuery().Count();
                const int step = 10000;

                this.processLog.Info("Расчет начат. Всего счетов {0}".FormatUsing(endIndex));

                this.calcService.InitRecalcHistory();

                for (var startIndex = 0; startIndex < endIndex; startIndex += step)
                {
                    var resultList = this.calcService.GetDebtors(startIndex, step, this.processLog);

                    TransactionHelper.InsertInManyTransactions(this.container, resultList, resultList.Count, true, true);

                    int executed = Math.Min(startIndex + step, endIndex);

                    indicator.Indicate(
                        null,
                        (ushort)((float)executed * 100 / endIndex),
                        "Обработано {0} из {1} ЛС".FormatUsing(executed, endIndex));

                    this.logger.LogInformation("(Формирование неплательщиков) Обработано {0} из {1} ЛС".FormatUsing(executed, endIndex));

                    sessionProvider.GetCurrentSession().Clear();
                }

                //Вызов хранимой функции удаления должников по определенным критериям
                //Выделено в функцию для возможного изменения на лету без апдейта системы
                sessionProvider.GetCurrentSession().Connection.Query("select debtor_cleanup()");
            }
            finally
            {
                try
                {
                    file = this.processLog.Save();
                }
                catch
                {
                    this.logger.LogError("Ошибка сохранения лога операции");
                    file = null;
                }
            }

            return new BaseDataResult(file.Return(x => (long?)x.Id));
        }

        public IDataResult Clear(BaseParams baseParams)
        {
            var sessions = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(sessions))
            {
                sessions.GetCurrentSession().CreateQuery("delete from Debtor").ExecuteUpdate();
            }

            return new BaseDataResult();
        }
    }
}