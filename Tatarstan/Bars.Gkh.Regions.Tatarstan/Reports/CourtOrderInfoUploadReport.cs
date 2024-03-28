namespace Bars.Gkh.Regions.Tatarstan.Reports
{
	using Bars.B4.Modules.DataExport;
	using Bars.B4.Modules.Reports;
	using Bars.B4.Utils;
	using Bars.Gkh.Regions.Tatarstan.Dto;
	using System;
	using System.Collections.Generic;
    using System.Linq;
	using System.Text;

	public class CourtOrderInfoUploadReport : DataExportReport
	{
		/// <inheritdoc />
		public CourtOrderInfoUploadReport()
			: base(new ReportTemplateBinary(Properties.Resources.CourtOrderInfoUploadPrintForm))
		{
		}

		/// <inheritdoc />
		public override string Name => string.Empty;

		/// <inheritdoc />
		public override void PrepareReport(ReportParams reportParams)
		{
			var connPgmu = reportParams.SimpleReportParams.СписокПараметров["connPgmu"].ToString();
			var records = (List<CourtOrderUploadFileInfoDto>)reportParams.SimpleReportParams.СписокПараметров["records"];

			var tmpTable1 = $"tmp1_{DateTime.Now.Ticks}";
			var tmpTable2 = $"tmp2_{DateTime.Now.Ticks}";

			var sql = new StringBuilder(
				$@"CREATE TEMP TABLE {tmpTable1}
                  (
                  	outer_id bigint,
                  	erc_code bigint,
                  	post_code character(6),
                  	town character(40),
                  	rajon character(40),
                  	ulica text,
                  	ndom character(40),
                  	nkor character(10),
                  	nkvar character(10),
                  	nkvar_n character(10)
                  );

				CREATE TEMP TABLE {tmpTable2} (outer_id bigint, dat_month date, sum_money numeric(12,2));");

			foreach (var rec in records.Where(x => x.PgmuAddress != null))
			{
				var addr = rec.PgmuAddress;
				sql.Append(
					$"INSERT INTO {tmpTable1} VALUES({rec.OuterId}, {addr.ErcCode}, '{addr.PostCode}', '{addr.Town}'," +
					$" '{addr.District}', '{addr.Street}', '{addr.House}', '{addr.Building}', '{addr.Apartment}', '{addr.Room}');");
			}

			sql.Append(
				$@"
                DO $$DECLARE
                	erc_code1 bigint := 0;
                	pkod text;
                	aliases text[];
                	alias text;
                	rec record;
                	pay record;
                BEGIN
                    FOR rec IN SELECT * FROM {tmpTable1} ORDER BY erc_code
                    LOOP
						pkod := null;
                        IF rec.erc_code != erc_code1 THEN
                			EXECUTE 'SELECT array_agg(substring(tablename from 11)  ORDER BY tablename DESC) FROM pg_tables WHERE tablename LIKE ''parameters_' || rec.erc_code || '_%''' INTO aliases;
                			erc_code1 = rec.erc_code;
                		END IF;
                		
                		FOREACH alias IN ARRAY aliases
                		LOOP
                			IF pkod ISNULL THEN
                				EXECUTE 'SELECT array_to_string(array_agg(pkod), '','') pkod
                					     FROM parameters' || alias ||
                					     ' WHERE COALESCE(trim(post_code), '''') = ''' || rec.post_code || ''' ' ||
                					     	'AND COALESCE(trim(town), '''') = ''' || rec.town || ''' ' ||
                					     	'AND COALESCE(trim(rajon), '''') = ''' || rec.rajon || ''' ' ||
                					     	'AND COALESCE(trim(ulica), '''') = ''' || rec.ulica || ''' ' ||
                					     	'AND COALESCE(trim(ndom), '''') = ''' || rec.ndom || ''' ' ||
                					     	'AND COALESCE(trim(nkor), '''') = ''' || rec.nkor || ''' ' ||
                					     	'AND COALESCE(trim(nkvar), '''') = ''' || rec.nkvar || ''' ' ||
                					     	'AND COALESCE(trim(nkvar_n), '''') = ''' || rec.nkvar_n || ''' ' INTO pkod;
                							
                				IF pkod ISNULL THEN CONTINUE;
                				END IF;
                			END IF;
                			
                			EXECUTE 'SELECT dat_month, SUM(sum_money) sum_money FROM charge' || alias || ' WHERE pkod IN (' || pkod || ') GROUP BY dat_month' INTO pay;
                			
                			IF pay NOTNULL AND COALESCE(pay.sum_money, 0) != 0 THEN
                				EXECUTE 'INSERT INTO {tmpTable2} SELECT ' || rec.outer_id || ', ''' || pay.dat_month || ''', ' || pay.sum_money;
                				EXIT;
                			END IF;
                
                		END LOOP;
                    END LOOP;
                END$$ LANGUAGE plpgsql;
                
                SELECT outer_id ""OuterId"", dat_month ""Date"", sum_money ""SumMoney"" FROM {tmpTable2};");

			IEnumerable<PayInfo> payInfoList;
			using (var sqlExecutor = new SqlExecutor.SqlExecutor(connPgmu))
			{
				payInfoList = sqlExecutor.ExecuteSql<PayInfo>(sql.ToString());
			}

			foreach (var rec in records)
			{
				var payInfo = payInfoList.FirstOrDefault(x => x.OuterId == rec.OuterId);
				if (payInfo.IsNotNull())
				{
					rec.PayDate = payInfo.Date;
					rec.PaySum = payInfo.SumMoney;
				}
			}

			var section = reportParams.ComplexReportParams.ДобавитьСекцию("DebtorData");

			foreach (var x in records)
			{
				var addressArr = new[]
				{
					x.PgmuAddress.PostCode,
					x.PgmuAddress.District,
					x.PgmuAddress.Town,
					x.PgmuAddress.Street,
					x.PgmuAddress.House,
					x.PgmuAddress.Building,
					x.PgmuAddress.Apartment,
					x.PgmuAddress.Room
				};
				
				section.ДобавитьСтроку();
				section["RowNum"] = x.OuterId;
				section["JurInstitution"] = x.JurInstitution;
				section["State"] = x.State;
				section["RegNumber"] = x.RegNumber;
				section["EntrepreneurCreateDate"] = x.EntrepreneurCreateDate;
				section["EntrepreneurDebtSum"] = x.EntrepreneurDebtSum;
				section["Debtor"] = x.Debtor;
				section["Address"] = string.Join(", ", addressArr);
				section["PayDate"] = x.PayDate;
				section["PaySum"] = x.PaySum;
			}
		}
	}

	/// <summary>
	/// Информация о платеже
	/// </summary>
	internal class PayInfo
	{
		/// <summary>
		/// Внешний Id (id ИП)
		/// </summary>
		public long OuterId { get; set; }

		/// <summary>
		/// Дата последнего платежа
		/// (в формате ПГМУ это месяц и год начисления)
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Сумма всех платежей за последнюю дату
		/// </summary>
		public decimal SumMoney { get; set; }
	}
}