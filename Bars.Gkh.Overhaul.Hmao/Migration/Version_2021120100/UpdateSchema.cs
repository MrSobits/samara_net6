namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2021120100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021011100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("REMARK", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "REMARK");
        }
    }
}