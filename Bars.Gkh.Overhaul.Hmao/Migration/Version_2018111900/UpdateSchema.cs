namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018111900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018111900")]
    [MigrationDependsOn(typeof(Version_2018091400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
           Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_SUBPROGRAM", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {           
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_SUBPROGRAM");           
        }
    }
}