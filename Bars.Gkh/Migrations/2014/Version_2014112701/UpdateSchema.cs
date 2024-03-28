namespace Bars.Gkh.Migrations.Version_2014112701
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("DEADLINE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "DEADLINE");
        }
    }
}