namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021111800
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021111800")]
    [MigrationDependsOn(typeof(Version_2021111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_EXECUTANT", new Column("ONAPPROVAL", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_EXECUTANT", "ONAPPROVAL");
        }

    }
}