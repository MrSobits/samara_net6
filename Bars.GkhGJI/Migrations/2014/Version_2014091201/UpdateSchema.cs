namespace Bars.GkhGji.Migration.Version_2014091201
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014091200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION", "CLOSED", DbType.Int32, ColumnProperty.NotNull, 30);
            Database.AddColumn("GJI_PRESCRIPTION", "CLOSE_REASON", DbType.Int32);
            Database.AddColumn("GJI_PRESCRIPTION", "CLOSE_NOTE", DbType.String, 1000);


            Database.AddEntityTable("GJI_PRESCR_CLOSE_DOC",
                new Column("DOC_DATE", DbType.DateTime),
                new Column("DOC_TYPE", DbType.Int32, ColumnProperty.NotNull, 10),
                new Column("NAME", DbType.String, 300),
                new RefColumn("PRESCR_ID", ColumnProperty.NotNull, "CLOSE_DOC_PRESCR", "GJI_PRESCRIPTION", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "PRESC_CLOSE_DOC_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRESCR_CLOSE_DOC");
            Database.RemoveColumn("GJI_PRESCRIPTION", "CLOSED");
            Database.RemoveColumn("GJI_PRESCRIPTION", "CLOSE_REASON");
            Database.RemoveColumn("GJI_PRESCRIPTION", "CLOSE_NOTE");
        }
    }
}