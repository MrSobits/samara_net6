namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2023091300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023091300")]
    [MigrationDependsOn(typeof(Version_2023090600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("CHANGE_BASIS_TYPE", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "CHANGE_BASIS_TYPE");
        }
    }
}