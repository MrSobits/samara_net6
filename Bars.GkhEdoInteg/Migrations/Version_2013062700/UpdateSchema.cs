namespace Bars.GkhEdoInteg.Migrations.Version_2013062700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013062700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhEdoInteg.Migrations.Version_2013061700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("INTGEDO_APPCITS", new Column("ADDRESS_EDO", DbType.String, 2000));
            Database.ExecuteNonQuery(@"UPDATE INTGEDO_APPCITS EDO SET
ADDRESS_EDO = (SELECT GA.ADDRESS_EDO FROM GJI_APPEAL_CITIZENS GA where GA.ID = EDO.APPEAL_CITS_ID)");
        }

        public override void Down()
        {
        }
    }
}