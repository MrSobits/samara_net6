namespace Bars.B4.Modules.FIAS.Migrations.Version_2018082700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018082700")]
    [MigrationDependsOn(typeof(Version_2018082400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("B4_FIAS_ADDRESS", "EST_STATUS", DbType.Byte, ColumnProperty.NotNull, 2);
        }

        public override void Down()
        {
            Database.RemoveColumn("B4_FIAS_ADDRESS", "EST_STATUS");
        }
    }
}