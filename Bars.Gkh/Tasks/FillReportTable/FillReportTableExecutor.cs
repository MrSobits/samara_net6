using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Dapper;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.Tasks
{
    public class FillReportTableExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }
        public ISessionProvider SessionProvider { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        
        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var periods = this.ChargePeriodDomain.GetAll().ToList();
                var count = 0;
                var junePeriods = this.ChargePeriodDomain.GetAll().Where(x => x.Id >= 3365).ToList();

                using (var session = this.SessionProvider.OpenStatelessSession())
                {
                    var connection = session.Connection;

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            indicator.Report(null, 0, "Подготовка к заполнению");
                            var query = "select prepareperioddata()";
                            connection.Execute(query, transaction: transaction, commandTimeout: 99999999);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    foreach (var period in periods)
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                count++;
                                indicator.Report(null, 40, $"Заполняем таблицу");
                                var query = $@"select getperioddata({period.Id})";
                                connection.Execute(query, transaction: transaction, commandTimeout: 99999999);
                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                    foreach (var per in junePeriods)
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                count++;
                                indicator.Report(null, 80, "Рассчитываем сальдо");
                                var query = $@"select fillsaldotable({per.Id})";
                                connection.Execute(query, transaction: transaction, commandTimeout: 99999999);
                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
                return new BaseDataResult(true, "Success");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
