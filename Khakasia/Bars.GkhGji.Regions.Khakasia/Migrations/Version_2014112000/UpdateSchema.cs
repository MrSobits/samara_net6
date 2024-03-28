namespace Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014112000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014092900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_PROTOCOL_LONGDESC",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GJI_PROTOCOL_LONGDESC", "GJI_PROTOCOL", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTSURVEY_LONGDESC");
        }
    }
}