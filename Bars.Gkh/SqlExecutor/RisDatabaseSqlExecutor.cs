namespace Bars.Gkh.SqlExecutor
{
    using Bars.B4.Application;

    /// <summary>
    /// Класс для выполнения sql-запросов к БД РИСа
    /// </summary>
    public class RisDatabaseSqlExecutor : SqlExecutor
    {
        private static readonly string risConnectionString = ApplicationContext.Current.Configuration.AppSettings.GetAs("RisDatabaseConnectionString", string.Empty);

        /// <inheritdoc />
        public RisDatabaseSqlExecutor()
            : base(RisDatabaseSqlExecutor.risConnectionString)
        {
        }
    }
}