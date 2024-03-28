namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014071600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014062000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_ACTCHECK_RO_DISCR",
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null),
                new RefColumn("ACTCHECK_RO_ID", ColumnProperty.NotNull, "GJI_ACT_RO_DISCR", "GJI_ACTCHECK_ROBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTCHECK_RO_DISCR");
        }
    }
}