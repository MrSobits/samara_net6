namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;

    public class UpdateFiasPlaceNameLengthAction : BaseExecutionAction
    {
        public override string Description => "Увеличивает размер столбца b4_fias_address.place_name с 50 до 100. " +
            "Сделано в виде Action потому что обновление типа столбца без удаления вьюх, которые этот столбец используют, крайне затруднительно";

        public override string Name => "Увеличения размера столбца b4_fias_address.place_name с 50 до 100";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var provider = this.Container.Resolve<ISessionProvider>();

            var view = this.Container.Resolve<IViewCollection>("RegopViewCollection");
            var dialect = ApplicationContext.Current.Configuration.DbDialect;
            var dbmsKind = dialect == DbDialect.PostgreSql ? DbmsKind.PostgreSql : DbmsKind.Oracle;

            try
            {
                var session = provider.GetCurrentSession();

                var dropQuery = view.GetDropAll(dbmsKind);

                session.CreateSQLQuery(string.Join(";", dropQuery)).ExecuteUpdate();
                session.CreateSQLQuery(
                    dbmsKind == DbmsKind.PostgreSql
                        ? "alter table b4_fias_address alter column place_name type character varying(100);"
                        : "alter table b4_fias_address modify ( place_name varchar(100) );").ExecuteUpdate();

                var createQuery = view.GetCreateAll(dbmsKind);
                session.CreateSQLQuery(string.Join(";", createQuery)).ExecuteUpdate();

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, "{0}\n{1}\n{2}".FormatUsing(exception.Message, exception.GetType().FullName, exception.StackTrace));
            }
            finally
            {
                this.Container.Release(view);
                this.Container.Release(provider);
            }
        }
    }
}