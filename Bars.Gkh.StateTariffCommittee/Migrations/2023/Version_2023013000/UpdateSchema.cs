namespace Bars.Gkh.StateTariffCommittee.Migrations._2023.Version_2023013000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.SqlExecutor;

    [Migration("2023013000")]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
	            #region GKHRIS-6564 GKHRIS-6565
	            sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_ls_account_info_table_name(oktmo_mo varchar, date_s date)
 RETURNS text
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
	report_month_part text;
	table_name text;
	any_recs boolean;
BEGIN
	IF NOT EXISTS (SELECT FROM gkh_dict_municipality m WHERE m.oktmo = oktmo_mo) THEN 
		RAISE EXCEPTION USING MESSAGE = 'Код ОКТМО ' || oktmo_mo || ' не найден';
	END IF;

	DROP TABLE IF EXISTS houses;
	CREATE TEMP TABLE houses AS 
		SELECT 
			gh.house_id,
			fh.fias_house_guid::varchar AS house_guid
		FROM data.gf_house gh
		JOIN public.fias_house fh ON gh.fias_address_id = fh.id AND fh.oktmo IN (
			SELECT m.oktmo FROM gkh_dict_municipality m
			WHERE m.oktmo = oktmo_mo
			UNION
			SELECT m3.oktmo FROM gkh_dict_municipality m2
			LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
			WHERE m2.oktmo = oktmo_mo
			UNION
			SELECT m4.oktmo FROM gkh_dict_municipality m2
			LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
			LEFT JOIN gkh_dict_municipality m4 ON m4.parent_mo_id = m3.id
			WHERE m2.oktmo = oktmo_mo)
		WHERE NOT gh.is_del AND gh.ogf_annul_reason_id ISNULL;

	CREATE INDEX ON houses (house_id);
	ANALYZE houses;

	report_month_part := 'charge_' || to_char(date_s, 'yyyy_mm') || '_%';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS charge_table_name,
			NULL::TEXT AS ls_account_table_name,
			NULL::TEXT AS ls_address_table_name,
			split_part(pt.tablename, '_', 4) AS owner_contragent_id
		FROM pg_catalog.pg_tables pt
		WHERE pt.schemaname = 'data_part' AND pt.tablename like report_month_part
	);

	UPDATE partitions p SET ls_account_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'ls_account_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	DELETE FROM partitions WHERE ls_account_table_name ISNULL;

	UPDATE partitions p SET ls_address_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'ls_address_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	DROP TABLE IF EXISTS ls_account_houses;
	CREATE TEMP TABLE ls_account_houses (
		ls_account_id bigint,
		house_id bigint,
		owner_contragent_id bigint
	);

	DROP TABLE IF EXISTS ls_account_premises;
	CREATE TEMP TABLE ls_account_premises (
		ls_account_id bigint,
		premise_id bigint,
		owner_contragent_id bigint
	);

	DROP TABLE IF EXISTS ls_account_rooms;
	CREATE TEMP TABLE ls_account_rooms (
		ls_account_id bigint,
		room_id bigint,
		owner_contragent_id bigint
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			DROP TABLE IF EXISTS ls_houses;
			CREATE TEMP TABLE ls_houses AS (
				SELECT DISTINCT
					c.ls_account_id,
					la.house_id,
					la.owner_contragent_id
				FROM data_part.' || rec.charge_table_name || ' c
				JOIN data_part.' || rec.ls_account_table_name || ' la ON c.ls_account_id = la.ls_account_id AND NOT la.is_del AND la.closed_on ISNULL
				WHERE NOT c.is_del AND c.tarif NOTNULL AND c.tarif <> 0
			);
		';

		IF EXISTS (SELECT FROM ls_houses) THEN
			IF rec.ls_address_table_name NOTNULL THEN
				CREATE INDEX index_ls_houses_ls_account_id ON ls_houses (ls_account_id);
				CREATE INDEX index_ls_houses_house_id ON ls_houses (house_id);
				ANALYZE ls_houses;

				EXECUTE '
					DROP TABLE IF EXISTS premises_rooms;
					CREATE TEMP TABLE premises_rooms AS (
						SELECT DISTINCT
							lh.ls_account_id,
							lh.house_id,
							la.premise_id,
							la.room_id,
							lh.owner_contragent_id
						FROM ls_houses lh
						JOIN data_part.' || rec.ls_address_table_name || ' la ON la.ls_account_id = lh.ls_account_id AND NOT la.is_del AND (la.premise_id NOTNULL OR la.room_id NOTNULL));
				';

				CREATE INDEX index_premises_rooms ON premises_rooms (house_id);
				ANALYZE premises_rooms;

				INSERT INTO ls_account_rooms
				SELECT
					pr.ls_account_id,
					pr.room_id,
					pr.owner_contragent_id
				FROM premises_rooms pr
				WHERE pr.room_id NOTNULL;

				INSERT INTO ls_account_premises
				SELECT
					pr.ls_account_id,
					pr.premise_id,
					pr.owner_contragent_id
				FROM premises_rooms pr
				WHERE pr.room_id ISNULL;

				INSERT INTO ls_account_houses
				SELECT
					lh.ls_account_id,
					lh.house_id,
					lh.owner_contragent_id
				FROM ls_houses lh
				WHERE lh.house_id NOTNULL AND NOT EXISTS (SELECT FROM premises_rooms pr WHERE pr.house_id = lh.house_id);
			ELSE
				INSERT INTO ls_account_houses
				SELECT
					lh.ls_account_id,
					lh.house_id,
					lh.owner_contragent_id
				FROM ls_houses lh
				WHERE lh.house_id NOTNULL;
			END IF;
		END IF;
	END LOOP;

	CREATE INDEX index_ls_account_houses_ls_account_id ON ls_account_houses (ls_account_id);
	CREATE INDEX index_ls_account_houses_house_id ON ls_account_houses (house_id);
	ANALYZE ls_account_houses;

	CREATE INDEX index_ls_account_premises_premise_id ON ls_account_premises (premise_id);
	ANALYZE ls_account_premises;

	CREATE INDEX index_ls_account_rooms_room_id ON ls_account_rooms (room_id);
	ANALYZE ls_account_rooms;

	SELECT 'tariff_ls_account_info_' || (EXTRACT(EPOCH FROM now())*1000)::bigint INTO table_name;

	EXECUTE '
		DROP TABLE IF EXISTS ' || table_name || ';
		CREATE TEMP TABLE ' || table_name || ' (
			ls_account_id bigint,
			house_id bigint,
			house_guid varchar,
			premise_id bigint,
			room_id bigint,
			owner_contragent_id bigint
		);

		INSERT INTO ' || table_name || '
		SELECT
			lar.ls_account_id,
			gh.house_id,
			gh.house_guid,
			gp.premise_id,
			gr.room_id,
			lar.owner_contragent_id
		FROM ls_account_rooms lar
		JOIN data.gf_room gr ON lar.room_id = gr.room_id AND NOT gr.is_del AND gr.ogf_annul_reason_id ISNULL
		LEFT JOIN data.gf_premise gp ON gr.premise_id = gp.premise_id AND NOT gp.is_del AND gp.ogf_annul_reason_id ISNULL
		JOIN houses gh ON gh.house_id = COALESCE(gp.house_id, gr.house_id)
		WHERE gr.premise_id ISNULL OR gp.premise_id NOTNULL;

		INSERT INTO ' || table_name || '
		SELECT
			lap.ls_account_id,
			gh.house_id,
			gh.house_guid,
			gp.premise_id,
			NULL::bigint,
			lap.owner_contragent_id
		FROM ls_account_premises lap
		JOIN data.gf_premise gp ON gp.premise_id = lap.premise_id AND NOT gp.is_del AND gp.ogf_annul_reason_id ISNULL
		JOIN houses gh ON gh.house_id = gp.house_id;

		CREATE INDEX index_' || table_name || ' ON ' || table_name || ' (ls_account_id, house_id);
		ANALYZE ' || table_name || ';

		DELETE FROM ls_account_houses lah WHERE EXISTS (SELECT FROM ' || table_name || ' fi WHERE fi.house_id = lah.house_id AND fi.ls_account_id = lah.ls_account_id);

		INSERT INTO ' || table_name || '
		SELECT
			lah.ls_account_id,
			gh.house_id,
			gh.house_guid,
			NULL::bigint,
			NULL::bigint,
			lah.owner_contragent_id
		FROM ls_account_houses lah
		JOIN houses gh ON gh.house_id = lah.house_id;
	';

	EXECUTE '
		SELECT NOT EXISTS (SELECT FROM ' || table_name || ');
	' INTO any_recs;
 
	IF any_recs THEN
  		RAISE EXCEPTION 'Не найдено данных удовлетворяющих заданным критериям поиска';
 	END IF;
 
 	EXECUTE '
		CREATE INDEX ix_' || table_name || '_ls_account_id ON ' || table_name || ' (ls_account_id);
		CREATE INDEX ix_' || table_name || '_house_id ON ' || table_name || ' (house_id);
		CREATE INDEX ix_' || table_name || '_premise_id ON ' || table_name || ' (premise_id);
		CREATE INDEX ix_' || table_name || '_room_id ON ' || table_name || ' (room_id);
		CREATE INDEX ix_' || table_name || '_owner_contragent_id ON ' || table_name || ' (owner_contragent_id);
		ANALYZE ' || table_name || ';
	';

	RETURN table_name;
END;
$function$;");
	            #endregion

                #region GKHRIS-6467 GKHRIS-6565
                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_consumer_full_address(table_name varchar)
 RETURNS TABLE(houseguid varchar, municipality varchar, locality varchar, ulica varchar, housenum varchar, strucnum varchar, buildnum varchar, housetypecode varchar, housetypename varchar, floors integer, premiseid bigint, premisenum varchar, premisetypecode varchar, premisetypename varchar)
 LANGUAGE plpgsql
AS $function$
DECLARE
	rec record;
	report_month_part text;
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS houses_premises;
		CREATE TEMP TABLE houses_premises AS (
			SELECT DISTINCT
				house_id,
				premise_id
			FROM ' || table_name || '
		);

		CREATE INDEX ix_houses_premises_house_id ON houses_premises (house_id);
		CREATE INDEX ix_houses_premises_premise_id ON houses_premises (premise_id);
		ANALYZE houses_premises;
	';

	RETURN query
	SELECT
		fh.fias_house_guid::varchar AS houseGuid,
		(fh.rajon || ' ' || fh.rajon_short)::varchar AS municipality,
		(CASE WHEN fh.gorod_short || ' ' || fh.gorod NOTNULL THEN fh.gorod_short || ' ' || fh.gorod ELSE fh.npunkt_short || ' ' || fh.npunkt END)::varchar AS locality,
		(fh.ulica_short || ' ' || fh.ulica)::varchar AS ulica,
		fh.house_num AS houseNum,
		fh.struc_num AS strucNum,
		fh.build_num AS buildNum,
		ht.dict_code AS houseTypeCode,
		ht.house_type AS houseTypeName,
		gh.floor_count AS floors,
		gp.premise_id AS premiseid,
		gp.premise_number AS premisenum,
		pc.dict_code AS premisetypecode,
		pc.premise_category AS premisetypename
	FROM houses_premises hp
	LEFT JOIN data.gf_premise gp ON gp.premise_id = hp.premise_id
	LEFT JOIN nsi.nsi_premise_category pc ON pc.premise_category_id = gp.premise_category_id AND NOT pc.is_del
	JOIN data.gf_house gh ON gh.house_id = hp.house_id
	LEFT JOIN nsi.nsi_house_type ht ON ht.house_type_id = gh.house_type_id AND NOT ht.is_del
	JOIN fias_house fh ON fh.id = gh.fias_address_id;
END;
$function$;");
                #endregion

                #region GKHRIS-6469 GKHRIS-6565
                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_main_data_house(table_name varchar)
 RETURNS TABLE(houseguid varchar, municipality varchar, locality varchar, ulica varchar, housenum varchar, strucnum varchar, buildnum varchar, housetypecode varchar, housetypename varchar, signmeteringdevice boolean)
 LANGUAGE plpgsql
AS $function$
DECLARE
	rec record;
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM ' || table_name || '
		);
	';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS meter_device_table_name,
			NULL::TEXT AS meter_device_house_table_name,
			c.owner_contragent_id
		FROM pg_catalog.pg_tables pt, contragents c
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'meter_device_' || c.owner_contragent_id
	);

	UPDATE partitions p SET meter_device_house_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'meter_device_house_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	/* data.meter_device.meter_device_type_id */
	-- 1 Коллективный (общедомовой)
	-- 2 Индивидуальный в МКД
	-- 3 Квартирный
	-- 4 Комнатный
	-- 5 Индивидуальный в жилом доме или доме блокированной застройки

	EXECUTE '
		DROP TABLE IF EXISTS houses;
		CREATE TEMP TABLE houses AS (
			SELECT DISTINCT
				house_id
			FROM ' || table_name || '
		);

		CREATE INDEX ix_houses ON houses (house_id);
		ANALYZE houses;
	';

	DROP TABLE IF EXISTS meter_houses;
	CREATE TEMP TABLE meter_houses (
		house_id bigint
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			INSERT INTO meter_houses
			SELECT h.house_id
			FROM houses h
			WHERE EXISTS (SELECT FROM data_part.' || rec.meter_device_table_name || ' md
				WHERE md.house_id = h.house_id AND NOT md.is_del AND NOT md.meter_device_type_id IN (1, 5));
		';

		IF rec.meter_device_house_table_name NOTNULL THEN
			EXECUTE '
				INSERT INTO meter_houses
				SELECT h.house_id
				FROM houses h
				WHERE EXISTS (SELECT FROM data_part.' || rec.meter_device_house_table_name || ' mdh
					WHERE mdh.house_id = H.house_id AND NOT mdh.is_del);
			';
		END IF;
	END LOOP;

	CREATE INDEX ix_meter_houses ON meter_houses (house_id);
	ANALYZE meter_houses;

	RETURN query
	SELECT
		fh.fias_house_guid::varchar AS houseGuid,
		(fh.rajon || ' ' || fh.rajon_short)::varchar AS municipality,
		(CASE WHEN fh.gorod_short || ' ' || fh.gorod NOTNULL THEN fh.gorod_short || ' ' || fh.gorod ELSE fh.npunkt_short || ' ' || fh.npunkt END)::varchar AS locality,
		(fh.ulica_short || ' ' || fh.ulica)::varchar AS ulica,
		fh.house_num AS houseNum,
		fh.struc_num AS strucNum,
		fh.build_num AS buildNum,
		ht.dict_code AS houseTypeCode,
		ht.house_type AS houseTypeName,
		EXISTS (SELECT FROM meter_houses mh WHERE mh.house_id = h.house_id) AS signMeteringDevice
	FROM houses h
	JOIN data.gf_house gh ON gh.house_id = h.house_id
	LEFT JOIN nsi.nsi_house_type ht ON ht.house_type_id = gh.house_type_id AND NOT ht.is_del
	JOIN fias_house fh ON fh.id = gh.fias_address_id;
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_main_data_premise(table_name varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, premisenum varchar, square numeric, numberresidents integer, premisetypecode varchar, premisetypename varchar, premisecharacteristicscode integer, premisecharacteristicsname varchar, signmeteringdevice boolean)
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
   	live_count_prm_id bigint;
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM ' || table_name || '
		);
	';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS meter_device_table_name,
			NULL::TEXT AS meter_device_premise_table_name,
			c.owner_contragent_id
		FROM pg_catalog.pg_tables pt, contragents c
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'meter_device_' || c.owner_contragent_id
	);

	UPDATE partitions p SET meter_device_premise_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'meter_device_premise_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	/* data.meter_device.meter_device_type_id */
	-- 1 Коллективный (общедомовой)
	-- 2 Индивидуальный в МКД
	-- 3 Квартирный
	-- 4 Комнатный
	-- 5 Индивидуальный в жилом доме или доме блокированной застройки

	/* 
	 * Функция выгружает FALSE под signMeteringDevice в случае,
	 * когда ПУ относится к комнате, которое связано с помещением.
	 * Если для такого помещения нужно указывать signMeteringDevice = TRUE,
	 * тогда нужно убрать NOT md.meter_device_type_id = 4 (поиск по файлу)
	 * */

	EXECUTE '
		DROP TABLE IF EXISTS premises;
		CREATE TEMP TABLE premises AS (
			SELECT DISTINCT ON (premise_id)
				house_guid,
				premise_id,
				owner_contragent_id
			FROM ' || table_name || '
			WHERE premise_id NOTNULL
		);

		CREATE INDEX ix_premises ON premises (premise_id);
		ANALYZE premises;
	';

	DROP TABLE IF EXISTS meter_premises;
	CREATE TEMP TABLE meter_premises (
		premise_id bigint
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			INSERT INTO meter_premises
			SELECT p.premise_id
			FROM premises p
			WHERE EXISTS (SELECT FROM data_part.' || rec.meter_device_table_name || ' md
					WHERE md.premise_id = p.premise_id AND NOT md.is_del AND NOT md.meter_device_type_id = 4);
		';

		IF rec.meter_device_premise_table_name NOTNULL THEN
			EXECUTE '
				INSERT INTO meter_premises
				SELECT p.premise_id
				FROM premises p
				WHERE EXISTS (SELECT FROM data_part.' || rec.meter_device_premise_table_name || ' mdp
						WHERE mdp.premise_id = p.premise_id AND NOT mdp.is_del);
			';
		END IF;
	END LOOP;

	CREATE INDEX ix_meter_premises ON meter_premises (premise_id);
	ANALYZE meter_premises;

	-- Количество лиц, проживающих в квартире
	SELECT gis_prm_id FROM nsi.nsi_gis_prm WHERE dict_code = '20125' INTO live_count_prm_id;

	RETURN query
	EXECUTE '
		SELECT
			p.house_guid AS houseGuid,
			gp.premise_id AS premiseId,
			gp.premise_number AS premiseNum,
			COALESCE(gp.live_square, gp.total_square) AS square,
			pp.val_int AS numberResidents,
			pc.dict_code AS premiseTypeCode,
			pc.premise_category AS premiseTypeName,
			npc.live_premise_char_id AS premiseCharacteristicsCode,
			npc.live_premise_char AS premiseCharacteristicsName,
			EXISTS (SELECT FROM meter_premises mp WHERE mp.premise_id = p.premise_id) AS signMeteringDevice
		FROM premises p
		JOIN data.gf_premise gp ON p.premise_id = gp.premise_id
		LEFT JOIN nsi.nsi_premise_category pc ON pc.premise_category_id = gp.premise_category_id AND NOT pc.is_del
		LEFT JOIN nsi.nsi_live_premise_char npc ON npc.live_premise_char_id = gp.live_premise_char_id AND NOT npc.is_del
		LEFT JOIN data.gis_premise_prm pp ON pp.premise_id = gp.premise_id AND NOT pp.is_del AND pp.gis_prm_id = ' || live_count_prm_id || ' AND pp.owner_contragent_id = p.owner_contragent_id;
	';
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_main_data_room(table_name varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, roomnum varchar, square numeric, numberresidents integer, signmeteringdevice boolean)
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
   	live_count_prm_id bigint;
BEGIN	
	EXECUTE '
		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM ' || table_name || '
		);
	';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS meter_device_room_table_name,
			c.owner_contragent_id
		FROM pg_catalog.pg_tables pt, contragents c
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'meter_device_room_' || c.owner_contragent_id
	);

	EXECUTE '
		DROP TABLE IF EXISTS rooms;
		CREATE TEMP TABLE rooms AS (
			SELECT DISTINCT ON (room_id)
				house_guid,
				premise_id,
				room_id,
				owner_contragent_id
			FROM ' || table_name || '
		);

		CREATE INDEX ix_rooms ON rooms (room_id);
		ANALYZE rooms;
	';

	DROP TABLE IF EXISTS meter_rooms;
	CREATE TEMP TABLE meter_rooms (
		room_id bigint
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			INSERT INTO meter_rooms
			SELECT r.room_id
			FROM rooms r
			WHERE EXISTS (SELECT FROM data_part.' || rec.meter_device_room_table_name ||' mdr
				WHERE mdr.room_id = r.room_id AND NOT mdr.is_del);
		';
	END LOOP;

	CREATE INDEX ix_meter_rooms ON meter_rooms (room_id);
	ANALYZE meter_rooms;

	-- Количество граждан, проживающих в комнате в коммунальной квартире
	SELECT gis_prm_id FROM nsi.nsi_gis_prm WHERE dict_code = '20130' INTO live_count_prm_id;

	RETURN query
	EXECUTE '
		SELECT
			r.house_guid AS houseGuid,
			r.premise_id AS premiseId,
			gr.room_id AS roomId,
			gr.room_number AS roomNum,
			gr.square AS square,
			pp.val_int AS numberResidents,
			EXISTS (SELECT FROM meter_rooms mr WHERE mr.room_id = r.room_id) AS signMeteringDevice
		FROM rooms r
		JOIN data.gf_room gr ON r.room_id = gr.room_id
		LEFT JOIN data.gis_room_prm pp ON pp.room_id = gr.room_id AND NOT pp.is_del AND pp.gis_prm_id =' || live_count_prm_id || ' AND pp.owner_contragent_id = r.owner_contragent_id;';
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_parameters_object(table_name varchar, date_s date)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, signelectricstove boolean, signgasstove boolean, signgascolumn boolean, signfirestove boolean)
 LANGUAGE plpgsql
