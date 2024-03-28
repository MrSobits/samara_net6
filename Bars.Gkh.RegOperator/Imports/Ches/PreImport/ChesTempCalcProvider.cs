namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;

    /// <inheritdoc />
    public class ChesTempCalcProvider : ChesTempDataProvider<CalcFileInfo>
    {
        /// <inheritdoc />
        public ChesTempCalcProvider(IWindsorContainer container, CalcFileInfo fileInfo, BaseParams baseParams)
            : base(container, fileInfo, baseParams)
        {
            this.CheckViewName = new SchemaQualifiedObjectName
            {
                Schema = this.TableName.Schema,
                Name = $"view_check_{this.TableName.Name}"
            };
        }

        protected override string GetCheckViewCreateSql()
        {
            return $@"CREATE MATERIALIZED VIEW {this.CheckViewName} AS
                SELECT c.id as Id, s.id as PeriodSummaryId, m.name as Municipality,
                ro.address||', кв. '||r.croom_num||CASE WHEN COALESCE(r.chamber_num, '') <> '' THEN ', ком. '||r.chamber_num ELSE '' END as Address,
                c.lsnum as LsNum, a.regop_pers_acc_extsyst as ChesLsNum, c.saldoouth as ChesSaldo,
                s.saldo_out as Saldo, (c.saldoouth - s.saldo_out) as DiffSaldo, c.isimported as IsImported
                FROM {this.TableName} c
                JOIN REGOP_PERS_ACC a ON a.acc_num = c.lsnum
                JOIN REGOP_PERS_ACC_PERIOD_SUMM s ON s.account_id = a.id AND s.period_id = {this.Period.Id}
                JOIN GKH_ROOM r ON r.id = a.room_id
                JOIN GKH_REALITY_OBJECT ro ON ro.id = r.ro_id
                JOIN GKH_DICT_MUNICIPALITY m ON m.Id = ro.municipality_id
                WHERE c.saldoouth - s.saldo_out <> 0 OR c.isimported = true
                WITH DATA";
        }

        public override IDictionary<string, object> GetSummaryData(BaseParams baseParams)
        {
            IDictionary<string, object> resultData = null;
            var columns = this.FileInfo.GetSummaryColumns();
            var paymentDay = baseParams.Params.GetAs<int>("paymentDay");

            this.Container.InStatelessConnectionTransaction((connection, transaction) =>
            {
                var transformProvider = MigratorUtils.GetTransformProvider(connection);
                if (transformProvider.MaterializedViewExists(this.SummaryViewName))
                {
                    var sql = string.Empty;
                    if (paymentDay > 0)
                    {
                        sql = $"SELECT paymentday, {string.Join(",", columns.Select(x => $"{x.ColumnName} AS \"{x.PropertyName}\""))} "
                            + $"FROM {this.SummaryViewName} WHERE paymentday = {paymentDay}";
                    }
                    else
                    {
                        sql = $"SELECT {string.Join(",", columns.Select(x => $"sum({x.ColumnName}) AS \"{x.PropertyName}\""))} "
                            + $"FROM {this.SummaryViewName}";
                    }

                    resultData = connection
                        .Query(sql)
                        .First() as IDictionary<string, object>;
                }
                else
                {
                    resultData = new Dictionary<string, object>();
                }

            });

            return resultData;
        }
    }
}