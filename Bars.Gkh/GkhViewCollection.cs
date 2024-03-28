namespace Bars.Gkh
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;

    public class GkhViewCollection : BaseGkhViewCollection
    {
        public override int Number 
        {
            get { return 0; }
        }

        #region Функции
        #region Create

        /// <summary>
        /// функция возвращает наименования актуальных управляющих организаций жилого дома
        /// gkhGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRobjectManorg(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gkhGetRobjectManorg (ro_id number) RETURN varchar2 is
                    manorg_name varchar2(4000):= '';
                    result varchar2(4000):= '';
                    zp  varchar2(4000):= ', ';
                    cur_date date:= current_date;
                    CURSOR cursorCon IS
                        select c.name
                        from gkh_morg_contract_realobj mcro
                            join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id
                            LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
                            LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
                        where mcro.reality_obj_id = ro_id
                            and moc.start_date <= cur_date
                            and (moc.end_date is null or moc.end_date >= cur_date);
                     begin 

                     OPEN cursorCon;
                     loop FETCH cursorCon INTO manorg_name;
                    EXIT WHEN cursorCon%NOTFOUND;
                    if(result is not null)
                    then
                    result:=result || zp;
                    end if;
                    result:=result || manorg_name;
                    end loop;
                    CLOSE cursorCon;
                    return result;
end;";
            }

            return @"
            CREATE OR REPLACE FUNCTION gkhGetRobjectManorg(bigint)
              RETURNS text AS
            $BODY$ declare
             manorg_name text := '';
             result text :='';
             zp text:=', ';
             cur_date date := (select CURRENT_DATE);
             cursorCon CURSOR IS
	            select c.name
	            from gkh_morg_contract_realobj mcro
		            join gkh_morg_contract moc on moc.id = mcro.man_org_contract_id
		            LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
		            LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
	            where mcro.reality_obj_id = $1
		            and moc.start_date <= cur_date
		            and (moc.end_date is null or moc.end_date >= cur_date);
             begin OPEN cursorCon;
            loop
            FETCH cursorCon INTO manorg_name;
            EXIT WHEN not FOUND;
            if(result!='')
            then
            result:=result || zp;
            end if;
            result:=result || manorg_name;
            end loop;
            CLOSE cursorCon;
            return result;
            end; $BODY$
              LANGUAGE plpgsql VOLATILE
              COST 100;";
        }

        /// <summary>
        /// Функция воозвращает наименования актуальных типов договоров управляющей организации жилого дома
        /// gkhGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetRobjectTypeContract(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return @"CREATE OR REPLACE FUNCTION gkhGetRobjectTypeContract(ro_id number) RETURN varchar2 is
                    result varchar2(4000) := '';
                    type_contract varchar2(4000) :='';
                    zp varchar2(4000):=', ';
                    cur_date date := current_date;
                    CURSOR cursorCon IS 
                    select 
                        CASE
                            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 20) THEN 'Договор между УК и ТСЖ'
                            WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 40) THEN 'Договор между УК и ЖСК'
                            WHEN moc.type_contract = 20 THEN 'Договор между УК и собственниками'
                            WHEN moc.type_contract = 30 AND mo.type_management = 20 THEN 'ТСЖ'
                            WHEN moc.type_contract = 30 AND mo.type_management = 40 THEN 'ЖСК'
                            ELSE 'Непосредственное управление'
                        END AS type_contract
                    from gkh_morg_contract_realobj mcro
                        JOIN gkh_morg_contract moc ON moc.id = mcro.man_org_contract_id
                        LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
                        LEFT JOIN gkh_morg_contract_jsktsj mocjsktsj ON mocjsktsj.id = moc.id
                        LEFT JOIN gkh_managing_organization mo2 ON mo2.id = mocjsktsj.man_org_jsk_tsj_id
                    where mcro.reality_obj_id = ro_id
                        and moc.start_date <= cur_date 
                        and (moc.end_date is null or moc.end_date >= cur_date);
                 begin
                OPEN cursorCon;
                loop 
                    FETCH cursorCon INTO type_contract;
                    EXIT WHEN cursorCon%NOTFOUND;
                    if(result is not null)
                    then
                        result:=result || zp;
                    end if;
                    result:=result || type_contract;
                end loop;
                CLOSE cursorCon;
                return result;
