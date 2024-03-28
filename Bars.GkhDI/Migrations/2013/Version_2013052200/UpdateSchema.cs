namespace Bars.GkhDi.Migrations.Version_2013052200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013052200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Выделил в отдельную сущность меры
            Database.AddEntityTable(
                "DI_ADMIN_RESP_ACTION",
                new Column("ACTION", DbType.String, 2000),
                new RefColumn("ADMIN_RESP_ID", ColumnProperty.NotNull, "ADMIN_RESP_ACTIONS", "DI_ADMIN_RESP", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("DI_ADMIN_RESP_ACTION");
        }
    }
}