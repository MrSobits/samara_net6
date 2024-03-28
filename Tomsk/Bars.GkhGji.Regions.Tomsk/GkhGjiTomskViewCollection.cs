namespace Bars.GkhGji.Regions.Tomsk
{
	using Bars.B4.Modules.Ecm7.Framework;

	using Gkh;

	public class GkhGjiTomskViewCollection : BaseGkhViewCollection
    {
        public override int Number
        {
            get
            {
                return 1;
            }
        }

        #region Create
        #region Функции

        /// <summary>
        /// Функция возвращает имена источников обращения
        /// gjiGetRevenueSourceNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRevenueSourceNames(DbmsKind dbmsKind)
        {
            return @"
				CREATE OR REPLACE FUNCTION gjiGetRevenueSourceNames(bigint)
						  RETURNS text AS
						$BODY$ declare
						 src_names text := '';
						 result text :='';
						 zp text:=', ';
						 cursorCon CURSOR IS
							select distinct s.name
							from gji_appeal_sources asrc
							join gji_dict_revenuesource s ON s.id = asrc.revenue_source_id
							where asrc.appcit_id = $1 ;
						 begin OPEN cursorCon;
						loop
						FETCH cursorCon INTO src_names;
						EXIT WHEN not FOUND;
						if(result!='')
						then
						result:=result || zp;
						end if;
						result:=result || src_names;
						end loop;
						CLOSE cursorCon;
						return result;
						end; $BODY$
						  LANGUAGE plpgsql VOLATILE
						  COST 100;";
        }

        /// <summary>
        /// Функция возвращает имена назначенных инспекторов обращения граждан
        /// gjiGetAppealExecutantNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetAppealExecutantNames(DbmsKind dbmsKind)
        {
            return @"
				CREATE OR REPLACE FUNCTION gjiGetAppealExecutantNames(bigint)
						  RETURNS text AS
						$BODY$ declare
						 ex_names text := '';
						 result text :='';
						 zp text:=', ';
						 cursorCon CURSOR IS
							select distinct insp.fio
							from gji_appcit_executant ex
							join gkh_dict_inspector insp ON insp.id = ex.executant_id
							join b4_state st on st.id = ex.state_id and st.final_state != true
							where ex.appcit_id = $1 ;
						 begin OPEN cursorCon;
						loop
						FETCH cursorCon INTO ex_names;
						EXIT WHEN not FOUND;
						if(result!='')
						then
						result:=result || zp;
						end if;
						result:=result || ex_names;
						end loop;
						CLOSE cursorCon;
						return result;
						end; $BODY$
						  LANGUAGE plpgsql VOLATILE
						  COST 100;";
        }

        #endregion Функции 

        #region Вьюхи 

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_appeal_cits;
CREATE VIEW view_gji_appeal_cits AS 
SELECT 
    gac.id, 
    gac.DOCUMENT_NUMBER, 
    gac.gji_number,
    gac.GJI_NUM_SORT, 
    gac.date_from, 
    gac.check_time, 
    gac.questions_count, 
    countro.count_ro, 
    mun.municipality, 
    '' as mo_settlement,
    '' as place_name,
    c.name AS 
    contragent_name, 
    gac.state_id,
    gac.executant_id, 
    gac.tester_id, 
    gac.SURETY_RESOLVE_ID,
    gac.EXECUTE_DATE,
    gac.SURETY_DATE,
    gjiGetAppealRobject(gac.id) AS ro_ids,
    gac.ZONAINSP_ID,
    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
    gac.correspondent,
    gac.correspondent_address,
    mun.municipality_id,
    gjiGetRevenueSourceNames(gac.id) as source_names,
	gjiGetAppealExecutantNames(gac.id) AS executant_names,
    gjiGetRevenueSourceDates(gac.id::bigint) AS source_dates,
    gjiGetRevenueSourceNumbers(gac.id::bigint) AS source_numbers,
    '' as subjects
FROM gji_appeal_citizens gac
    LEFT JOIN ( 
        SELECT 
            count(garo.reality_object_id) AS count_ro, 
            gac1.id AS gac_id
        FROM gji_appeal_citizens gac1
            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            GROUP BY gac1.id
        ) countro ON countro.gac_id = gac.id
    LEFT JOIN ( 
        SELECT 
            gaac.id AS gac_id, 
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality,
            ( 
                SELECT 
                    gdm.id AS municipality_id
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality_id
        FROM gji_appeal_citizens gaac
        ) mun ON mun.gac_id = gac.id
    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id";
		}

        #endregion Вьюхи 
        #endregion Create

        #region Delete

        /// <summary>
        /// view_gji_appeal_cits
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewAppealCits(DbmsKind dbmsKind)
        {
            return this.DropViewPostgreQuery("view_gji_appeal_cits");
        }
   
        /// <summary>
        /// gjiGetAppealExecutantNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetAppealExecutantNames(DbmsKind dbmsKind)
		{
			return @"DROP FUNCTION if exists gjiGetAppealExecutantNames(bigint)";
		}

        /// <summary>
        /// gjiGetRevenueSourceNames(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRevenueSourceNames(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetRevenueSourceNames(bigint)";
        }

        #endregion Delete
      
    }
}