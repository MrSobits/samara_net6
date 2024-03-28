namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022120800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022120800")]
    [MigrationDependsOn(typeof(Version_2022113000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
	    /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                CREATE OR REPLACE FUNCTION public.gjigetwarningdoccntrobj(bigint)
                RETURNS integer
                LANGUAGE 'plpgsql'
                COST 100
                VOLATILE PARALLEL UNSAFE
	            AS $BODY$
	            declare 
                result integer :=0;  
                begin
                    select count(warning_doc_ro.id) into result
                    from GJI_WARNING_DOC_ROBJECT warning_doc_ro 
                    where warning_doc_ro.warning_doc_id = $1; 
                    return result; 
                end;
	            $BODY$;
	
	            CREATE OR REPLACE FUNCTION public.gjigetwarningdocrobj(bigint)
                RETURNS text
                LANGUAGE 'plpgsql'
                COST 100
                VOLATILE PARALLEL UNSAFE
                AS $BODY$
                declare 
                   objectId text :=''; 
                   result text := '/';  
                   cursorCon CURSOR IS 
                   select warning_doc_ro.reality_object_id 
                   from GJI_WARNING_DOC_ROBJECT warning_doc_ro where warning_doc_ro.warning_doc_id = $1; 
                begin 
                   OPEN cursorCon;
                   loop
                   FETCH cursorCon INTO objectId;
                   EXIT WHEN not FOUND; 
                   result:=result || objectId ||'/'; 
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end;
                $BODY$;

                CREATE OR REPLACE VIEW public.view_gji_warning_doc
				AS
				SELECT 
				doc.id,
				doc.state_id,
				doc.document_num,
				doc.document_number,
				doc.document_date,
				insp.id 												AS inspection_id,
				insp.type_base,
				disp.base_warning,
				CASE 
					WHEN gta.id NOTNULL 
					THEN gtac.name
					ELSE c.name
				END 													AS contragent_name,
				wi.inspection_basis,
				CASE 
					WHEN gta.id NOTNULL 
					THEN CASE gta.type_object
						 	WHEN 1 THEN 'Физическое лицо'::text
							WHEN 2 THEN 'Юридическое лицо'::text
							WHEN 3 THEN 'Должностное лицо'::text
							WHEN 4 THEN 'Индивидуальный предприниматель'::text
							ELSE ''::text
						 END
					ELSE CASE insp.person_inspection
				        	WHEN 10 THEN 'Физическое лицо'::text
				       		WHEN 20 THEN 'Организация'::text
				      		WHEN 30 THEN 'Должностное лицо'::text
				       		WHEN 40 THEN 'Жилой дом'::text
				       		ELSE ''::text
				   	 END 	
				END 												    AS person_inspection,
				CASE 
					WHEN gta.id NOTNULL 
					THEN gta.person_name::character varying(300)
					ELSE insp.physical_person
				END 												    AS physical_person,
				gjigetinsprobjectmuid(doc.inspection_id) 				AS mu_id,
				gjigetwarningdoccntrobj(doc.id::bigint) 				AS ro_count,
				gjigetwarningdocrobj(doc.id::bigint) AS ro_ids,
				inspector.fio::text AS inspectors,
				CASE 
					WHEN gta.id NOTNULL 
					THEN gtamu.name
					ELSE gjigetinsprobjectmuname(doc.inspection_id) 
				END 													AS municipality,
				doc.type_document 										AS type_doc
				FROM gji_document doc
				JOIN gji_warning_doc disp ON disp.id = doc.id
				LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
				LEFT JOIN gji_warning_inspection wi ON wi.id = insp.id
				LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
				LEFT JOIN b4_state state ON doc.state_id = state.id
				LEFT JOIN gkh_dict_inspector inspector ON inspector.id = disp.executant_id
				LEFT JOIN 
				(
			  	    gji_task_actionisolated gta 
			  	    JOIN gji_document gtadoc ON gtadoc.id = gta.id
			  	    JOIN gkh_dict_municipality gtamu ON gta.municipality_id = gtamu.id
			  	    JOIN gkh_contragent gtac ON gtac.id = gta.contragent_id
				) ON gtadoc.inspection_id = doc.inspection_id");
        }
    }
}