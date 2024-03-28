namespace Bars.GkhGji.Migrations.Version_2014022000
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("CASE_NUMBER", DbType.String, 50));
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("CASE_DATE", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CASE_NUMBER");
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CASE_DATE");
        }
    }
}