namespace Bars.Gkh.StateTariffCommittee.Migrations._2023.Version_2023021700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.SqlExecutor;

    [Migration("2023021700")]
    [MigrationDependsOn(typeof(Version_2023020800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
	        using (var sqlExecutor = new RisDatabaseSqlExecutor())
	        {
		        // Расширение для генерации UUID
		        sqlExecutor.ExecuteSql($@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");

		        #region Информация о постраничном выводе
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_paged_table_info(table_name varchar, paging_table_name varchar, page_guid varchar)
 RETURNS TABLE(pageguid varchar, nextpageguid varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	IF page_guid = '' THEN
		EXECUTE '
			DROP TABLE IF EXISTS paged_houses;
			CREATE TEMP TABLE paged_houses AS (
				SELECT
					tt.house_id,
					(sum(tt.premises_count) OVER(ORDER BY tt.house_id))::int/50000 AS page
				FROM (SELECT house_id, count(DISTINCT premise_id) AS premises_count FROM tariff.' || table_name || ' GROUP BY house_id) AS tt
			);

			CREATE INDEX ON paged_houses (page);
			ANALYZE paged_houses;

			DROP TABLE IF EXISTS page_guids;
			CREATE TEMP TABLE page_guids AS (
				SELECT
					uuid_generate_v4()::varchar AS page_guid,
					page,
					row_number() OVER (ORDER BY page) as page_index
				FROM paged_houses
				GROUP BY page
			);

			CREATE INDEX ix_page_guids_page ON page_guids (page);
			CREATE INDEX ix_page_guids_page_index ON page_guids (page_index);
			ANALYZE page_guids;

			DROP TABLE IF EXISTS tariff.' || paging_table_name || ';
			CREATE TABLE tariff.' || paging_table_name || ' AS (
				SELECT
					ph.house_id,
					pg.page_guid,
					pg_next.page_guid AS next_page_guid
				FROM paged_houses ph
				JOIN page_guids pg ON pg.page = ph.page
				LEFT JOIN page_guids pg_next ON pg_next.page_index = pg.page_index + 1
			);

			CREATE INDEX ix_' || paging_table_name || '_page_guid ON tariff.' || paging_table_name || ' (page_guid);
			CREATE INDEX ix_' || paging_table_name || '_house_id ON tariff.' || paging_table_name || ' (house_id);
			ANALYZE tariff.' || paging_table_name || ';
		';

		EXECUTE '
			SELECT page_guid
			FROM page_guids
			WHERE page = (SELECT MIN(page) FROM page_guids)
		' INTO page_guid;
	ELSE
		IF (SELECT count(*) <> 1 FROM pg_catalog.pg_tables pt WHERE pt.schemaname = 'tariff' AND pt.tablename = paging_table_name) THEN
			RAISE EXCEPTION USING MESSAGE = 'Все подготовленные пакеты были отправлены';
		END IF;
	ENd IF;

	EXECUTE '
		DROP TABLE IF EXISTS paged_info_result;
		CREATE TEMP TABLE paged_info_result AS (
			SELECT DISTINCT ON (page_guid)
				page_guid AS pageguid,
				next_page_guid AS nextpageguid
			FROM tariff.' || paging_table_name || '
			WHERE page_guid = ''' || page_guid || '''::varchar
		);
	';

	IF (SELECT count(*) <> 1 FROM paged_info_result) THEN
		RAISE EXCEPTION USING MESSAGE = 'Не удалось определить текущий пакет';
	ENd IF;

	RETURN query
	SELECT * FROM paged_info_result;
END;
$function$;");
		        #endregion

		        #region Метод с тарифами, функция по домам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_house(table_name varchar, paging_table_name varchar, page_guid varchar)
 RETURNS TABLE(houseguid varchar, municipality varchar, locality varchar, ulica varchar, housenum varchar, strucnum varchar, buildnum varchar, housetypecode varchar, housetypename varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS houses;
		CREATE TEMP TABLE houses AS (
			SELECT DISTINCT
				tt.house_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page_guid = ''' || page_guid || '''
			' ELSE '' END || '
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
		        #endregion

		        #region Метод с тарифами, функция по помещениям
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_premise(table_name varchar, paging_table_name varchar, page_guid varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, premisenum varchar, premisetypecode varchar, premisetypename varchar, premisecharacteristicscode varchar, premisecharacteristicsname varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS premises;
		CREATE TEMP TABLE premises AS (
			SELECT DISTINCT ON (tt.premise_id)
				tt.house_guid,
				tt.premise_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page_guid = ''' || page_guid || '''
			' ELSE '' END || '
			WHERE tt.premise_id NOTNULL
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
		        #endregion

		        #region Метод с тарифами, функция по комнатам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_room(table_name varchar, paging_table_name varchar, page_guid varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, roomnum varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS rooms;
		CREATE TEMP TABLE rooms AS (
			SELECT DISTINCT ON (tt.room_id)
				tt.house_guid,
				tt.premise_id,
				tt.room_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page_guid = ''' || page_guid || '''
			' ELSE '' END || '
			WHERE tt.room_id NOTNULL
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
		        #endregion

		        #region Метод с тарифами, функция по тарифам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_tariff(table_name varchar, date_s date, paging_table_name varchar, page_guid varchar)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, lsaccountid bigint, provider varchar, inn varchar, kpp varchar, service varchar, tariff numeric, metriccalculation boolean, volume numeric, standard numeric, unitid bigint, unitname varchar)
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
	report_month_part text;
BEGIN
	report_month_part := to_char(date_s, 'yyyy_mm_');

	EXECUTE '
		DROP TABLE IF EXISTS tariff_houses;
		CREATE TEMP TABLE tariff_houses AS (
			SELECT tt.*
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page_guid = ''' || page_guid || '''
			' ELSE '' END || '
		);

		CREATE INDEX ix_tariff_houses_ls_account_id ON tariff_houses (ls_account_id);
		CREATE INDEX ix_tariff_houses_owner_contragent_id ON tariff_houses (owner_contragent_id);
		ANALYZE tariff_houses;

		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM tariff_houses
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
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'charge_' || report_month_part || c.owner_contragent_id
	);

	UPDATE partitions p SET pay_doc_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_' || report_month_part || p.owner_contragent_id AND pt.schemaname = 'data_part';

	UPDATE partitions p SET pay_doc_charge_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_charge_' || report_month_part || p.owner_contragent_id AND pt.schemaname = 'data_part';

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
			FROM tariff_houses tt
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
		FROM tariff_houses tt
		JOIN ls_tariffs lt ON lt.ls_account_id = tt.ls_account_id
		LEFT JOIN nsi.okei ok ON ok.okei_id = lt.okei_id AND NOT ok.is_del
		LEFT JOIN data.contragent c ON c.contragent_id = lt.supplier_contragent_id AND NOT c.is_del;
	';
END;
$function$;");
		        #endregion

		        sqlExecutor.ExecuteSql($@"
					DROP FUNCTION IF EXISTS tariff.get_paged_table_info(table_name varchar, paging_table_name varchar);
					DROP FUNCTION IF EXISTS tariff.get_information_house(table_name varchar, paging_table_name varchar, page integer);
					DROP FUNCTION IF EXISTS tariff.get_information_premise(table_name varchar, paging_table_name varchar, page integer);
					DROP FUNCTION IF EXISTS tariff.get_information_room(table_name varchar, paging_table_name varchar, page integer);
					DROP FUNCTION IF EXISTS tariff.get_information_tariff(table_name varchar, date_s date, paging_table_name varchar, page integer);
				");
	        }
        }

        /// <inheritdoc />
        public override void Down()
        {
	        using (var sqlExecutor = new RisDatabaseSqlExecutor())
	        {
		        // Расширение для генерации UUID
		        sqlExecutor.ExecuteSql($@"DROP EXTENSION IF EXISTS ""uuid-ossp"";");

		        #region Информация о постраничном выводе
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_paged_table_info(table_name varchar, paging_table_name varchar)
 RETURNS TABLE(page integer, nextpageexists boolean)
 LANGUAGE plpgsql
AS $function$
DECLARE
	curr_page integer;
BEGIN
	IF table_name <> '' THEN
		EXECUTE '
			DROP TABLE IF EXISTS tariff.' || paging_table_name || ';
			CREATE TABLE tariff.' || paging_table_name || ' AS (
				SELECT
					tt.house_id,
					(sum(tt.premises_count) OVER(ORDER BY tt.house_id))::int/120000 AS page
				FROM (SELECT house_id, count(DISTINCT premise_id) AS premises_count FROM tariff.' || table_name || ' GROUP BY house_id) AS tt
			);

			CREATE INDEX ix_' || paging_table_name || '_page ON tariff.' || paging_table_name || ' (page);
			CREATE INDEX ix_' || paging_table_name || '_house_id ON tariff.' || paging_table_name || ' (house_id);
			ANALYZE tariff.' || paging_table_name || ';
		';
	ELSE
		IF (SELECT count(*) <> 1 FROM pg_catalog.pg_tables pt WHERE pt.schemaname = 'tariff' AND pt.tablename = paging_table_name) THEN
			RAISE EXCEPTION USING MESSAGE = 'Все подготовленные пакеты были отправлены';
		END IF;
	END IF;

	EXECUTE '
		SELECT MIN(page)
		FROM tariff.' || paging_table_name || ';
	' INTO curr_page;

	IF curr_page ISNULL THEN
		RAISE EXCEPTION USING MESSAGE = 'Не удалось определить следующий пакет данных';
	END IF;

	RETURN query
	EXECUTE '
		SELECT
			' || curr_page || '::integer AS page,
			EXISTS (SELECT FROM tariff.' || paging_table_name || ' WHERE NOT page = ' || curr_page || ') AS nextpageexists
	';
END;
$function$;");
		        #endregion

		        #region Метод с тарифами, функция по домам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_house(table_name varchar, paging_table_name varchar, page integer)
 RETURNS TABLE(houseguid varchar, municipality varchar, locality varchar, ulica varchar, housenum varchar, strucnum varchar, buildnum varchar, housetypecode varchar, housetypename varchar)
 LANGUAGE plpgsql
AS $function$
DECLARE
	house_table_name text;
	next_page integer;
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS houses;
		CREATE TEMP TABLE houses AS (
			SELECT DISTINCT
				tt.house_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page = ' || page || '
			' ELSE '' END || '
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
		        #endregion

		        #region Метод с тарифами, функция по помещениям
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_premise(table_name varchar, paging_table_name varchar, page integer)
 RETURNS TABLE(houseguid varchar, premiseid bigint, premisenum varchar, premisetypecode varchar, premisetypename varchar, premisecharacteristicscode varchar, premisecharacteristicsname varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS premises;
		CREATE TEMP TABLE premises AS (
			SELECT DISTINCT ON (tt.premise_id)
				tt.house_guid,
				tt.premise_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page = ' || page || '
			' ELSE '' END || '
			WHERE tt.premise_id NOTNULL
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
		        #endregion

		        #region Метод с тарифами, функция по комнатам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_room(table_name varchar, paging_table_name varchar, page integer)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, roomnum varchar)
 LANGUAGE plpgsql
AS $function$
BEGIN
	EXECUTE '
		DROP TABLE IF EXISTS rooms;
		CREATE TEMP TABLE rooms AS (
			SELECT DISTINCT ON (tt.room_id)
				tt.house_guid,
				tt.premise_id,
				tt.room_id
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page = ' || page || '
			' ELSE '' END || '
			WHERE tt.room_id NOTNULL
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
		        #endregion

		        #region Метод с тарифами, функция по тарифам
		        sqlExecutor.ExecuteSql($@"
CREATE OR REPLACE FUNCTION tariff.get_information_tariff(table_name varchar, date_s date, paging_table_name varchar, page integer)
 RETURNS TABLE(houseguid varchar, premiseid bigint, roomid bigint, lsaccountid bigint, provider varchar, inn varchar, kpp varchar, service varchar, tariff numeric, metriccalculation boolean, volume numeric, standard numeric, unitid bigint, unitname varchar)
 LANGUAGE plpgsql
AS $function$
DECLARE
    rec record;
	report_month_part text;
BEGIN
	report_month_part := to_char(date_s, 'yyyy_mm_');

	EXECUTE '
		DROP TABLE IF EXISTS tariff_houses;
		CREATE TEMP TABLE tariff_houses AS (
			SELECT tt.*
			FROM tariff.' || table_name || ' tt
			' || CASE WHEN paging_table_name <> '' THEN '
			JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page = ' || page || '
			' ELSE '' END || '
		);

		CREATE INDEX ix_tariff_houses_ls_account_id ON tariff_houses (ls_account_id);
		CREATE INDEX ix_tariff_houses_owner_contragent_id ON tariff_houses (owner_contragent_id);
		ANALYZE tariff_houses;

		DROP TABLE IF EXISTS contragents;
		CREATE TEMP TABLE contragents AS (
			SELECT DISTINCT
				owner_contragent_id
			FROM tariff_houses
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
		WHERE pt.schemaname = 'data_part' AND pt.tablename = 'charge_' || report_month_part || c.owner_contragent_id
	);

	UPDATE partitions p SET pay_doc_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_' || report_month_part || p.owner_contragent_id AND pt.schemaname = 'data_part';

	UPDATE partitions p SET pay_doc_charge_table_name = pt.tablename
	FROM pg_catalog.pg_tables pt
	WHERE pt.tablename = 'pay_doc_charge_' || report_month_part || p.owner_contragent_id AND pt.schemaname = 'data_part';

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
			FROM tariff_houses tt
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
		FROM tariff_houses tt
		JOIN ls_tariffs lt ON lt.ls_account_id = tt.ls_account_id
		LEFT JOIN nsi.okei ok ON ok.okei_id = lt.okei_id AND NOT ok.is_del
		LEFT JOIN data.contragent c ON c.contragent_id = lt.supplier_contragent_id AND NOT c.is_del;
	';
END;
$function$;");
		        #endregion

		        sqlExecutor.ExecuteSql($@"
					DROP FUNCTION IF EXISTS tariff.get_paged_table_info(table_name varchar, paging_table_name varchar, page_guid varchar);
					DROP FUNCTION IF EXISTS tariff.get_information_house(table_name varchar, paging_table_name varchar, page_guid varchar);
					DROP FUNCTION IF EXISTS tariff.get_information_premise(table_name varchar, paging_table_name varchar, page_guid varchar);
					DROP FUNCTION IF EXISTS tariff.get_information_room(table_name varchar, paging_table_name varchar, page_guid varchar);
					DROP FUNCTION IF EXISTS tariff.get_information_tariff(table_name varchar, date_s date, paging_table_name varchar, page_guid varchar);
				");
	        }
        }
    }
}