namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018102600
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2018102600")]
    [MigrationDependsOn(typeof(Version_2018100100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(
                $@"UPDATE GJI_PROTOCOL_MVD
                   SET SERIAL_AND_NUMBER = REPLACE(SERIAL_AND_NUMBER, ' ', '');");
        }
    }
}