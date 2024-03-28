namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022011900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022011900")]
    [MigrationDependsOn(typeof(Version_2022011800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                -- Сохраняем существующие значения
                CREATE TEMP TABLE tmp1 AS
                SELECT id,	basis_id
                FROM gji_warning_inspection
                WHERE basis_id NOTNULL;

                -- Удаляем представления, где есть заменяемый столбец
                DROP VIEW view_warning_inspection;
                DROP VIEW view_gji_motivation_conclusion;
                DROP VIEW view_gji_warning_doc;
                
                -- Заменяем столбец
                ALTER TABLE gji_warning_inspection DROP COLUMN basis_id;
                ALTER TABLE gji_warning_inspection ADD COLUMN inspection_basis smallint;
                
                -- Заполняем новый столбец сохраненными значениями
                UPDATE gji_warning_inspection a
                SET inspection_basis = t.basis_id
                FROM tmp1 t
                WHERE t.id = a.id;

                -- Пересоздаем представления с учетом изменений
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
                       (gac.document_number || ' ' || to_char(gac.date_from , 'DD.MM.YYYY')) AS appealcits_number_date
                FROM gji_inspection i
                	     JOIN gji_warning_inspection w ON w.id = i.id
                	     LEFT JOIN gji_appeal_citizens gac ON w.appeal_cits_id = gac.id
                	     LEFT JOIN gkh_contragent c ON i.contragent_id = c.id
                	     LEFT JOIN b4_state state ON i.state_id = state.id
                	     LEFT JOIN municipality mu ON mu.id = i.id
                	     LEFT JOIN inspectors insp ON insp.id = i.id;
                
                ALTER TABLE public.view_warning_inspection
                	OWNER TO bars;
                
                CREATE OR REPLACE VIEW public.view_gji_warning_doc
                AS
                SELECT doc.id,
                   doc.state_id,
                   doc.document_num,
                   doc.document_number,
                   doc.document_date,
                   insp.id AS inspection_id,
                   insp.type_base,
                   disp.base_warning,
                   c.name AS contragent_name,
                   wi.inspection_basis AS inspection_basis,
                       CASE insp.person_inspection
                           WHEN 10 THEN 'Физическое лицо'::text
                           WHEN 20 THEN 'Организация'::text
                           WHEN 30 THEN 'Должностное лицо'::text
                           WHEN 40 THEN 'Жилой дом'::text
                           ELSE ''::text
                       END AS person_inspection,
                   insp.physical_person,
                   gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
                   gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
                   gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
                   gjigetdocumentinspectors(doc.id::bigint) AS inspectors,
                   gjigetinsprobjectmuname(doc.inspection_id) AS municipality,
                   doc.type_document AS type_doc
                  FROM gji_document doc
                    JOIN gji_warning_doc disp ON disp.id = doc.id
                    LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
                    LEFT JOIN gji_warning_inspection wi ON wi.id = insp.id
                    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN b4_state state ON doc.state_id = state.id;

                ALTER TABLE public.view_gji_warning_doc
                    OWNER TO bars;

                CREATE OR REPLACE VIEW public.view_gji_motivation_conclusion
                 AS
                 SELECT doc.id,
                    doc.state_id,
                    doc.document_num,
                    doc.document_number,
                    doc.document_date,
                    insp.id AS inspection_id,
                    insp.type_base,
                    c.name AS contragent_name,
                    wi.inspection_basis AS inspection_basis,
                        CASE insp.person_inspection
                            WHEN 10 THEN 'Физическое лицо'::text
                            WHEN 20 THEN 'Организация'::text
                            WHEN 30 THEN 'Должностное лицо'::text
                            WHEN 40 THEN 'Жилой дом'::text
                            ELSE ''::text
                        END AS person_inspection,
                    insp.physical_person,
                    gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
                    gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
                    gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
                    gjigetdocumentinspectors(doc.id::bigint) AS inspectors,
                    gjigetinsprobjectmuname(doc.inspection_id) AS municipality,
                    doc.type_document AS type_doc
                   FROM gji_document doc
                     JOIN gji_motivation_conclusion m ON m.id = doc.id
                     LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
                     LEFT JOIN gji_warning_inspection wi ON wi.id = insp.id
                     LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                     LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                     LEFT JOIN b4_state state ON doc.state_id = state.id;
                
                ALTER TABLE public.view_gji_motivation_conclusion
                    OWNER TO bars;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery(@"
                -- Сохраняем существующие значения
                CREATE TEMP TABLE tmp1 AS
                SELECT id,	inspection_basis
                FROM gji_warning_inspection
                WHERE inspection_basis NOTNULL;

                -- Удаляем представления, где есть заменяемый столбец
                DROP VIEW view_warning_inspection;
                DROP VIEW view_gji_motivation_conclusion;
                DROP VIEW view_gji_warning_doc;
                
                -- Заменяем столбец
                ALTER TABLE gji_warning_inspection DROP COLUMN inspection_basis;
                ALTER TABLE gji_warning_inspection ADD COLUMN basis_id bigint;
                ALTER TABLE gji_warning_inspection
                    ADD CONSTRAINT fk_gji_warning_inspection_basis FOREIGN KEY (basis_id) REFERENCES gji_inspection_basis (id);
                
                -- Заполняем новый столбец сохраненными значениями
                UPDATE gji_warning_inspection a
                SET basis_id = t.inspection_basis
                FROM tmp1 t
                WHERE t.id = a.id;
                
                -- Возвращаем старые представления
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
                        WHERE insp_ro.inspection_id = i.id) AS ro_count
                FROM gji_inspection i
                	     JOIN gji_warning_inspection w ON w.id = i.id
                	     LEFT JOIN gkh_contragent c ON i.contragent_id = c.id
                	     LEFT JOIN b4_state state ON i.state_id = state.id
                	     LEFT JOIN municipality mu ON mu.id = i.id
                	     LEFT JOIN inspectors insp ON insp.id = i.id;
                
                ALTER TABLE public.view_warning_inspection
                	OWNER TO bars;
                
                CREATE OR REPLACE VIEW public.view_gji_motivation_conclusion
                AS
                SELECT doc.id,
                   doc.state_id,
                   doc.document_num,
                   doc.document_number,
                   doc.document_date,
                   insp.id AS inspection_id,
                   insp.type_base,
                   c.name AS contragent_name,
                   ib.name AS inspection_basis,
                       CASE insp.person_inspection
                           WHEN 10 THEN 'Физическое лицо'::text
                           WHEN 20 THEN 'Организация'::text
                           WHEN 30 THEN 'Должностное лицо'::text
                           WHEN 40 THEN 'Жилой дом'::text
                           ELSE ''::text
                       END AS person_inspection,
                   insp.physical_person,
                   gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
                   gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
                   gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
                   gjigetdocumentinspectors(doc.id::bigint) AS inspectors,
                   gjigetinsprobjectmuname(doc.inspection_id) AS municipality,
                   doc.type_document AS type_doc
                  FROM gji_document doc
                    JOIN gji_motivation_conclusion m ON m.id = doc.id
                    LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
                    LEFT JOIN gji_warning_inspection wi ON wi.id = insp.id
                    LEFT JOIN gji_inspection_basis ib ON ib.id = wi.basis_id
                    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN b4_state state ON doc.state_id = state.id;

                ALTER TABLE public.view_gji_motivation_conclusion
                    OWNER TO bars;

                CREATE OR REPLACE VIEW public.view_gji_warning_doc
                AS
                SELECT doc.id,
                   doc.state_id,
                   doc.document_num,
                   doc.document_number,
                   doc.document_date,
                   insp.id AS inspection_id,
                   insp.type_base,
                   disp.base_warning,
                   c.name AS contragent_name,
                   ib.name AS inspection_basis,
                       CASE insp.person_inspection
                           WHEN 10 THEN 'Физическое лицо'::text
                           WHEN 20 THEN 'Организация'::text
                           WHEN 30 THEN 'Должностное лицо'::text
                           WHEN 40 THEN 'Жилой дом'::text
                           ELSE ''::text
                       END AS person_inspection,
                   insp.physical_person,
                   gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
                   gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
                   gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
                   gjigetdocumentinspectors(doc.id::bigint) AS inspectors,
                   gjigetinsprobjectmuname(doc.inspection_id) AS municipality,
                   doc.type_document AS type_doc
                  FROM gji_document doc
                    JOIN gji_warning_doc disp ON disp.id = doc.id
                    LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
                    LEFT JOIN gji_warning_inspection wi ON wi.id = insp.id
                    LEFT JOIN gji_inspection_basis ib ON ib.id = wi.basis_id
                    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN b4_state state ON doc.state_id = state.id;

                ALTER TABLE public.view_gji_warning_doc
                    OWNER TO bars;");
        }
    }
}
