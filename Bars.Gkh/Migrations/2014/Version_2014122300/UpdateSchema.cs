namespace Bars.Gkh.Migrations.Version_2014122300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014121902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_MANORG_LICENSE",
                    new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GKH_MANORG_LICENSE_C", "GKH_CONTRAGENT", "ID"),
                    new RefColumn("REQUEST_ID", ColumnProperty.NotNull, "GKH_MANORG_LICENSE_R", "GKH_CONTRAGENT", "ID"),
                    new RefColumn("STATE_ID", ColumnProperty.Null, "GKH_MANORG_LICENSE_S", "B4_STATE", "ID"),
                    new Column("LIC_NUMBER", DbType.String, 100),
                    new Column("LIC_NUM", DbType.Int64, 22),
                    new Column("DATE_ISSUED", DbType.DateTime),
                    new Column("NUM_DISPOSAL", DbType.String, 100),
                    new Column("DATE_DISPOSAL", DbType.DateTime),
                    new Column("DATE_REGISTER", DbType.DateTime),
                    new Column("DATE_TERMINATION", DbType.DateTime),
                    new Column("TYPE_TERMINATION", DbType.Int16, 4, ColumnProperty.NotNull, 0));

            Database.AddEntityTable("GKH_MANORG_LIC_DOC",
                    new RefColumn("LIC_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_DOC_L", "GKH_MANORG_LIC_DOC", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_DOC_F", "B4_FILE_INFO", "ID"),
                    new Column("DOC_TYPE", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                    new Column("DOC_NUMBER", DbType.String, 100),
                    new Column("DOC_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MANORG_LICENSE");
            Database.RemoveTable("GKH_MANORG_LIC_DOC");
        }
    }
}