namespace Bars.GkhGji.Migrations.Version_2014060600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014060500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                "GJI_RESOLUTION",
                new Column("TERMINATION_BASEMENT", DbType.Int32, 4, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "TERMINATION_BASEMENT");
        }
    }
}