namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_REQUIREMENT", new RefColumn("ARTICLELAW_ID","GJI_REQUIREMENT_ARTLAW", "GJI_DICT_ARTICLELAW", "ID"));
            Database.RemoveColumn("GJI_REQUIREMENT", "CLAUSE");

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_REQUIREMENT", "ARTICLELAW_ID");
            Database.AddColumn("GJI_REQUIREMENT", new Column("CLAUSE", DbType.String, 500));
        }
    }
}
