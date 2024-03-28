namespace Bars.Gkh.Migrations.Version_2013122601
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013122600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("APPEAL_CODE", DbType.String, 30));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "APPEAL_CODE");
        }
    }
}