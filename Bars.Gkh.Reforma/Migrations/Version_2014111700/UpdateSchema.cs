namespace Bars.Gkh.Reforma.Migrations.Version_2014111700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014111700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "RFRM_MANAGING_ORG",
                new RefColumn("MAN_ORG_ID", ColumnProperty.NotNull, "RFRM_MAN_ORG", "GKH_MANAGING_ORGANIZATION", "ID"),
                new Column("REQUEST_STATUS", DbType.Int16, ColumnProperty.NotNull),
                new Column("REQUEST_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PROCESS_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("RFRM_MANAGING_ORG");
        }
    }
}