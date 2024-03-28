namespace Bars.GkhGji.Migrations.Version_2014021700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("ARCHIVE_NUMBER", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "ARCHIVE_NUMBER");
        }
    }
}