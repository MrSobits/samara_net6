namespace Bars.GkhGji.Migrations._2020.Version_2020072000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020072000")]
    [MigrationDependsOn(typeof(Version_2020070900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_APPEAL_CITIZENS_FILES", new Column("name", DbType.String.WithSize(500), ColumnProperty.NotNull));
        }

        public override void Down()
        {

        }
    }
}