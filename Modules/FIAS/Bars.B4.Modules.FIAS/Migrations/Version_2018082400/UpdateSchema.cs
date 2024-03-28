namespace Bars.B4.Modules.FIAS.Migrations.Version_2018082400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018082400")]
    [MigrationDependsOn(typeof(Version_2017090600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("B4_FIAS_HOUSE", "EST_STATUS", DbType.Byte, ColumnProperty.NotNull, 2);
        }

        public override void Down()
        {
            Database.RemoveColumn("B4_FIAS_HOUSE", "EST_STATUS");
        }
    }
}