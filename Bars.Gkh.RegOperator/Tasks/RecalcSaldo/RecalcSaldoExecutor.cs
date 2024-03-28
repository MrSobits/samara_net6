using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Entities;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.RegOperator.Entities;
using Bars.GkhCr.Entities;
using Dapper;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.RegOperator
{
    public class RecalcSaldoExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }
        public ISessionProvider SessionProvider { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<PersAccGroupRelation> PersAccGrpDomain { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> PersAccPerSummDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var accounts = this.PersAccGrpDomain.GetAll()
                    .Where(x => x.Group.Name == "Пересчет сальдо по ЛС" && x.Group.IsSystem == YesNo.Yes)
                    .Select(x => x.PersonalAccount.Id)
                    .ToList();

                using (var session = this.SessionProvider.OpenStatelessSession())
                {
                    var connection = session.Connection;

                    foreach (var acc in accounts)
                    {
                        long minPer = PersAccPerSummDomain.GetAll()
                            .Where(x => x.PersonalAccount.Id == acc)
                            .Min(x => x.Period.Id);

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                indicator.Report(null, 0, "Считаем сальдо");
                                var query = $@"select recalc_saldo({acc},{minPer})";
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
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            indicator.Report(null, 50, $"Обновляем задолженность");
                            var query = $@"select recalc_debt()";
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
                return new BaseDataResult(true, "Success");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
