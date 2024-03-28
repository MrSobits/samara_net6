namespace Bars.Gkh.Migrations.Version_2015020200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015020200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MANORG_REQ_PROVDOC", new Column("LIC_PROVDOC_NUMBER", DbType.String, 100));
            Database.AddColumn("GKH_MANORG_REQ_PROVDOC", new Column("LIC_PROVDOC_DATE", DbType.DateTime));
            Database.AddRefColumn("GKH_MANORG_REQ_PROVDOC", new RefColumn("LIC_PROVDOC_FILE_ID", "LIC_PROVDOC_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MANORG_REQ_PROVDOC", "LIC_PROVDOC_FILE_ID");
            Database.RemoveColumn("GKH_MANORG_REQ_PROVDOC", "LIC_PROVDOC_DATE");
            Database.RemoveColumn("GKH_MANORG_REQ_PROVDOC", "LIC_PROVDOC_NAME");
        }
    }
}