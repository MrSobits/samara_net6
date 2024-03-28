namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014071400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014061900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_SMOL", "VIOLATION_DESCIPTION", DbType.String, 20000);
            Database.AddColumn("GJI_PROTOCOL_SMOL", "EXPLANATIONS_COMMENTS", DbType.String, 20000);
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_SMOL", "EXPLANATIONS_COMMENTS");
            Database.RemoveColumn("GJI_PROTOCOL_SMOL", "VIOLATION_DESCIPTION");
        }
    }
}