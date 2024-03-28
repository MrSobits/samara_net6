namespace Bars.GkhRf.Migrations.Version_2015121100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015121100")]
    [MigrationDependsOn(typeof(Version_2015112300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("RF_TRANSFER_RECORD", new Column("IS_CALCULATING", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_RECORD", "IS_CALCULATING");
        }
    }
}