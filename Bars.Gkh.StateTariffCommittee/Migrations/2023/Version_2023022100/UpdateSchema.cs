namespace Bars.Gkh.StateTariffCommittee.Migrations._2023.Version_2023022100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.SqlExecutor;

	///<inheritdoc />
    [Migration("2023022100")]
    [MigrationDependsOn(typeof(Version_2023022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
			using (var sqlExecutor = new RisDatabaseSqlExecutor())
			{
				sqlExecutor.ExecuteSql($@"
					CREATE OR REPLACE FUNCTION tariff.create_ls_account_info_table(
						table_name character varying,
						oktmo_mo character varying,
						date_s date)
						RETURNS void
						LANGUAGE 'plpgsql'
					AS
					$FUNCTION$
					DECLARE
						rec               record;
						month_part text;
						any_recs          boolean;
					BEGIN
						IF NOT EXISTS(SELECT FROM gkh_dict_municipality m WHERE m.oktmo = oktmo_mo) THEN
							RAISE EXCEPTION USING MESSAGE = 'Код ОКТМО ' || oktmo_mo || ' не найден';
						END IF;
					
						DROP TABLE IF EXISTS houses;
						CREATE TEMP TABLE houses AS
						SELECT gh.house_id,
						       fh.fias_house_guid::varchar AS house_guid
						FROM data.gf_house gh
							     JOIN public.fias_house fh ON gh.fias_address_id = fh.id
							                                      AND fh.oktmo IN (SELECT m.oktmo
							                                                       FROM gkh_dict_municipality m
							                                                       WHERE m.oktmo = oktmo_mo
							                                                       UNION
							                                                       SELECT m3.oktmo
							                                                       FROM gkh_dict_municipality m2
								                                                            LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
							                                                       WHERE m2.oktmo = oktmo_mo
							                                                       UNION
							                                                       SELECT m4.oktmo
							                                                       FROM gkh_dict_municipality m2
								                                                            LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
								                                                            LEFT JOIN gkh_dict_municipality m4 ON m4.parent_mo_id = m3.id
							                                                       WHERE m2.oktmo = oktmo_mo)
						WHERE NOT gh.is_del
						  AND gh.ogf_annul_reason_id ISNULL;
					
						CREATE INDEX ON houses (house_id);
						ANALYZE houses;
					
						month_part := TO_CHAR(date_s, 'yyyy_mm');
					
						DROP TABLE IF EXISTS partitions;
						CREATE TEMP TABLE partitions AS
						SELECT pt.tablename                     AS pay_doc_charge_table_name,
						       NULL::TEXT                       AS pay_doc_table_name,
						       NULL::TEXT                       AS ls_account_table_name,
						       NULL::TEXT                       AS ls_address_table_name,
						       SPLIT_PART(pt.tablename, '_', 6) AS owner_contragent_id
						FROM pg_catalog.pg_tables pt
						WHERE pt.schemaname = 'data_part'
						  AND pt.tablename LIKE 'pay_doc_charge_' || month_part || '_%';
					
						UPDATE partitions p
						SET pay_doc_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'pay_doc_' || month_part || '_' || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						UPDATE partitions p
						SET ls_account_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'ls_account_' || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						DELETE FROM partitions WHERE ls_account_table_name ISNULL;
					
						UPDATE partitions p
						SET ls_address_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'ls_address_' || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						DROP TABLE IF EXISTS ls_account_houses;
						CREATE TEMP TABLE ls_account_houses
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							house_id            bigint,
							owner_contragent_id bigint
						);
					
						DROP TABLE IF EXISTS ls_account_premises;
						CREATE TEMP TABLE ls_account_premises
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							premise_id          bigint,
							owner_contragent_id bigint
						);
					
						DROP TABLE IF EXISTS ls_account_rooms;
						CREATE TEMP TABLE ls_account_rooms
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							room_id             bigint,
							owner_contragent_id bigint
						);
					
						FOR rec IN SELECT * FROM partitions
							LOOP
								EXECUTE '
									DROP TABLE IF EXISTS ls_houses;
									CREATE TEMP TABLE ls_houses AS
									SELECT DISTINCT
										pd.ls_account_id,
								        la.live_square,
										la.gil_cnt,
										la.house_id,
										la.owner_contragent_id
									FROM data_part.' || rec.pay_doc_charge_table_name || ' c
									JOIN data_part.' || rec.pay_doc_table_name || ' pd ON NOT pd.is_del AND c.pay_doc_id = pd.pay_doc_id
									JOIN data_part.' || rec.ls_account_table_name || ' la ON pd.ls_account_id = la.ls_account_id AND NOT la.is_del AND la.closed_on ISNULL
									WHERE NOT c.is_del AND c.rate NOTNULL AND c.rate <> 0;
								';
					
								IF EXISTS(SELECT FROM ls_houses) THEN
									IF rec.ls_address_table_name NOTNULL THEN
										CREATE INDEX index_ls_houses_ls_account_id ON ls_houses (ls_account_id);
										CREATE INDEX index_ls_houses_house_id ON ls_houses (house_id);
										ANALYZE ls_houses;
					
										EXECUTE '
											DROP TABLE IF EXISTS premises_rooms;
											CREATE TEMP TABLE premises_rooms AS
												SELECT DISTINCT
													lh.ls_account_id,
													lh.live_square,
													lh.gil_cnt,
													lh.house_id,
													la.premise_id,
													la.room_id,
													lh.owner_contragent_id
												FROM ls_houses lh
												JOIN data_part.' || rec.ls_address_table_name || ' la ON la.ls_account_id = lh.ls_account_id AND NOT la.is_del AND (la.premise_id NOTNULL OR la.room_id NOTNULL);
										';
					
										CREATE INDEX index_premises_rooms ON premises_rooms (house_id);
										ANALYZE premises_rooms;
					
										INSERT INTO ls_account_rooms
										SELECT pr.ls_account_id,
										       pr.live_square,
										       pr.gil_cnt,
										       pr.room_id,
										       pr.owner_contragent_id
										FROM premises_rooms pr
										WHERE pr.room_id NOTNULL;
					
										INSERT INTO ls_account_premises
										SELECT pr.ls_account_id,
										       pr.live_square,
										       pr.gil_cnt,
										       pr.premise_id,
										       pr.owner_contragent_id
										FROM premises_rooms pr
										WHERE pr.room_id ISNULL;
					
										INSERT INTO ls_account_houses
										SELECT lh.ls_account_id,
										       lh.live_square,
										       lh.gil_cnt,
										       lh.house_id,
										       lh.owner_contragent_id
										FROM ls_houses lh
										WHERE lh.house_id NOTNULL
										  AND NOT EXISTS(SELECT FROM premises_rooms pr WHERE pr.house_id = lh.house_id);
									ELSE
										INSERT INTO ls_account_houses
										SELECT lh.ls_account_id,
										       lh.live_square,
										       lh.gil_cnt,
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
					
						EXECUTE '
							DROP TABLE IF EXISTS tariff.' || table_name || ';
							CREATE TABLE tariff.' || table_name || ' (
								ls_account_id bigint,
								live_square numeric,
								gil_cnt integer,
								house_id bigint,
								house_guid varchar,
								premise_id bigint,
								room_id bigint,
								owner_contragent_id bigint
							);
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lar.ls_account_id,
								lar.live_square,
								lar.gil_cnt,
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
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lap.ls_account_id,
								lap.live_square,
								lap.gil_cnt,
								gh.house_id,
								gh.house_guid,
								gp.premise_id,
								NULL::bigint,
								lap.owner_contragent_id
							FROM ls_account_premises lap
							JOIN data.gf_premise gp ON gp.premise_id = lap.premise_id AND NOT gp.is_del AND gp.ogf_annul_reason_id ISNULL
							JOIN houses gh ON gh.house_id = gp.house_id;
					
							CREATE INDEX ix_' || table_name || ' ON tariff.' || table_name || ' (ls_account_id, house_id);
							ANALYZE tariff.' || table_name || ';
					
							DELETE FROM ls_account_houses lah WHERE EXISTS (SELECT FROM tariff.' || table_name || ' fi WHERE fi.house_id = lah.house_id AND fi.ls_account_id = lah.ls_account_id);
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lah.ls_account_id,
								lah.live_square,
								lah.gil_cnt,
								gh.house_id,
								gh.house_guid,
								NULL::bigint,
								NULL::bigint,
								lah.owner_contragent_id
							FROM ls_account_houses lah
							JOIN houses gh ON gh.house_id = lah.house_id;
						';
					
						EXECUTE '
							SELECT NOT EXISTS (SELECT FROM tariff.' || table_name || ');
						' INTO any_recs;
					
						IF any_recs THEN
							RAISE EXCEPTION 'Не найдено данных удовлетворяющих заданным критериям поиска';
						END IF;
					
						EXECUTE '
							CREATE INDEX ix_' || table_name || '_ls_account_id ON tariff.' || table_name || ' (ls_account_id);
							CREATE INDEX ix_' || table_name || '_house_id ON tariff.' || table_name || ' (house_id);
							CREATE INDEX ix_' || table_name || '_premise_id ON tariff.' || table_name || ' (premise_id);
							CREATE INDEX ix_' || table_name || '_room_id ON tariff.' || table_name || ' (room_id);
							CREATE INDEX ix_' || table_name || '_owner_contragent_id ON tariff.' || table_name || ' (owner_contragent_id);
							ANALYZE tariff.' || table_name || ';
						';
					END;
					$FUNCTION$;");
			}
		}

        /// <inheritdoc />
        public override void Down()
        {
            using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
				sqlExecutor.ExecuteSql($@"
					CREATE OR REPLACE FUNCTION tariff.create_ls_account_info_table(
						table_name character varying,
						oktmo_mo character varying,
						date_s date)
						RETURNS void
						LANGUAGE 'plpgsql'
					AS
					$FUNCTION$
					DECLARE
						rec               record;
						report_month_part text;
						any_recs          boolean;
					BEGIN
						IF NOT EXISTS(SELECT FROM gkh_dict_municipality m WHERE m.oktmo = oktmo_mo) THEN
							RAISE EXCEPTION USING MESSAGE = 'Код ОКТМО ' || oktmo_mo || ' не найден';
						END IF;
					
						DROP TABLE IF EXISTS houses;
						CREATE TEMP TABLE houses AS
						SELECT gh.house_id,
						       fh.fias_house_guid::varchar AS house_guid
						FROM data.gf_house gh
							     JOIN public.fias_house fh ON gh.fias_address_id = fh.id
							                                      AND fh.oktmo IN (SELECT m.oktmo
							                                                       FROM gkh_dict_municipality m
							                                                       WHERE m.oktmo = oktmo_mo
							                                                       UNION
							                                                       SELECT m3.oktmo
							                                                       FROM gkh_dict_municipality m2
								                                                            LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
							                                                       WHERE m2.oktmo = oktmo_mo
							                                                       UNION
							                                                       SELECT m4.oktmo
							                                                       FROM gkh_dict_municipality m2
								                                                            LEFT JOIN gkh_dict_municipality m3 ON m3.parent_mo_id = m2.id
								                                                            LEFT JOIN gkh_dict_municipality m4 ON m4.parent_mo_id = m3.id
							                                                       WHERE m2.oktmo = oktmo_mo)
						WHERE NOT gh.is_del
						  AND gh.ogf_annul_reason_id ISNULL;
					
						CREATE INDEX ON houses (house_id);
						ANALYZE houses;
					
						report_month_part := 'charge_' || TO_CHAR(date_s, 'yyyy_mm') || '_%';
					
						DROP TABLE IF EXISTS partitions;
						CREATE TEMP TABLE partitions AS (SELECT pt.tablename                     AS charge_table_name,
						                                        NULL::TEXT                       AS ls_account_table_name,
						                                        NULL::TEXT                       AS ls_address_table_name,
						                                        SPLIT_PART(pt.tablename, '_', 4) AS owner_contragent_id
						                                 FROM pg_catalog.pg_tables pt
						                                 WHERE pt.schemaname = 'data_part'
							                               AND pt.tablename LIKE report_month_part);
					
						UPDATE partitions p
						SET ls_account_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'ls_account_' || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						DELETE FROM partitions WHERE ls_account_table_name ISNULL;
					
						UPDATE partitions p
						SET ls_address_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'ls_address_' || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						DROP TABLE IF EXISTS ls_account_houses;
						CREATE TEMP TABLE ls_account_houses
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							house_id            bigint,
							owner_contragent_id bigint
						);
					
						DROP TABLE IF EXISTS ls_account_premises;
						CREATE TEMP TABLE ls_account_premises
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							premise_id          bigint,
							owner_contragent_id bigint
						);
					
						DROP TABLE IF EXISTS ls_account_rooms;
						CREATE TEMP TABLE ls_account_rooms
						(
							ls_account_id       bigint,
							live_square         numeric,
							gil_cnt             integer,
							room_id             bigint,
							owner_contragent_id bigint
						);
					
						FOR rec IN SELECT * FROM partitions
							LOOP
								EXECUTE '
								DROP TABLE IF EXISTS ls_houses;
								CREATE TEMP TABLE ls_houses AS (
									SELECT DISTINCT
										c.ls_account_id,
										la.live_square,
										la.gil_cnt,
										la.house_id,
										la.owner_contragent_id
									FROM data_part.' || rec.charge_table_name || ' c
									JOIN data_part.' || rec.ls_account_table_name || ' la ON c.ls_account_id = la.ls_account_id AND NOT la.is_del AND la.closed_on ISNULL
									WHERE NOT c.is_del AND c.tarif NOTNULL AND c.tarif <> 0
								);';
					
								IF EXISTS(SELECT FROM ls_houses) THEN
									IF rec.ls_address_table_name NOTNULL THEN
										CREATE INDEX index_ls_houses_ls_account_id ON ls_houses (ls_account_id);
										CREATE INDEX index_ls_houses_house_id ON ls_houses (house_id);
										ANALYZE ls_houses;
					
										EXECUTE '
										DROP TABLE IF EXISTS premises_rooms;
										CREATE TEMP TABLE premises_rooms AS (
											SELECT DISTINCT
												lh.ls_account_id,
												lh.live_square,
												lh.gil_cnt,
												lh.house_id,
												la.premise_id,
												la.room_id,
												lh.owner_contragent_id
											FROM ls_houses lh
											JOIN data_part.' || rec.ls_address_table_name || ' la ON la.ls_account_id = lh.ls_account_id AND NOT la.is_del AND (la.premise_id NOTNULL OR la.room_id NOTNULL)
										);';
					
										CREATE INDEX index_premises_rooms ON premises_rooms (house_id);
										ANALYZE premises_rooms;
					
										INSERT INTO ls_account_rooms
										SELECT pr.ls_account_id,
										       pr.live_square,
										       pr.gil_cnt,
										       pr.room_id,
										       pr.owner_contragent_id
										FROM premises_rooms pr
										WHERE pr.room_id NOTNULL;
					
										INSERT INTO ls_account_premises
										SELECT pr.ls_account_id,
										       pr.live_square,
										       pr.gil_cnt,
										       pr.premise_id,
										       pr.owner_contragent_id
										FROM premises_rooms pr
										WHERE pr.room_id ISNULL;
					
										INSERT INTO ls_account_houses
										SELECT lh.ls_account_id,
										       lh.live_square,
										       lh.gil_cnt,
										       lh.house_id,
										       lh.owner_contragent_id
										FROM ls_houses lh
										WHERE lh.house_id NOTNULL
										  AND NOT EXISTS(SELECT FROM premises_rooms pr WHERE pr.house_id = lh.house_id);
									ELSE
										INSERT INTO ls_account_houses
										SELECT lh.ls_account_id,
										       lh.live_square,
										       lh.gil_cnt,
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
					
						EXECUTE '
							DROP TABLE IF EXISTS tariff.' || table_name || ';
							CREATE TABLE tariff.' || table_name || ' (
								ls_account_id bigint,
								live_square numeric,
								gil_cnt integer,
								house_id bigint,
								house_guid varchar,
								premise_id bigint,
								room_id bigint,
								owner_contragent_id bigint
							);
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lar.ls_account_id,
								lar.live_square,
								lar.gil_cnt,
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
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lap.ls_account_id,
								lap.live_square,
								lap.gil_cnt,
								gh.house_id,
								gh.house_guid,
								gp.premise_id,
								NULL::bigint,
								lap.owner_contragent_id
							FROM ls_account_premises lap
							JOIN data.gf_premise gp ON gp.premise_id = lap.premise_id AND NOT gp.is_del AND gp.ogf_annul_reason_id ISNULL
							JOIN houses gh ON gh.house_id = gp.house_id;
					
							CREATE INDEX ix_' || table_name || ' ON tariff.' || table_name || ' (ls_account_id, house_id);
							ANALYZE tariff.' || table_name || ';
					
							DELETE FROM ls_account_houses lah WHERE EXISTS (SELECT FROM tariff.' || table_name || ' fi WHERE fi.house_id = lah.house_id AND fi.ls_account_id = lah.ls_account_id);
					
							INSERT INTO tariff.' || table_name || '
							SELECT
								lah.ls_account_id,
								lah.live_square,
								lah.gil_cnt,
								gh.house_id,
								gh.house_guid,
								NULL::bigint,
								NULL::bigint,
								lah.owner_contragent_id
							FROM ls_account_houses lah
							JOIN houses gh ON gh.house_id = lah.house_id;
						';
					
						EXECUTE '
							SELECT NOT EXISTS (SELECT FROM tariff.' || table_name || ');
						' INTO any_recs;
					
						IF any_recs THEN
							RAISE EXCEPTION 'Не найдено данных удовлетворяющих заданным критериям поиска';
						END IF;
					
						EXECUTE '
							CREATE INDEX ix_' || table_name || '_ls_account_id ON tariff.' || table_name || ' (ls_account_id);
							CREATE INDEX ix_' || table_name || '_house_id ON tariff.' || table_name || ' (house_id);
							CREATE INDEX ix_' || table_name || '_premise_id ON tariff.' || table_name || ' (premise_id);
							CREATE INDEX ix_' || table_name || '_room_id ON tariff.' || table_name || ' (room_id);
							CREATE INDEX ix_' || table_name || '_owner_contragent_id ON tariff.' || table_name || ' (owner_contragent_id);
							ANALYZE tariff.' || table_name || ';
						';
					END;
					$FUNCTION$;");
			}
        }
    }
}