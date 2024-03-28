namespace Bars.Gkh.Migrations._2016.Version_2016101400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Map;

    /// <summary>
    /// Миграция 2016101400
    /// </summary>
    [Migration("2016101400")]
    [MigrationDependsOn(typeof(Version_2016092200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //TODO не работает с DbType.DateTimeOffset
            //this.Database.AddEntityTable(ExecutionActionHistoryMap.TableName,
            //   new Column(ExecutionActionHistoryMap.CodeColumnName, DbType.String, 255),
            //   new Column(ExecutionActionHistoryMap.JobIdColumnName, DbType.Guid),
            //   new Column(ExecutionActionHistoryMap.CreateDateColumnName, DbType.DateTime),
            //   new Column(ExecutionActionHistoryMap.StartDateColumnName, DbType.DateTime),
            //   new Column(ExecutionActionHistoryMap.EndDateColumnName, DbType.DateTime),
            //   new Column(ExecutionActionHistoryMap.DataResultColumnName, DbType.Binary),
            //   new Column(ExecutionActionHistoryMap.StatusColumnName, DbType.Int32));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable(ExecutionActionHistoryMap.TableName);
        }
    }
}
