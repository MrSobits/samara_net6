namespace Bars.Gkh1468.Migrations.Version_2015051500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2014120500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                "GKH_PSTRUCT_META_ATTR",
                new Column("USE_PERC_CALC", DbType.Boolean, ColumnProperty.NotNull, (object)false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PSTRUCT_META_ATTR", "USE_PERC_CALC");
        }
    }
}