namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022020500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2022020500")]
    [MigrationDependsOn(typeof(Version_2022020400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "GJI_ACTCHECK_ACTION_VIOLATION";
        private const string NewColumn = "GJI_DICT_VIOLATION_ID";
        private readonly Column oldColumn = new Column("VIOLATION", DbType.String.WithSize(255));

        public override void Up()
        {
            this.Database.AddRefColumn(TableName,
                new RefColumn(NewColumn, ColumnProperty.NotNull, "FK_GJI_ACTCHECK_ACTION_VIOLATION_GJI_DICT_VIOLATION", "GJI_DICT_VIOLATION", "ID"));
            this.Database.RemoveColumn(TableName, oldColumn.Name);
        }

        public override void Down()
        {
            this.Database.AddColumn(TableName, oldColumn);
            this.Database.RemoveColumn(TableName, NewColumn);
        }
    }
}
