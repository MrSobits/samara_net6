namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022022400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022022400")]
    [MigrationDependsOn(typeof(Version_2022020500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_VALIDITY_DOC_PERIOD";

        public override void Up()
        {
            this.Database.AddEntityTable(TableName,
                new Column("TYPE_DOC", DbType.Int16, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.Date));
        }

        public override void Down()
        {
            this.Database.RemoveTable(TableName);
        }
    }
}
