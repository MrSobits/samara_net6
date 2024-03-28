namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030400")]
    [MigrationDependsOn(typeof(Version_2024022100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "COST_LIMIT", false);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "TYPE_FIN_SOURCE", false);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "END_DATE", false);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "RESPONSIBLE", false);
        }

        public override void Down()
        {
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "RESPONSIBLE", true);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "END_DATE", true);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "TYPE_FIN_SOURCE", true);
            Database.ChangeColumnNotNullable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK", "COST_LIMIT", true);
        }
    }
}