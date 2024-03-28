namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class VersionPriorityFixAction : BaseExecutionAction
    {
        public override string Description => "Исправление приоритета записей версий ДПКР (КАМЧАТКА)";

        public override string Name => "Исправление приоритета записей версий ДПКР (КАМЧАТКА)";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var stage3Domain = this.Container.ResolveDomain<VersionRecord>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var listForUpdate = new List<IEntity>();

            using (this.Container.Using(stage3Domain, programVersionDomain, sessionProvider))
            {
                //берем только основные версии
                var recordsMustUpdate = stage3Domain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .GroupBy(x => x.ProgramVersion.Id);

                foreach (var version in recordsMustUpdate)
                {
                    int prevNumber = 1;

                    foreach (var record in version.OrderBy(x => x.IndexNumber))
                    {
                        record.IndexNumber = prevNumber++;
                        listForUpdate.Add(record);
                    }
                }

                return this.Update(sessionProvider, listForUpdate);
            }
        }

        private BaseDataResult Update(ISessionProvider sessionProvider, IEnumerable<IEntity> forUpdate)
        {
            try
            {
                var session = sessionProvider.OpenStatelessSession();

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        forUpdate.ForEach(session.Update);

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            tr.Rollback();
                        }
                        catch (TransactionRollbackException ex)
                        {
                            throw new DataAccessException(ex.Message, exc);
                        }
                        catch (Exception e)
                        {
                            throw new DataAccessException(
                                string.Format(
                                    "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                    e.Message,
                                    e.StackTrace),
                                exc);
                        }

                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            return new BaseDataResult();
        }
    }
}