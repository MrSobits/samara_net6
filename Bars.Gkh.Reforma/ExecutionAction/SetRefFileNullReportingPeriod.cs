namespace Bars.Gkh.Reforma.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;

    /// <summary>
    /// Реформа ЖКХ - Проставить текущий период синхронизации файлов
    /// </summary>
    public class SetRefFileNullReportingPeriod : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Действие проставляет текущий активный период синхронизации у сущности RefFile, если он не был установлен";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Реформа ЖКХ - Проставить текущий период синхронизации файлов";

        /// <summary>
        /// Домен-сервис "Файлы в реформе"
        /// </summary>
        public IDomainService<RefFile> RefFileDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Отчётный период в реформе"
        /// </summary>
        public IDomainService<ReportingPeriodDict> ReportingPeriodDomain { get; set; }

        /// <summary>
        /// Провайдер сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var reportingPeriod = this.ReportingPeriodDomain.GetAll()
                .Where(x => x.Synchronizing)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (reportingPeriod.IsNull())
            {
                return BaseDataResult.Error("Отсутствует активный период синхронизации с Реформой ЖКХ");
            }

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.CreateSQLQuery("update RFRM_FILE set REPORTING_PERIOD_ID = :id where REPORTING_PERIOD_ID is null")
                            .SetParameter("id", reportingPeriod.Id)
                            .ExecuteUpdate();

                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        return BaseDataResult.Error(exception.Message);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}