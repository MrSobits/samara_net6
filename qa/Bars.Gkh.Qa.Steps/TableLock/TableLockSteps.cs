namespace Bars.Gkh.Qa.Steps
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using NHibernate.Linq;

    using TechTalk.SpecFlow;

    [Binding]
    internal class TableLockSteps : BindingBase
    {
        [When(@"пользователь снимает блокировки в Реестре блокировки таблиц")]
        public void ЕслиПользовательСнимаетБлокировкиВРеестреБлокировкиТаблиц()
        {
            var tableLocks =
                Container.Resolve<ISessionProvider>()
                    .GetCurrentSession()
                    .Query<TableLock>()
                    .Select(x => new { x.Action, x.LockStart, x.TableName })
                    .ToArray();

            var locker = Container.Resolve<IBatchTableLocker>();

            foreach (var tableLock in tableLocks)
            {
                locker.With(tableLock.TableName, tableLock.Action);
            }

            locker.Unlock();
        }

        [Then(@"в реестре блокировок присутствуют записи по блокировкам")]
        public void ТоВРеестреБлокировокПрисутствуютЗаписиПоБлокировкам()
        {
            Container.Resolve<ISessionProvider>().GetCurrentSession().Query<TableLock>().Select(x => new { x.Action, x.LockStart, x.TableName })
                .Any().Should().BeTrue("в реестре блокировок должны присутствовать записи по блокировкам");
        }
    }
}
