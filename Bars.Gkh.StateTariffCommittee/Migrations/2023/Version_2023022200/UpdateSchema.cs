namespace Bars.Gkh.StateTariffCommittee.Migrations._2023.Version_2023022200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.SqlExecutor;

    [Migration("2023022200")]
    [MigrationDependsOn(typeof(Version_2023022100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        ///<inheritdoc />
        public override void Up()
        {
            using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
                sqlExecutor.ExecuteSql($@"
                    CREATE OR REPLACE FUNCTION tariff.get_information_tariff(
						table_name varchar,
					    date_s date,
					    paging_table_name varchar,
					    page_guid varchar)
						RETURNS TABLE
						(
						   houseguid         varchar,
						   premiseid         bigint,
						   roomid            bigint,
						   lsaccountid       bigint,
						   provider          varchar,
						   inn               varchar,
						   kpp               varchar,
						   service           varchar,
						   tariff            numeric,
						   metriccalculation boolean,
						   volume            numeric,
						   standard          numeric,
						   unitid            bigint,
						   unitname          varchar
						)
						LANGUAGE plpgsql
					AS
					$function$
					DECLARE
						rec               record;
						report_month_part text;
					BEGIN
						report_month_part := TO_CHAR(date_s, 'yyyy_mm_');
					
						EXECUTE '
							DROP TABLE IF EXISTS tariff_houses;
							CREATE TEMP TABLE tariff_houses AS
							SELECT tt.*
							FROM tariff.' || table_name || ' tt
							JOIN tariff.' || paging_table_name || ' pt ON pt.house_id = tt.house_id AND pt.page_guid = ''' || page_guid || ''';
					
							CREATE INDEX ix_tariff_houses_ls_account_id ON tariff_houses (ls_account_id);
							CREATE INDEX ix_tariff_houses_owner_contragent_id ON tariff_houses (owner_contragent_id);
							ANALYZE tariff_houses;
					
							DROP TABLE IF EXISTS contragents;
							CREATE TEMP TABLE contragents AS
							SELECT DISTINCT	owner_contragent_id
							FROM tariff_houses;';
					
						DROP TABLE IF EXISTS partitions;
						CREATE TEMP TABLE partitions AS
						SELECT pt.tablename AS pay_doc_charge_table_name,
						       NULL::TEXT   AS pay_doc_table_name,
						       c.owner_contragent_id
						FROM pg_catalog.pg_tables pt, contragents c
						WHERE pt.schemaname = 'data_part'
						  AND pt.tablename = 'pay_doc_charge_' || report_month_part || c.owner_contragent_id;
					
						UPDATE partitions p
						SET pay_doc_table_name = pt.tablename
						FROM pg_catalog.pg_tables pt
						WHERE pt.tablename = 'pay_doc_' || report_month_part || p.owner_contragent_id
						  AND pt.schemaname = 'data_part';
					
						DROP TABLE IF EXISTS ls_tariffs;
						CREATE TEMP TABLE ls_tariffs
						(
							ls_account_id          bigint,
							tariff                 numeric,
							metricCalculation      boolean,
							service_id             bigint,
							okei_id                bigint,
							supplier_contragent_id bigint,
							volume                 numeric,
							standard               numeric
						);
					
						FOR rec IN SELECT * FROM partitions WHERE pay_doc_table_name NOTNULL AND pay_doc_charge_table_name NOTNULL
							LOOP
								EXECUTE '
									INSERT INTO ls_tariffs
								        SELECT DISTINCT ON (pd.ls_account_id, pdc.service_id, pdc.rate)
					                        pd.ls_account_id,
					                        pdc.rate                     AS tariff,
					                        CASE pdc.ind_volume_type_id
						                        WHEN 1 THEN TRUE
						                        WHEN 2 THEN FALSE
						                        WHEN 3 THEN NULL
						                    END                          AS metricCalculation,
											pdc.service_id,
						                    pdc.supplier_contragent_id,
						                    pdc.ind_serv_volume          AS volume,
					                        pdc.ind_norm                 AS standard
										FROM tariff_houses tt
								            JOIN data_part.' || rec.pay_doc_table_name || ' pd ON NOT pd.is_del AND tt.ls_account_id = pd.ls_account_id
											JOIN data_part.' || rec.pay_doc_charge_table_name || ' pdc ON NOT pdc.is_del AND pd.pay_doc_id = pdc.pay_doc_id
										WHERE pdc.rate NOTNULL AND pdc.rate <> 0;';
							END LOOP;
					
						CREATE INDEX index_ls_tariffs_ls_account_id ON ls_tariffs (ls_account_id);
						CREATE INDEX index_ls_tariffs_supplier_contragent_id ON ls_tariffs (supplier_contragent_id);
						CREATE INDEX index_ls_tariffs_okei_id ON ls_tariffs (okei_id);
						ANALYZE ls_tariffs;
					
						RETURN QUERY
							EXECUTE '
								SELECT
									tt.house_guid AS houseGuid,
									tt.premise_id AS premiseid,
									tt.room_id AS roomid,
									tt.ls_account_id AS lsaccountid,
									c.full_name AS provider,
									c.inn,
									c.kpp,
									s.service,
									lt.tariff,
									lt.metriccalculation,
									lt.volume,
									lt.standard,
									ok.okei_id::bigint AS unitid,
									ok.okei_name AS unitname
								FROM tariff_houses tt
									JOIN ls_tariffs lt ON lt.ls_account_id = tt.ls_account_id
							        JOIN data.service s ON s.service_id = lt.service_id AND NOT s.is_del
									LEFT JOIN nsi.okei ok ON ok.okei_id = s.okei_id AND NOT ok.is_del
									LEFT JOIN data.contragent c ON c.contragent_id = lt.supplier_contragent_id AND NOT c.is_del;';
					END;
					$function$;
                ");
            }
        }

		///<inheritdoc />
		public override void Down()
        {
			using (var sqlExecutor = new RisDatabaseSqlExecutor())
            {
				sqlExecutor.ExecuteSql($@"
					CREATE OR REPLACE FUNCTION tariff.get_information_tariff(table_name varchar, date_s date, paging_table_name varchar, page_guid varchar)
						RETURNS TABLE
						(
							houseguid varchar,
							premiseid bigint,
							roomid bigint,
							lsaccountid bigint,
							provider varchar,
							inn varchar,
							kpp varchar,
							service varchar,
							tariff numeric,
							metriccalculation boolean,
							volume numeric,
							standard numeric,
							unitid bigint,
							unitname varchar
						)
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
								tt.premise_id AS premiseid,
								tt.room_id AS roomid,
								tt.ls_account_id AS lsaccountid,
								c.full_name AS provider,
								c.inn,
								c.kpp,
								lt.service,
								lt.tariff,
								lt.metriccalculation,
								lT.volume,
								lt.standard,
								ok.okei_id::bigint AS unitid,
								ok.okei_name AS unitname
							FROM tariff_houses tt
							JOIN ls_tariffs lt ON lt.ls_account_id = tt.ls_account_id
							LEFT JOIN nsi.okei ok ON ok.okei_id = lt.okei_id AND NOT ok.is_del
							LEFT JOIN data.contragent c ON c.contragent_id = lt.supplier_contragent_id AND NOT c.is_del;
						';
					END;
					$function$;");
			}
		}
    }
}