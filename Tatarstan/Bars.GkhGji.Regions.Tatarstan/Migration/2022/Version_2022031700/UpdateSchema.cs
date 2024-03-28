namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022031700
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022031700")]
    [MigrationDependsOn(typeof(Version_2022022400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ChangeNullableColumn(false);
        }

        public override void Down()
        {
            ChangeNullableColumn(true);
        }

        private void ChangeNullableColumn (bool notNull)
            => this.Database.ChangeColumnNotNullable("GJI_DICT_CONTROL_TYPES", "TOR_ID", notNull);
    }
}
