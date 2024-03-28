namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018041100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018041100")]
    [MigrationDependsOn(typeof(Version_2018040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("STATE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_SUB_RECORD", DbType.Boolean));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("STATE_CHANGE_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "STATE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_SUB_RECORD");
            Database.RemoveColumn("OVRHL_VERSION_REC", "STATE_CHANGE_DATE");
        }
    }
}