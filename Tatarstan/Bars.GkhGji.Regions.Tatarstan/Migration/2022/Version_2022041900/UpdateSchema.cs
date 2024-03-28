namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041900
{

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022041900")]
    [MigrationDependsOn(typeof(Version_2022041600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var sql = $@"
                INSERT INTO gji_dict_tat_risk_category (id)
                SELECT id
                FROM gkh_dict_risk_category";
            this.Database.ExecuteNonQuery(sql);
        }
    }
}
