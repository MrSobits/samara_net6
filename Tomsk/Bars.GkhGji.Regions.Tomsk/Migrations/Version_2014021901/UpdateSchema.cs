namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021901
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_REQUIREMENT", new Column("DOCUMENT_NUM", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_REQUIREMENT", "DOCUMENT_NUM");
        }
    }
}