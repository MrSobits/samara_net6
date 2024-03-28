namespace Bars.Gkh.Migrations._2018.Version_2018050100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018050100")]
    [MigrationDependsOn(typeof(Version_2018042000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_WORK", new Column("IS_PSD", DbType.Boolean, ColumnProperty.None, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_WORK", "IS_PSD");
        }
    }
}