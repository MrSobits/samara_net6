namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013112201
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013112200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("CORRECT_YEAR", DbType.Int16, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "CORRECT_YEAR");
        }
    }
}