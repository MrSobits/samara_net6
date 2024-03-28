namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014041500
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014040900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_REQUIREMENT", new Column("INSPECTION_DATE", DbType.Date, ColumnProperty.Null));
            Database.AddColumn("GJI_REQUIREMENT", new Column("INSPECTION_HOUR", DbType.Int16, ColumnProperty.Null));
            Database.AddColumn("GJI_REQUIREMENT", new Column("INSPECTION_MIN", DbType.Int16, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_REQUIREMENT", "INSPECTION_DATE");
            Database.RemoveColumn("GJI_REQUIREMENT", "INSPECTION_HOUR");
            Database.RemoveColumn("GJI_REQUIREMENT", "INSPECTION_MIN");
        }
    }
}