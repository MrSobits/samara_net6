namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071700
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_ACTCHECK_SMOL_ROBJECT_DESCR",
                new RefColumn("ACT_CHECK_ROBJECT_ID", ColumnProperty.NotNull, "ACT_CHECK_RO_DESCR", "GJI_ACTCHECK_ROBJECT", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));

            Database.AddEntityTable(
                "GJI_PRESCRIPTION_VIOLAT_SMOL_DESCR",
                new RefColumn("PRESCR_VIOLAT_ID", ColumnProperty.NotNull, "PRESC_VIOLAT_DESCR", "GJI_PRESCRIPTION_VIOLAT", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null),
                new Column("ACTION", DbType.Binary, ColumnProperty.Null));

            Database.AddEntityTable(
                "GJI_PROTOCOL_SMOL_VIOL_DESCR",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "PROTOCOL_VIOL_DESCR", "GJI_PROTOCOL_SMOL", "ID"),
                new Column("VIOLATION_DESCR", DbType.Binary, ColumnProperty.Null),
                new Column("EXPLANATIONS_COMMENTS", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PROTOCOL_SMOL_VIOL_DESCR");
            Database.RemoveTable("GJI_PRESCRIPTION_VIOLAT_SMOL_DESCR");
            Database.RemoveTable("GJI_ACTCHECK_SMOL_ROBJECT_DESCR");
        }
    }
}