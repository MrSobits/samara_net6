namespace Bars.Gkh.Migrations.Version_2014070300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014070300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("ANSWER_DATE", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "ANSWER_DATE");
        }
    }
}