namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014022101
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014022100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_REQUIREMENT", new Column("MATERIAL_SUB_DATE", DbType.Date));
            Database.AddColumn("GJI_REQUIREMENT", new Column("ADD_MATERIALS", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_REQUIREMENT", "MATERIAL_SUB_DATE");
            Database.RemoveColumn("GJI_REQUIREMENT", "ADD_MATERIALS");
        }
    }
}