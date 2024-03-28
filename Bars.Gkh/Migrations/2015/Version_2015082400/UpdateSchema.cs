namespace Bars.Gkh.Migrations._2015.Version_2015082400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015082400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015082100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            if (!Database.TableExists("OVRHL_REG_OPERATOR"))
            {
                Database.AddEntityTable(
                    "OVRHL_REG_OPERATOR",
                    new RefColumn(
                        "CONTRAGENT_ID",
                        ColumnProperty.NotNull,
                        "OVRHL_REG_OPER_CNTR",
                        "GKH_CONTRAGENT",
                        "ID"));
            }

            if (!Database.TableExists("GKH_PUBLIC_SERVORG"))
            {
                Database.AddEntityTable(
                    "GKH_PUBLIC_SERVORG",
                    new Column("CONTRAGENT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                    new Column("ORG_STATE_ROLE", DbType.Int32, 4, ColumnProperty.NotNull),
                    new Column("DESCRIPTION", DbType.String, 500),
                    new Column("ACTIVITY_TERMINATION", DbType.Int32, 4, ColumnProperty.NotNull),
                    new Column("DESCRIPTION_TERM", DbType.String, 500),
                    new Column("DATE_TERMINATION", DbType.DateTime));
                Database.AddIndex("IND_GKH_PUBLSERV_CNTR", false, "GKH_PUBLIC_SERVORG", "CONTRAGENT_ID");
                Database.AddForeignKey(
                    "FK_GKH_PUBLSERV_CNTR",
                    "GKH_PUBLIC_SERVORG",
                    "CONTRAGENT_ID",
                    "GKH_CONTRAGENT",
                    "ID");
            }
        }
    }
}