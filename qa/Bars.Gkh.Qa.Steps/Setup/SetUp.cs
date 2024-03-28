namespace Bars.Gkh.Qa.Steps.Setup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    using Newtonsoft.Json;

    using NHibernate.Linq;

    using TechTalk.SpecFlow;

    internal sealed class SetUp : BindingBase
    {
        [AfterScenario("ClearTableLock")]
        public void ClearTableLock()
        {
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
            var configService = Container.Resolve<IGkhConfigService>();
            var locker = Container.Resolve<IBatchTableLocker>();

            try
            {
                session.Clear();

                var configsDictionary = new Dictionary<string, object>();

                var tableLocks =
                    session.Query<TableLock>().Select(x => new { x.Action, x.LockStart, x.TableName }).ToArray();



                foreach (var tableLock in tableLocks)
                {
                    locker.With(tableLock.TableName, tableLock.Action);
                }

                locker.Unlock();

                configsDictionary["RegOperator.GeneralConfig.OperationLock.Enabled"] = "false";
                configsDictionary["RegOperator.GeneralConfig.OperationLock.PreserveLockAfterCalc"] = "false";

                var configString = JsonConvert.SerializeObject(configsDictionary);



                IDictionary<string, string> errors;

                var success = configService.UpdateConfigs(configString, out errors);

                if (!success)
                {
                    var errorsString = string.Join(", ", errors.Select(x => x.Value));

                    throw new SpecFlowException(
                        string.Format(
                            "Во время сохранения конфигураций по очистке блокироквок были отловленны ошибки {0}",
                            errorsString));
                }
            }
            finally
            {
                Container.Release(session);
                Container.Release(configService);
                Container.Release(locker);
            }
        }
    }
}
