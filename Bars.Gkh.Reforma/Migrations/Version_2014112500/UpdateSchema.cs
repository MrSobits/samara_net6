namespace Bars.Gkh.Reforma.Migrations.Version_2014112500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RFRM_REPORTING_PERIOD", new Column("SYNCHRONIZING", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddPersistentObjectTable(
                "RFRM_CHANGED_MAN_ORG",
                new RefColumn("MAN_ORG_ID", ColumnProperty.NotNull, "RFRM_CHANGED_MANOR_MANOR", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("PERIOD_DI_ID", ColumnProperty.Null, "RFRM_CHANGED_MANOR_PER", "DI_DICT_PERIOD", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("RFRM_CHANGED_MAN_ORG");
            Database.RemoveColumn("RFRM_REPORTING_PERIOD", "SYNCHRONIZING");
        }
    }
}