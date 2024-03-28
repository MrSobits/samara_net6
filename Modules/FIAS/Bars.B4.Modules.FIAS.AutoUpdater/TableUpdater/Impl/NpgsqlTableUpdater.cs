namespace Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater;
    using Castle.Windsor;

    using Npgsql;

    internal class NpgsqlTableUpdater : ITableUpdater
    {
        public IWindsorContainer Container { get; set; }
        public IDbConfigProvider DbConfigProvider { get; set; }

        /// <inheritdoc />
        public void UpdateRecords<T>(IEnumerable<T> newRecords, bool cleanBeforeUpdate = false)
            where T : PersistentObject
        {
            var toTempTable = !cleanBeforeUpdate;
            var records = newRecords.ToList();
            var tableUpdateHelper = this.Container.Resolve<ITableUpdateHelper<T>>();

            using (this.Container.Using(tableUpdateHelper))
            using (var connection = new NpgsqlConnection(this.DbConfigProvider.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        tableUpdateHelper.UpdateProcess(connection, records, toTempTable);
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
    }
}