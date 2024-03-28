namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022012800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022012800")]
    [MigrationDependsOn(typeof(Version_2022012400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                DROP VIEW public.view_warning_inspection;                
                CREATE OR REPLACE VIEW public.view_warning_inspection
                AS
                WITH municipality AS (
                	SELECT s.id, STRING_AGG(s.name::text, '; '::text) AS municipality
                	FROM (SELECT gji_ro.inspection_id AS id, mu_1.name
                	      FROM gji_inspection_robject gji_ro
                		           JOIN gkh_reality_object ro ON ro.id = gji_ro.reality_object_id
                		           LEFT JOIN gkh_dict_municipality mu_1 ON mu_1.id = ro.municipality_id
                	      GROUP BY gji_ro.inspection_id, mu_1.id) s
                	GROUP BY s.id
                ),
                     inspectors AS (
                	     SELECT s.id, STRING_AGG(s.fio::text, '; '::text) AS fio
                	     FROM (SELECT gji_in.inspection_id AS id, ins.fio
                	           FROM gji_inspection_inspector gji_in
                		                JOIN gkh_dict_inspector ins ON ins.id = gji_in.inspector_id
                	           GROUP BY gji_in.inspection_id, ins.fio) s
                	     GROUP BY s.id
                     )
                SELECT w.id,
                       state.id AS state_id,
                       mu.municipality,
                       c.name AS contragent_name,
                       i.person_inspection,
                       CASE
                	       WHEN i.person_inspection = 10 THEN NULL::integer
                	       ELSE i.type_jur_person
                	       END AS type_jur_person,
                       i.check_date,
                       w.date AS inspection_date,
                       w.document_number,
                       i.inspection_number,
                       i.registr_number,
                       insp.fio AS inspectors,
                       (SELECT COUNT(insp_ro.reality_object_id) AS count
                        FROM gji_inspection_robject insp_ro
                        WHERE insp_ro.inspection_id = i.id) AS ro_count,
                       (CASE WHEN gji_number NOTNULL THEN '№ ' || gji_number ELSE '' END
	                    || CASE WHEN date_from NOTNULL THEN ' от ' || to_char(date_from, 'DD.MM.YYYY') || ' г.' ELSE '' END) AS appealcits_number_date
                FROM gji_inspection i
                	     JOIN gji_warning_inspection w ON w.id = i.id
                	     LEFT JOIN gji_appeal_citizens gac ON w.appeal_cits_id = gac.id
                	     LEFT JOIN gkh_contragent c ON i.contragent_id = c.id
                	     LEFT JOIN b4_state state ON i.state_id = state.id
                	     LEFT JOIN municipality mu ON mu.id = i.id
                	     LEFT JOIN inspectors insp ON insp.id = i.id;
                
                ALTER TABLE public.view_warning_inspection
                	OWNER TO bars;");
        }
    }
}
