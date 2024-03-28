namespace Bars.Gkh.RegOperator.Tasks.Period.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;
    using Authentification;
    using Gkh.Entities;

    public class PeriodCloseCheckTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        public PeriodCloseCheckTaskExecutor(IWindsorContainer container)
        {
            this.container = container;
        }

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var resultId = @params.Params.GetAsId("resultId");

            var resultDomain = this.container.ResolveDomain<PeriodCloseCheckResult>();
            var fileMgr = this.container.Resolve<IFileManager>();
            var userManager = this.container.Resolve<IGkhUserManager>();
            var logDomain = this.container.ResolveDomain<LogOperation>();
            IPeriodCloseChecker checker = null;
            try
            {
                var result = resultDomain.Get(resultId);
                if (result == null)
                {
                    return new BaseDataResult(false, "Не найдена запись в таблице результатов проверок периода");
                }

                if (result.CheckState != PeriodCloseCheckStateType.Pending)
                {
                    return new BaseDataResult(false, "Неверный статус записи в таблице результатов проверок периода");
                }

                checker = this.container.Resolve<IPeriodCloseChecker>(result.Impl);
                if (checker == null)
                {
                    return new BaseDataResult(false, "Не найдена проверка с указанным кодом: {0}".FormatUsing(result.Impl));
                }

                result.CheckState = PeriodCloseCheckStateType.Running;
                resultDomain.Update(result);

                try
                {
                    var checkResult = checker.Check(result.Period.Id);

                    result.PersAccGroup = this.GetOrCreateGroup(checker);
                    result.Note = checkResult.Note;

                    if (checkResult.Log.Length > 0)
                    {
                        result.LogFile = fileMgr.SaveFile(result.Name, "csv", Encoding.GetEncoding(1251).GetBytes(checkResult.Log.ToString()));
                    }

                    if (checkResult.FullLog.Length > 0)
                    {
                        var file = fileMgr.SaveFile(result.Name, "csv", Encoding.GetEncoding(1251).GetBytes(checkResult.FullLog.ToString()));

                        result.FullLogFile = file;

                        logDomain.Save(
                            new LogOperation
                            {
                                OperationType = LogOperationType.PerformingControlChecks,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now,
                                User = userManager.GetActiveUser(),
                                Comment = $"Контрольная проверка {result.Name}",
                                LogFile = fileMgr.SaveFile(result.Name, "csv", Encoding.GetEncoding(1251).GetBytes(checkResult.FullLog.ToString()))
                    });
                    }

                    this.SetGroupMembers(result.PersAccGroup, checkResult.InvalidAccountIds ?? new long[0]);
                    result.CheckState = checkResult.Success ? PeriodCloseCheckStateType.Success : PeriodCloseCheckStateType.Error;
                }
                catch (Exception e)
                {
                    result.CheckState = PeriodCloseCheckStateType.Exception;
                    result.LogFile = fileMgr.SaveFile(result.Name, "txt", Encoding.UTF8.GetBytes(e.ToString()));
                }

                resultDomain.Update(result);
            }
            finally
            {
                this.container.Release(resultDomain);
                this.container.Release(fileMgr);
                this.container.Release(userManager);
                this.container.Release(logDomain);
                if (checker != null)
                {
                    this.container.Release(checker);
                }
            }

            return new BaseDataResult();
        }

        public string ExecutorCode { get; }

        private PersAccGroup GetOrCreateGroup(IPeriodCloseChecker checker)
        {
            var service = this.container.ResolveDomain<PersAccGroup>();
            try
            {
                var g = service.GetAll().Where(x => x.IsSystem == YesNo.Yes).FirstOrDefault(x => x.Name == checker.Name);
                if (g == null)
                {
                    g = new PersAccGroup
                    {
                        IsSystem = YesNo.Yes,
                        Name = checker.Name
                    };

                    service.Save(g);
                }

                return g;
            }
            finally
            {
                this.container.Release(service);
            }
        }

        private void SetGroupMembers(PersAccGroup group, IEnumerable<long> ids)
        {
            var sessionProvider = this.container.Resolve<ISessionProvider>();
            var relationDomain = this.container.ResolveDomain<PersAccGroupRelation>();
            try
            {
                var existingIds = relationDomain.GetAll().Where(x => x.Group.Id == group.Id).Select(x => x.PersonalAccount.Id).ToList();
                var toDelete = relationDomain.GetAll().Where(x => x.Group.Id == group.Id).WhereContains(x => x.PersonalAccount.Id, existingIds.Except(ids)).Select(x => x.Id).ToList();

                var session = sessionProvider.GetCurrentSession();
                foreach (var toDeletePart in toDelete.Split(1000))
                {
                    var hql = session.CreateQuery("delete PersAccGroupRelation where Id in (:ids)");
                    hql.SetParameterList("ids", toDeletePart);
                    hql.ExecuteUpdate();
                }

                var toCreate = new List<PersAccGroupRelation>();
                toCreate.AddRange(
                    ids.Except(existingIds).Select(id => new PersAccGroupRelation { Group = group, PersonalAccount = new BasePersonalAccount { Id = id } }));

                TransactionHelper.InsertInManyTransactions(this.container, toCreate, useStatelessSession: true);
            }
            finally
            {
                this.container.Release(sessionProvider);
                this.container.Release(relationDomain);
            }
        }
    }
}