AS $function$
DECLARE
	rec record;
BEGIN	
	EXECUTE '
		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM ' || table_name || '
		);
	';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS ls_account_prm_table_name
		FROM pg_catalog.pg_tables pt, contragents c
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'ls_account_prm_' || c.owner_contragent_id
	);

	/* nsi.nsi_house_type.dict_code */
	-- 1 Многоквартирный
	-- 2 Квартира коммунального заселения
	-- 3 Общежитие

	/* nsi.nsi_live_premise_char.dict_code */
	-- 1 Отдельная квартира
	-- 2 Квартира коммунального заселения
	-- 3 Общежитие

	/* nsi.bill_prm_name.nzp_prm */
	-- 1    Признак наличия газовой колонки
	-- 19   Признак наличия электрической плиты
	-- 551  Признак наличия газовой плиты
	-- 1172 Признак наличия огневой плиты

	DROP TABLE IF EXISTS data_prm;
	CREATE TEMP TABLE data_prm AS (
		SELECT prm_id, base_prm_code
		FROM data.prm
		WHERE NOT is_del AND base_prm_code IN (1, 19, 551, 1172)
	);

	CREATE INDEX index_data_prm ON data_prm (prm_id);
	ANALYZE data_prm;

	DROP TABLE IF EXISTS ls_prms;
	CREATE TEMP TABLE ls_prms (
		ls_account_id bigint,
		nzp_prm_1 varchar,
		nzp_prm_19 varchar,
		nzp_prm_551 varchar,
		nzp_prm_1172 varchar,
		object_edit_date timestamp
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			DROP TABLE IF EXISTS report_month_prms;
			CREATE TEMP TABLE report_month_prms AS (
				SELECT DISTINCT
					lap.ls_account_id,
					lap.prm_id,
					lap.prm_val,
					lap.object_edit_date
				FROM data_part.' || rec.ls_account_prm_table_name || ' lap
				WHERE NOT lap.is_del AND lap.report_month = ''' || date_s || '''::date
					AND EXISTS (SELECT FROM ' || table_name || ' tt WHERE tt.ls_account_id = lap.ls_account_id)
			);
		';

		INSERT INTO ls_prms
		SELECT
			rmp.ls_account_id,
			MAX(CASE WHEN dp.base_prm_code = 1 THEN rmp.prm_val END) AS nzp_prm_1,
			MAX(CASE WHEN dp.base_prm_code = 19 THEN rmp.prm_val END) AS nzp_prm_19,
			MAX(CASE WHEN dp.base_prm_code = 551 THEN rmp.prm_val END) AS nzp_prm_551,
			MAX(CASE WHEN dp.base_prm_code = 1172 THEN rmp.prm_val END) AS nzp_prm_1172,
			MAX(rmp.object_edit_date) AS object_edit_date
		FROM report_month_prms rmp
		JOIN data_prm dp ON dp.prm_id = rmp.prm_id
		GROUP BY rmp.ls_account_id;
	END LOOP;

	CREATE INDEX index_ls_prms ON ls_prms (ls_account_id);
	ANALYZE ls_prms;

	RETURN query
	EXECUTE '
		SELECT DISTINCT ON (tt.house_id, tt.premise_id, tt.room_id)
			tt.house_guid AS houseGuid,
			tt.premise_id AS premiseId,
			tt.room_id AS roomId,
			(CASE
				WHEN npc.dict_code = ''1'' THEN
					(CASE
						WHEN lp.nzp_prm_19 = ''1'' THEN TRUE
						WHEN lp.nzp_prm_19 = ''2'' THEN FALSE
						ELSE NULL
					END)
				ELSE NULL
			END) AS SignElectricStove,
	 		(CASE
				WHEN npc.dict_code = ''1'' THEN
					(CASE
						WHEN lp.nzp_prm_551 = ''1'' THEN TRUE
						WHEN lp.nzp_prm_551 = ''2'' THEN FALSE
						ELSE NULL
					END)
				ELSE NULL
			END) as signgasstove,
			(CASE
				WHEN lp.nzp_prm_1 = ''1'' THEN TRUE
				WHEN lp.nzp_prm_1 = ''2'' THEN FALSE
				ELSE NULL
			END) as signgascolumn,
			(CASE
				WHEN lp.nzp_prm_1172 = ''1'' THEN TRUE
				WHEN lp.nzp_prm_1172 = ''2'' THEN FALSE
				ELSE NULL
			END) as signfirestove
		FROM ' || table_name || ' tt
		LEFT JOIN ls_prms lp ON lp.ls_account_id = tt.ls_account_id
		LEFT JOIN data.gf_premise gp ON tt.premise_id = gp.premise_id
		LEFT JOIN nsi.nsi_live_premise_char npc ON npc.live_premise_char_id = gp.live_premise_char_id AND NOT npc.is_del
		ORDER BY tt.house_id, tt.premise_id, tt.room_id, lp.object_edit_date DESC;
	';
END;
$function$;");
                #endregion

                #region GKHRIS-6470 GKHRIS-6565
                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_information_house(table_name varchar)
 RETURNS TABLE(houseguid varchar, municipality varchar, locality varchar, ulica varchar, housenum varchar, strucnum varchar, buildnum varchar, housetypecode varchar, housetypename varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS houses;
		CREATE TEMP TABLE houses AS (
			SELECT DISTINCT
				house_id
			FROM ' || table_name || '
		);

		CREATE INDEX ix_houses ON houses (house_id);
		ANALYZE houses;
	';

	RETURN query
	SELECT
		fh.fias_house_guid::varchar AS houseGuid,
		(fh.rajon || ' ' || fh.rajon_short)::varchar AS municipality,
		(CASE WHEN fh.gorod_short || ' ' || fh.gorod NOTNULL THEN fh.gorod_short || ' ' || fh.gorod ELSE fh.npunkt_short || ' ' || fh.npunkt END)::varchar AS locality,
		(fh.ulica_short || ' ' || fh.ulica)::varchar AS ulica,
		fh.house_num AS houseNum,
		fh.struc_num AS strucNum,
		fh.build_num AS buildNum,
		ht.dict_code AS houseTypeCode,
		ht.house_type AS houseTypeName
	FROM houses h
	JOIN data.gf_house gh ON gh.house_id = h.house_id
	LEFT JOIN nsi.nsi_house_type ht ON ht.house_type_id = gh.house_type_id AND NOT ht.is_del
	JOIN fias_house fh ON fh.id = gh.fias_address_id;
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_information_premise(table_name varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, premisenum varchar, premisetypecode varchar, premisetypename varchar, premisecharacteristicscode varchar, premisecharacteristicsname varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS premises;
		CREATE TEMP TABLE premises AS (
			SELECT DISTINCT ON (premise_id)
				house_guid,
				premise_id
			FROM ' || table_name || '
			WHERE premise_id NOTNULL
		);

		CREATE INDEX ix_premises ON premises (premise_id);
		ANALYZE premises;
	';

	RETURN query
	SELECT
		p.house_guid AS houseGuid,
		gp.premise_id AS premiseId,
		gp.premise_number AS premiseNum,
		pc.dict_code AS premiseTypeCode,
		pc.premise_category AS premiseTypeName,
		npc.dict_code AS premiseCharacteristicsCode,
		npc.live_premise_char AS premiseCharacteristicsName
	FROM premises p
	JOIN data.gf_premise gp ON gp.premise_id = p.premise_id
	LEFT JOIN nsi.nsi_premise_category pc ON pc.premise_category_id = gp.premise_category_id AND NOT pc.is_del
	LEFT JOIN nsi.nsi_live_premise_char npc ON npc.live_premise_char_id = gp.live_premise_char_id AND NOT npc.is_del;
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_information_room(table_name varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, roomnum varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS rooms;
		CREATE TEMP TABLE rooms AS (
			SELECT DISTINCT ON (room_id)
				house_guid,
				premise_id,
				room_id
			FROM ' || table_name || '
		);

		CREATE INDEX ix_rooms ON rooms (room_id);
		ANALYZE rooms;
	';

	RETURN query
	SELECT
		r.house_guid AS houseGuid,
		r.premise_id AS premiseId,
		gr.room_id AS roomId,
		gr.room_number AS roomNum
	FROM rooms r
	JOIN data.gf_room gr ON r.room_id = gr.room_id;
END;
$function$;");

                sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION public.tariff_get_information_tariff(table_name varchar, date_s date)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, lsaccountid bigint, provider varchar, inn varchar, kpp varchar, service varchar, tariff numeric, metriccalculation boolean, volume numeric, standard numeric, unitid bigint, unitname varchar)
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
	report_month_part text;
BEGIN
	report_month_part := 'charge_' || to_char(date_s, 'yyyy_mm_');

	EXECUTE '
		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM ' || table_name || '
		);
	';

	DROP TABLE IF EXISTS partitions;
	CREATE TEMP TABLE partitions AS (
		SELECT
			pt.tablename AS charge_table_name,
			NULL::TEXT AS pay_doc_table_name,
			NULL::TEXT AS pay_doc_charge_table_name,
			c.owner_contragent_id
		FROM pg_catalog.pg_tables pt, contragents c
		WHERE pt.schemaname = 'data_part' AND pt.tablename = report_month_part || c.owner_contragent_id
	);

	UPDATE partitions p SET pay_doc_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	UPDATE partitions p SET pay_doc_charge_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_charge_' || p.owner_contragent_id AND pt.schemaname = 'data_part';

	DROP TABLE IF EXISTS ls_tariffs;
	CREATE TEMP TABLE ls_tariffs (
		ls_account_id bigint,
		tariff numeric,
		metricCalculation boolean,
		service varchar,
		service_id bigint,
		okei_id bigint,
		supplier_contragent_id bigint,
		volume numeric,
		standard numeric
	);

	FOR rec IN SELECT * FROM partitions
	LOOP
		EXECUTE '
			INSERT INTO ls_tariffs
			SELECT DISTINCT ON (c.ls_account_id, s.service_id, c.tarif)
				c.ls_account_id,
				c.tarif AS tariff,
				c.is_pu_rashod AS metricCalculation,
				s.service,
				s.service_id,
				s.okei_id,
			' || CASE WHEN rec.pay_doc_charge_table_name NOTNULL THEN '
				pdc.supplier_contragent_id,
				(CASE WHEN c.is_pu_rashod THEN pdc.ind_serv_volume ELSE NULL::numeric END) AS volume,
				(CASE WHEN NOT c.is_pu_rashod THEN pdc.ind_norm ELSE NULL END) AS standard
			' ELSE '
				NULL::bigint AS supplier_contragent_id,
				NULL::numeric AS volume,
				NULL::numeric AS standard
			' END || '
			FROM ' || table_name || ' tt
			JOIN data_part.' || rec.charge_table_name || ' c ON c.ls_account_id = tt.ls_account_id AND NOT c.is_del AND c.tarif NOTNULL AND c.tarif <> 0
			JOIN data.service s ON s.service_id = c.service_id AND NOT s.is_del
			' || CASE WHEN rec.pay_doc_table_name NOTNULL AND rec.pay_doc_charge_table_name NOTNULL THEN '
			LEFT JOIN (data_part.' || rec.pay_doc_charge_table_name || ' pdc
					JOIN data_part.' || rec.pay_doc_table_name || ' pd ON pdc.pay_doc_id = pd.pay_doc_id AND NOT pd.is_del)
					ON c.ls_account_id = pd.ls_account_id AND c.service_id = pdc.service_id AND NOT pdc.is_del
			' ELSE '' END || ';
		';
	END LOOP;

	CREATE INDEX index_ls_tariffs_ls_account_id ON ls_tariffs (ls_account_id);
	CREATE INDEX index_ls_tariffs_supplier_contragent_id ON ls_tariffs (supplier_contragent_id);
	CREATE INDEX index_ls_tariffs_okei_id ON ls_tariffs (okei_id);
	ANALYZE ls_tariffs;

	RETURN query
	EXECUTE '
		SELECT
			tt.house_guid AS houseGuid,
			TT.premise_id AS premiseid,
			TT.room_id AS roomid,
			tt.ls_account_id AS lsaccountid,
			c.full_name AS provider,
			c.inn,
			c.kpp,
			lT.service,
			lT.tariff,
			lT.metriccalculation,
			lT.volume,
			lT.standard,
			ok.okei_id::bigint AS unitid,
			ok.okei_name AS unitname
		FROM ' || table_name || ' tt
		JOIN ls_tariffs lt ON lt.ls_account_id = tt.ls_account_id
		LEFT JOIN nsi.okei ok ON ok.okei_id = lt.okei_id AND NOT ok.is_del
		LEFT JOIN data.contragent c ON c.contragent_id = lt.supplier_contragent_id AND NOT c.is_del;
	';
END;
$function$;");
                #endregion
            }
        }

        /// <inheritdoc />
        public override void Down()
        {
            using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
	            var sql = string.Join("\n",
		            this.GetSqlDropFuncWithTextAndDate("tariff_get_ls_account_info_table_name"),
                    this.GetSqlDropFuncWithText("tariff_get_consumer_full_address"),
                    this.GetSqlDropFuncWithText("tariff_main_data_house"),
                    this.GetSqlDropFuncWithText("tariff_main_data_premise"),
                    this.GetSqlDropFuncWithText("tariff_main_data_room"),
                    this.GetSqlDropFuncWithTextAndDate("tariff_parameters_object"),
                    this.GetSqlDropFuncWithText("tariff_get_information_house"),
                    this.GetSqlDropFuncWithText("tariff_get_information_premise"),
                    this.GetSqlDropFuncWithText("tariff_get_information_room"),
                    this.GetSqlDropFuncWithTextAndDate("tariff_get_information_tariff"));

                sqlExecutor.ExecuteSql(sql);
            }
        }

        /// <summary>
        /// Получить текст запроса для удаления функции
        /// </summary>
        private string GetSqlDropFuncWithText(string funcName) => $"DROP FUNCTION IF EXISTS public.{funcName}(varchar);";

        /// <summary>
        /// Получить текст запроса для удаления функции
        /// </summary>
        private string GetSqlDropFuncWithTextAndDate(string funcName) => $"DROP FUNCTION IF EXISTS public.{funcName}(varchar, date);";
    }
}