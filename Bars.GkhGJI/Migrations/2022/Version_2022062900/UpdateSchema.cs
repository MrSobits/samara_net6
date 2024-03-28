namespace Bars.GkhGji.Migrations._2022.Version_2022062900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022062900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022062400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_ACTCHECK", new RefColumn("SIGNATORY_INSPECTOR_ID", "GJI_SIGNATORY_INSPECTOR_INSP_ID", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddColumn("GJI_DICISION", new Column("MINUTE_OF_DICISION", DbType.Int32));
            Database.AddColumn("GJI_DICISION", new Column("HOUR_OF_DICISION", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICISION", "MINUTE_OF_DICISION");
            Database.RemoveColumn("GJI_DICISION", "MINUTE_OF_PROCEEDINGS");
            Database.RemoveColumn("GJI_ACTCHECK", "SIGNATORY_INSPECTOR_ID");
        }
    }
}