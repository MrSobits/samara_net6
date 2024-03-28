namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014052800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014052601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("MOTIV_REQUEST_NUM", DbType.String, 100));
            Database.AddColumn("GJI_NSO_DISPOSAL", new Column("MOTIV_REQUEST_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "MOTIV_REQUEST_NUM");
            Database.RemoveColumn("GJI_NSO_DISPOSAL", "MOTIV_REQUEST_DATE");
        }
    }
}