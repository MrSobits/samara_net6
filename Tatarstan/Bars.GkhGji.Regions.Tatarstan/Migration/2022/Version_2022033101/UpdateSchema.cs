namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022033101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;

    [Migration("2022033101")]
    [MigrationDependsOn(typeof(Version_2022033100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            var sql = $@"
                 WITH deleted AS 
                 (
                    DELETE FROM gji_basestat_appcit gba
                    WHERE EXISTS 
                    (
                        SELECT 
                        FROM   gji_warning_inspection gwi
                        WHERE  gba.inspection_id = gwi.id
                    )
                    RETURNING inspection_id, gji_appcit_id
                 )
                 UPDATE gji_warning_inspection
                 SET    appeal_cits_id = d.gji_appcit_id,
                        inspection_basis = {(int)InspectionCreationBasis.AppealCitizens}
                 FROM   
                 (
                     SELECT inspection_id, MAX(gji_appcit_id) AS gji_appcit_id
                     FROM deleted 
                     GROUP BY 1
                 ) d
                 WHERE d.inspection_id = id";

            this.Database.ExecuteNonQuery(sql);
        }
    }
}