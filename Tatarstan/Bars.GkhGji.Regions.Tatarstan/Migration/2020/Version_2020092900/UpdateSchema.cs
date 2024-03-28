namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020092900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020092900")]
    [MigrationDependsOn(typeof(Version_2020091700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_PRESCRIPTION_CANCEL_TATARSTAN";

        /// <inheritdoc />
        public override void Up()
        {
            var columns = new[]
            {
                new Column("DATE", DbType.Date, 10, ColumnProperty.Null),
                new Column("DOCUMENT_NUMBER", DbType.String, 255),
                new Column("OUT_MAIL_DATE", DbType.Date, 10, ColumnProperty.Null),
                new Column("OUT_MAIL_NUMBER", DbType.String, 255),
                new Column("NOTIFICATION_TRANSMISSION", DbType.Int32),
                new Column("NOTIFICATION_RECEIVE", DbType.Int32),
                new Column("NOTIFICATION_TYPE", DbType.Int32),
                new Column("PROLONGATION_DATE", DbType.Date, 10, ColumnProperty.Null),
            };

            this.Database.AddJoinedSubclassTable(UpdateSchema.TableName, "GJI_PRESCRIPTION_CANCEL", "ID", columns);

            this.AddEntries();
        }

        private void AddEntries()
        {
            var sql = $@"
                insert into {UpdateSchema.TableName} as t (id, DATE, DOCUMENT_NUMBER, OUT_MAIL_DATE, OUT_MAIL_NUMBER, NOTIFICATION_TRANSMISSION, NOTIFICATION_RECEIVE, NOTIFICATION_TYPE, PROLONGATION_DATE)
                select id, null, null, null, null, null, null, null, null from GJI_PRESCRIPTION_CANCEL;";

            this.Database.ExecuteQuery(sql);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TableName);
        }
    }
}