end;";
            }

            return @"
                CREATE OR REPLACE FUNCTION gkhGetRobjectTypeContract(bigint)
                RETURNS text AS
                $BODY$ 
                declare
                    result text := '';
                    type_contract text :='';
                    zp text:=', ';
                    cur_date date := (select CURRENT_DATE);
                    cursorCon CURSOR IS 
	                select 
		                CASE
		                    WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 20) THEN 'Договор между УК и ТСЖ'::text
		                    WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 40) THEN 'Договор между УК и ЖСК'::text
		                    WHEN moc.type_contract = 20 THEN 'Договор между УК и собственниками'::text
		                    WHEN moc.type_contract = 30 AND mo.type_management = 20 THEN 'ТСЖ'::text
		                    WHEN moc.type_contract = 30 AND mo.type_management = 40 THEN 'ЖСК'::text
		                    ELSE 'Непосредственное управление'::text
		                END AS type_contract
	                from gkh_morg_contract_realobj mcro
		                JOIN gkh_morg_contract moc ON moc.id = mcro.man_org_contract_id
		                LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
		                LEFT JOIN gkh_morg_contract_jsktsj mocjsktsj ON mocjsktsj.id = moc.id
		                LEFT JOIN gkh_managing_organization mo2 ON mo2.id = mocjsktsj.man_org_jsk_tsj_id
	                where mcro.reality_obj_id = $1 
		                and moc.start_date <= cur_date 
		                and (moc.end_date is null or moc.end_date >= cur_date);
                begin
                OPEN cursorCon;
                loop 
                    FETCH cursorCon INTO type_contract;
                    EXIT WHEN not FOUND;
                    if(result <> '')
                    then
	                    result:=result || zp;
                    end if;
                    result:=result || type_contract;
                end loop;
                CLOSE cursorCon;
                return result;
                end; 
                $BODY$
                LANGUAGE plpgsql;";
        }

        private string CreateFunctionRebuildSequences(DbmsKind dbmsKind)
        {
            if (dbmsKind != DbmsKind.PostgreSql)
            {
                return string.Empty;
            }

            return @"CREATE OR REPLACE FUNCTION rebuilt_sequences()
  RETURNS integer AS
$BODY$
  DECLARE sequencedefs RECORD;
            c integer;
            BEGIN
                FOR sequencedefs IN Select
      constraint_column_usage.table_name as tablename,
      constraint_column_usage.column_name as columnname,
      replace(replace(columns.column_default, '''::regclass)', ''), 'nextval(''', '') as sequencename
      from information_schema.constraint_column_usage, information_schema.columns
      where constraint_column_usage.table_schema = 'public' AND
      columns.table_schema = 'public' AND columns.table_name = constraint_column_usage.table_name
      AND constraint_column_usage.column_name = columns.column_name
      AND columns.column_default is not null
   LOOP
      EXECUTE 'select max(' || sequencedefs.columnname || ') from ' || sequencedefs.tablename INTO c;
            IF c is null THEN c = 0;
            END IF;
            IF c is not null THEN c = c + 1;
            END IF;
            --RAISE NOTICE '%', sequencedefs.tablename || ':' || 'alter sequence ' || sequencedefs.sequencename || ' restart  with ' || c;
            if sequencedefs.sequencename != '0' then EXECUTE 'alter sequence ' || sequencedefs.sequencename || ' restart  with ' || c;
            end if;
            END LOOP;
            RETURN 1;
            END;
$BODY$
  LANGUAGE plpgsql";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// Удаление
        /// gkhGetRobjectManorg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRobjectManorg(DbmsKind dbmsKind)
        {
			var funcName = "gkhGetRobjectManorg";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists gkhGetRobjectManorg(integer)";
        }

        private string DeleteFunctionRebuildSequences(DbmsKind dbmsKind)
        {
            var funcName = "rebuilt_sequences";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists gkhGetRobjectManorg(integer)";
        }

        /// <summary>
        /// Удаление
        /// gkhGetRobjectTypeContract
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetRobjectTypeContract(DbmsKind dbmsKind)
        {
			var funcName = "gkhGetRobjectTypeContract";
            if (dbmsKind == DbmsKind.Oracle)
            {
				return DropFunctionOracleQuery(funcName);
            }

            return @"drop function if exists gkhGetRobjectTypeContract(integer)";
        }

        #endregion Delete
        #endregion
        #region Вьюхи
        #region Create

        /// <summary>
        /// Вьюха КЭ
        /// view_struct_el_ro
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewStructElementRealObj(DbmsKind dbmsKind)
        {
            return @"CREATE OR REPLACE VIEW public.view_struct_el_ro AS
                    SELECT rel.ro_id,
                         obj.name AS ooi,
                         el.name AS name_se,
                         rel.last_overhaul_year AS last_year,
                         rel.wearout,
                         rel.volume,
                         meas.name AS unit_measure
                         FROM ovrhl_ro_struct_el rel
                         JOIN ovrhl_struct_el el ON el.id = rel.struct_el_id
                         JOIN ovrhl_struct_el_group elg ON elg.id = el.group_id
                         JOIN ovrhl_common_estate_object obj ON obj.id = elg.cmn_estate_obj_id
                         JOIN gkh_dict_unitmeasure meas ON meas.id = el.unit_measure_id;";
        }

        /// <summary>
        /// Вьюха жилых домов
        /// view_gkh_reality_object
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRealityObject(DbmsKind dbmsKind)
        {
            return @"CREATE OR REPLACE VIEW VIEW_GKH_REALITY_OBJECT AS
   SELECT ro.id,
          ro.external_id,
          ro.id AS reality_object_id,
          ro.address,
          ro.type_house,
          ro.condition_house,
          ro.date_demolition,
          ro.floors,
          ro.number_entrances,
          ro.number_living,
          ro.number_apartments,
          ro.area_mkd,
          ro.area_living,
          ro.physical_wear,
          ro.number_lifts,
          ro.heating_system,
          ro.type_roof,
          ro.date_last_renovation,
          ro.date_commissioning,
          ro.code_erc,
          ro.is_insured_object,
          ro.roofing_material_id,
          ro.wall_material_id,
          ro.gkh_code,
          rm.name AS roofing_material_name,
          wm.name AS wall_material_name,
          mu.id AS municipality_id,
          mu2.name as stl_name,
          ro.stl_municipality_id as settlement_id,
          mu.name AS municipality_name,
          gkhGetRobjectTypeContract (ro.id) AS type_contract_name,
          gkhGetRobjectManorg (ro.id) AS manorg_name,
          ro.is_build_soc_mortgage,
          fa.address_name AS full_address,
          state.id as state_id,
          ro.is_repair_inadvisable,
          ro.is_not_involved_cr,
          ro.district
     FROM gkh_reality_object ro
          LEFT JOIN b4_fias_address fa ON fa.id = ro.fias_address_id
          LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
            LEFT JOIN gkh_dict_municipality mu2 ON mu2.id = ro.stl_municipality_id
          LEFT JOIN gkh_dict_roofing_material rm
             ON rm.id = ro.roofing_material_id
          LEFT JOIN gkh_dict_wall_material wm ON wm.id = ro.wall_material_id
          LEFT JOIN b4_state state ON ro.state_id = state.id
     ORDER BY mu.name, ro.address";
        }

        /// <summary>
        /// Вьюха договоров жилого дома и управляющей организации
        /// view_real_obj_manorg_contr
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRealityObjectManorgContract(DbmsKind dbmsKind)
        {
            return @"
            CREATE OR REPLACE VIEW view_real_obj_manorg_contr AS 
            SELECT mocr.reality_obj_id AS ro_id, 
                    CASE
                        WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 20) THEN 'Договор между УК и ТСЖ'
                        WHEN moc.type_contract = 10 AND (mo2.id IS NULL OR mo2.type_management = 40) THEN 'Договор между УК и ЖСК'
                        WHEN moc.type_contract = 20 THEN 'Договор между УК и собственниками'
                        WHEN moc.type_contract = 30 AND mo.type_management = 20 THEN 'ТСЖ'
                        WHEN moc.type_contract = 30 AND mo.type_management = 40 THEN 'ЖСК'
                        ELSE 'Непосредственное управление'
                    END AS type_contract, 
                mo.id AS mo_id, 
                c.name AS manorg_name, 
                moc.start_date, 
                moc.end_date
            FROM gkh_morg_contract_realobj mocr
               JOIN gkh_morg_contract moc ON moc.id = mocr.man_org_contract_id
               LEFT JOIN gkh_managing_organization mo ON mo.id = moc.manag_org_id
               LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
               LEFT JOIN gkh_morg_contract_jsktsj mocjsktsj ON mocjsktsj.id = moc.id
               LEFT JOIN gkh_managing_organization mo2 ON mo2.id = mocjsktsj.man_org_jsk_tsj_id";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// Удаление view_gkh_reality_object
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRealityObject(DbmsKind dbmsKind)
        {
            var viewName = "view_gkh_reality_object";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return base.DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        private string DeleteViewRealityObjectManorgContract(DbmsKind dbmsKind)
        {
            var viewName = "view_real_obj_manorg_contr";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        #endregion Delete
        #endregion
    }
}