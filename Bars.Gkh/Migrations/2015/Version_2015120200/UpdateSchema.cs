namespace Bars.Gkh.Migrations._2015.Version_2015120200
    {
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015112500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_EMERGENCY_OBJECT", new Column("EXEMPTIONS_BASIS", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "EXEMPTIONS_BASIS");
        }
    }
}
