namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018050100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018050100")]
    [MigrationDependsOn(typeof(Version_2018042500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "STATE");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_SUB_RECORD");

            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("STATE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_STAGE1_VERSION", new Column("STATE_CHANGE_DATE", DbType.DateTime));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_SHOW", DbType.Boolean, ColumnProperty.NotNull, true));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "STATE");
            Database.RemoveColumn("OVRHL_STAGE1_VERSION", "IS_SUB_RECORD");
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_SHOW");

            Database.AddColumn("OVRHL_VERSION_REC", new Column("STATE", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_VERSION_REC", new Column("STATE_CHANGE_DATE", DbType.DateTime));
        }
    }
}