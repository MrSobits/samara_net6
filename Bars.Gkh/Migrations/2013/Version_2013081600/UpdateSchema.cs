namespace Bars.Gkh.Migrations.Version_2013081600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013081600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013081501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MORG_CONTR_REL", new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MORG_CONTR_REL", "EXTERNAL_ID");
        }
    }
}