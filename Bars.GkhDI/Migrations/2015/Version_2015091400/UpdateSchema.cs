namespace Bars.GkhDi.Migrations.Version_2015091400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015090800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_SERVICE_PROVIDER", new Column("NUMBER_CONTRACT", DbType.String));

            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("APPOINTMENT_COMMON_FACILITIES", DbType.String));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("AREA_OF_COMMON_FACILITIES", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("CONTRACT_NUMBER", DbType.String));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("CONTRACT_DATE", DbType.Date));
            Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("COST_BY_CONTRACT_IN_MONTH", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_SERVICE_PROVIDER", "CONTRACT_NUMBER");

            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "APPOINTMENT_COMMON_FACILITIES");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "AREA_OF_COMMON_FACILITIES");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "NUMBER_CONTRACT");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "CONTRACT_DATE");
            Database.RemoveColumn("DI_DISINFO_COM_FACILS", "COST_BY_CONTRACT_IN_MONTH");
        }
    }
}
