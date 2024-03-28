namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Domain.Repository.RealityObjectAccount;
    using Gkh.Domain;
    using NHibernate;
    using NHibernate.Transform;

    public class CalcAccountMoneyService : ICalcAccountMoneyService
    {
        private readonly ISessionProvider sessionProvider;

        public CalcAccountMoneyService(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public ListDataResult List(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accId");
            var isCredit = baseParams.Params.GetAs<bool>("isCredit");
            var loadParams = baseParams.GetLoadParam();
            
            var list = this.GetQuery(accountId, isCredit, TypeOperation.List, loadParams)
                .SetResultTransformer(Transformers.AliasToBean<TransferDto>())
                .List<TransferDto>();

            var count = this.GetQuery(accountId, isCredit, TypeOperation.Count, loadParams)
                .List()[0]
                .ToInt();

            return new ListDataResult(list, count);
        }

        public DateTime GetLastOperationDate(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accId");
            var loadParams = baseParams.GetLoadParam();

            var date = this.GetQuery(accountId)
                .List()[0]
                .ToDateTime();

            return date;
        }

        private IQuery GetQuery(long accountId, bool isCredit, TypeOperation typeOperation, LoadParam loadParam)
        {
            IQuery query = null;

            var session = this.sessionProvider.GetCurrentSession();

            switch (typeOperation)
            {
                case TypeOperation.List:
                    query = session.CreateSQLQuery(this.listQuery.FormatUsing(
                            this.AggregateQuery(isCredit),
                            this.GetFilter(loadParam),
                            this.GetOrder(loadParam),
                            this.GetLimit(loadParam)))
                        .SetInt64("accountId", accountId);
                    break;
                case TypeOperation.Count:
                    query = session.CreateSQLQuery(this.countQuery.FormatUsing(
                            this.AggregateQuery(isCredit), 
                            this.GetFilter(loadParam)))
                        .SetInt64("accountId", accountId);
                    break;
            }

            return query;
        }

        private IQuery GetQuery(long accountId)
        {
            IQuery query = null;

            var session = this.sessionProvider.GetCurrentSession();
            
            query = session.CreateSQLQuery(this.maxQuery)
                            .SetInt64("accountId", accountId);    
           
            return query;
        }

        private string AggregateQuery(bool isCredit)
        {
            var sql = new StringBuilder();

            foreach (var walletName in this.walletNames)
            {
                if (sql.Length > 0)
                {
                    sql.AppendLine().Append(@"union all").AppendLine();
                }

                sql.Append(isCredit
                    ? this.GetListCreditUnion(walletName)
                    : this.GetListDebetUnion(walletName));
                
                sql.AppendLine();
            }

            return sql.ToString();
        }

        private string GetListDebetUnion(string walletName)
        {
            return @"
    select 
        t.id as ""Id"", 
        t.amount as ""Amount"", 
        coalesce(coalesce(t.reason, mo.reason), orig.reason) as ""Reason"", 
        t.operation_date as ""OperationDate""
    from regop_transfer t 
        join regop_money_operation mo on mo.id = t.op_id 
        join regop_wallet w on w.wallet_guid = t.target_guid 
        join regop_ro_payment_account rpa on rpa.{0} = w.id 
        join regop_calc_acc_ro caro on caro.ro_id = rpa.ro_id
        left join regop_transfer orig on orig.id = t.originator_id
    where mo.canceled_op_id is null and not t.is_loan and not t.is_return_loan and caro.account_id = :accountId

    union all

    select 
        t.id as ""Id"", 
        (-1 * t.amount) as ""Amount"", 
        coalesce(coalesce(t.reason, mo.reason), orig.reason) as ""Reason"", 
        t.operation_date as ""OperationDate""
    from regop_transfer t 
        join regop_money_operation mo on mo.id = t.op_id 
        join regop_wallet w on w.wallet_guid = t.source_guid 
        join regop_ro_payment_account rpa on rpa.{0} = w.id 
        join regop_calc_acc_ro caro on caro.ro_id = rpa.ro_id
        left join regop_transfer orig on orig.id = t.originator_id
    where mo.canceled_op_id is not null and not t.is_loan and not t.is_return_loan and caro.account_id = :accountId"
                .FormatUsing(walletName);
        }

        private string GetListCreditUnion(string walletName)
        {
            return @"
    select
        t.id as ""Id"", 
        t.amount as ""Amount"", 
        coalesce(coalesce(t.reason, mo.reason), orig.reason) as ""Reason"", 
        t.operation_date as ""OperationDate""
    from regop_transfer t
        join regop_money_operation mo on mo.id = t.op_id
        join regop_wallet w on w.wallet_guid = t.source_guid
        join regop_ro_payment_account rpa on rpa.{0} = w.id
        join regop_calc_acc_ro caro on caro.ro_id = rpa.ro_id
        left join regop_transfer orig on orig.id = t.originator_id
    where mo.canceled_op_id is null and not t.is_loan and not t.is_return_loan and caro.account_id = :accountId

    union all

    select
        t.id as ""Id"", 
        t.amount as ""Amount"", 
        coalesce(coalesce(t.reason, mo.reason), orig.reason) as ""Reason"", 
        t.operation_date as ""OperationDate""
    from regop_transfer t
        join regop_money_operation mo on mo.id = t.op_id
        join regop_wallet w on w.wallet_guid = t.target_guid
        join regop_ro_payment_account rpa on rpa.{0} = w.id
        join regop_calc_acc_ro caro on caro.ro_id = rpa.ro_id
        left join regop_transfer orig on orig.id = t.originator_id
    where mo.canceled_op_id is not null and not t.is_loan and not t.is_return_loan and caro.account_id = :accountId"
                .FormatUsing(walletName);
        }

        private string GetLimit(LoadParam loadParam)
        {
            if (loadParam == null)
            {
                return null;
            }

            return string.Format("limit {0} offset {1}", loadParam.Limit, loadParam.Start);
        }

        private string GetOrder(LoadParam loadParam)
        {
            if (loadParam == null || loadParam.Order.Length == 0)
            {
                return null;
            }

            var parameter = loadParam.Order.First();

            return string.Format(" order by ttt.{0} {1}", parameter.Name.InQuots(), parameter.Asc ? "asc" : "desc");
        }

        private string GetFilter(LoadParam loadParam)
        {
            if (loadParam == null || loadParam.ComplexFilter == null)
            {
                return null;
            }

            var filterString = new StringBuilder();

            this.GetFilterRecursive(filterString, loadParam.ComplexFilter);

            filterString.Insert(0, " where ");

            return filterString.ToString();
        }

        private void GetFilterRecursive(StringBuilder sb, ComplexFilter complexFilter)
        {
            if (complexFilter.Left != null)
            {
                this.GetFilterRecursive(sb, complexFilter.Left);
            }

            if (complexFilter.Operator == ComplexFilterOperator.and)
            {
                sb.Append(" and ");
            }

            if (complexFilter.Right != null)
            {
                this.GetFilterRecursive(sb, complexFilter.Right);
            }

            if (complexFilter.Operator == ComplexFilterOperator.eq)
            {
                if (complexFilter.Value is DateTime)
                {
                    sb.AppendFormat(" date(ttt.{0}) = to_date('{1}', 'dd.mm.yyyy')",
                        complexFilter.Field.InQuots(),
                        complexFilter.Value.ToDateTime().ToShortDateString());
                }
                else
                {
                    var value = complexFilter.Value.ToString();
                    decimal d;
                    if (decimal.TryParse(value, out d))
                    {
                        value = value.Replace(",", ".");
                    }

                    sb.AppendFormat(" ttt.{0} = {1}", complexFilter.Field.InQuots(), value);
                }
            }
            else if (complexFilter.Operator == ComplexFilterOperator.icontains)
            {
                sb.AppendFormat(" lower(ttt.{0}) like '%{1}%'",
                    complexFilter.Field.InQuots(),
                    complexFilter.Value.ToStr().ToLower());
            }
        }

        private enum TypeOperation
        {
            List,
            Count,
            Max
        }

        #region Queries

        private readonly string listQuery = @"
select ttt.""Id"", ttt.""Amount"", ttt.""Reason"", ttt.""OperationDate"" 
from ({0}) ttt
{1}
{2}
{3}";

        private readonly string countQuery = @"
select count(*) 
from ({0}) ttt
{1}";

        private readonly string maxQuery = @"
select max(t.operation_date)
    from regop_transfer t
        join regop_money_operation mo on mo.id = t.op_id
        join regop_wallet w on 
        w.wallet_guid = t.target_guid
        join regop_ro_payment_account rpa 
        on 
        rpa.bp_wallet_id= w.id or
        rpa.fsu_wallet_id= w.id or
        rpa.os_wallet_id= w.id or
        rpa.rsu_wallet_id= w.id or
        rpa.ssu_wallet_id= w.id or
        rpa.tsu_wallet_id= w.id or
        rpa.af_wallet_id= w.id or
        rpa.pwp_wallet_id= w.id or
        rpa.r_wallet_id= w.id or
        rpa.ss_wallet_id= w.id or
        rpa.bt_wallet_id= w.id or
        rpa.dt_wallet_id= w.id or
        rpa.p_wallet_id= w.id
join regop_calc_acc_ro caro on caro.ro_id = rpa.ro_id
        left join regop_transfer orig on orig.id = t.originator_id
    where mo.canceled_op_id is not null and not t.is_loan and not t.is_return_loan and caro.account_id = :accountId";

        #endregion Queries

        private readonly string[] walletNames =
        {
            "AF_WALLET_ID",
            "BP_WALLET_ID",
            "BT_WALLET_ID",
            "DT_WALLET_ID",
            "FSU_WALLET_ID",
            "OS_WALLET_ID",
            "P_WALLET_ID",
            "PWP_WALLET_ID",
            "RSU_WALLET_ID",
            "R_WALLET_ID",
            "SS_WALLET_ID",
            "SSU_WALLET_ID",
            "TSU_WALLET_ID"
        };
    }

    public static class StringExtensions
    {
        public static string InQuots(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (!value.StartsWith("\""))
            {
                value = "\"" + value;
            }

            if (!value.EndsWith("\""))
            {
                value += "\"";
            }

            return value;
        }
    }
}