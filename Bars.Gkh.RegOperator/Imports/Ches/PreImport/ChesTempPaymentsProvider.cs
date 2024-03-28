namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;

    using NHibernate.Linq;

    /// <inheritdoc />
    public class ChesTempPaymentsProvider : ChesTempDataProvider<PayFileInfo>
    {
        /// <inheritdoc />
        public ChesTempPaymentsProvider(IWindsorContainer container, PayFileInfo fileInfo, BaseParams baseParams)
            : base(container, fileInfo, baseParams)
        {
            this.PaymentDay = baseParams.Params.GetAs<int>("paymentDay");
        }

        private int CommandTimeOut = 3600 * 3;

        private readonly int PaymentDay; 

        /// <summary>
        /// Импортировать секцию
        /// </summary>
        public override void Import()
        {
            this.Container.InStatelessConnectionTransaction(
                (con, tr) =>
                {
                    this.CreateSchema(con);
                    this.CreateTable(con, false);
                    this.InStatelessImportAction(con, tr);
                    this.CheckAndFillPayments(con, tr);
                    this.UpdateSummaryView(con, tr);
                });
        }

        private void CheckAndFillPayments(IDbConnection connection, IDbTransaction transaction)
        {
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();

            try
            {
                var sql =
                    $@"SELECT id as Id, lsnum as LsNum, paymentdate as PaymentDate, tariffpayment as TariffPayment, 
                          tariffdecisionpayment as TariffDecisionPayment, penaltypayment as PenaltyPayment,
                          paymenttype as PaymentType, registrynum as RegistryNum, registrydate as RegistryDate,
                          paymentday as PaymentDay, version as Version, isvalid as IsValid, reason as Reason
                   from {this.TableName};";

                var rows = connection.Query<PayFileInfo.Row>(sql, commandTimeout: this.CommandTimeOut)
                    .ToList();

                var currentRows = rows.Where(x => x.Version == null && x.PaymentDay == null).ToList();

                var take = 10000;
                for (int skip = 0; skip < currentRows.Count; skip += take)
                {
                    var portion = currentRows.Skip(skip).Take(take);
                    var importedPayments = importedPaymentDomain.GetAll()
                        .Fetch(x => x.PersonalAccount)
                        .WhereContainsBulked(x => x.Id, portion.Select(x => x.RegistryNum.ToLong()), take)
                        .ToDictionary(x => x.Id);

                    foreach (var row in portion)
                    {
                        row.PaymentDay = this.FileInfo.PaymentDay.Value.Day;
                        row.Version = this.FileInfo.Version;

                        if (row.LsNum.IsEmpty())
                        {
                            row.Reason = "Номер лс в файле не заполнен";
                            row.IsValid = false;
                            continue;
                        }

                        if (!row.PaymentDate.IsValid())
                        {
                            row.Reason = "Дата операции в файле не заполнена";
                            row.IsValid = false;
                            continue;
                        }

                        if (row.TariffPayment == 0 && row.TariffDecisionPayment == 0 && row.PenaltyPayment == 0)
                        {
                            row.Reason = "Сумма оплаты в файле не заполнена";
                            row.IsValid = false;
                            continue;
                        }

                        if (!row.RegistryDate.IsValid())
                        {
                            row.Reason = "Дата документа/реестра в файле не заполнена";
                            row.IsValid = false;
                            continue;
                        }

                        if (row.PaymentType == ImportPaymentType.CancelPayment)
                        {
                            row.IsValid = true;
                            continue;
                        }

                        //if (row.RegistryNum == null)
                        //{
                        //    row.Reason = "Не найдена оплата в БАРС";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //var payment = importedPayments.Get(row.RegistryNum.ToLong());

                        //if (payment.IsNull())
                        //{
                        //    row.Reason = "Не найдена оплата в БАРС";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.LsNum != payment.PersonalAccount.PersonalAccountNum)
                        //{
                        //    row.Reason =
                        //        $"Номер лс в файле ({row.LsNum}) не соответствует номеру лс в БАРС ({payment.PersonalAccount.PersonalAccountNum})";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.PaymentDate != payment.PaymentDate)
                        //{
                        //    row.Reason =
                        //        $"Дата операции в файле ({row.PaymentDate.ToShortDateString()}) не соответствует дате операции в БАРС ({payment.PaymentDate.ToShortDateString()})";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if ((row.TariffPayment + row.TariffDecisionPayment != payment.Sum && payment.PaymentType == ImportedPaymentType.ChargePayment)
                        //    || row.TariffPayment + row.TariffDecisionPayment + row.PenaltyPayment != payment.Sum)
                        //{
                        //    row.Reason =
                        //        $"Сумма в файле ({row.TariffPayment + row.TariffDecisionPayment + row.PenaltyPayment}) не соответствует Сумме в БАРС ({payment.Sum})";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.PenaltyPayment != payment.Sum
                        //    && payment.PaymentType == ImportedPaymentType.Penalty)
                        //{
                        //    row.Reason = $"Сумма по пени в файле ({row.PenaltyPayment}) не соответствует Сумме по пенив БАРС ({payment.Sum})";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.PenaltyPayment == 0 && row.PaymentType == ImportPaymentType.ReturnPayment
                        //    && payment.PaymentType == ImportedPaymentType.PenaltyRefund)
                        //{
                        //    row.Reason = $"Тип оплаты в файле ({row.PaymentType.GetDisplayName()}) не соответствует Типу оплаты в БАРС {payment.PaymentType.GetDisplayName()}";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.TariffPayment + row.TariffDecisionPayment == 0 && row.PaymentType == ImportPaymentType.ReturnPayment
                        //    && payment.PaymentType == ImportedPaymentType.Refund)
                        //{
                        //    row.Reason = $"Тип оплаты в файле ({row.PaymentType.GetDisplayName()}) не соответствует Типу оплаты в БАРС {payment.PaymentType.GetDisplayName()}";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (row.PaymentType == ImportPaymentType.Payment && payment.PaymentType != ImportedPaymentType.ChargePayment
                        //    && payment.PaymentType != ImportedPaymentType.Penalty)
                        //{
                        //    row.Reason =
                        //        $"Тип оплаты в файле ({row.PaymentType.GetDisplayName()}) не соответствует Типу оплаты в БАРС {payment.PaymentType.GetDisplayName()}";
                        //    row.IsValid = false;
                        //    continue;
                        //}
                        //// пока отменяем проверку на дату докмента/реестра
                        //if (row.RegistryDate != payment.BankDocumentImport.DocumentDate)
                        //{
                        //    row.Reason =
                        //        $"Дата документа/реестра в файле ({row.RegistryDate}) не соответствует Дате документа/реестра в БАРС ({payment.BankDocumentImport.DocumentDate})";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        //if (rows.Any(x => x.RegistryNum == row.RegistryNum && x.IsValid == true))
                        //{
                        //    row.Reason = "Оплата с таким ID уже была сопоставлена";
                        //    row.IsValid = false;
                        //    continue;
                        //}

                        row.IsValid = true;
                    }

                    sql = $@"UPDATE {this.TableName} 
                                     SET 
                                        paymentday=@PaymentDay, 
                                        version = @Version,
                                        isvalid=@IsValid, 
                                        reason =@Reason
                                        WHERE id=@Id;";
                    connection.Execute(sql, portion, transaction);
                }
            }
            finally
            {
                this.Container.Release(importedPaymentDomain);
            }
        }

        protected override string GetSummaryViewCreateSql()
        {
            var summaryViewFields = this.FileInfo.GetSummaryColumns();
            var columns = string.Join(",", summaryViewFields.Select(x => $"sum({x.Formula}) \"{x.ColumnName.ToLower()}\""));
            return $"CREATE MATERIALIZED VIEW {this.SummaryViewName} AS "
                    + $"SELECT paymentday, {columns} FROM {this.TableName} "
                    + "WHERE paymentday is not null AND isvalid = true "
                    + "GROUP BY paymentday "
                    + "WITH DATA;";
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

        protected override string GetOutSqlQuery(ColumnDef[] fields)
        {
            var sqlQuery = $@"COPY (select {string.Join(",", fields.Select(x => x.Name))} 
                FROM  {this.TableName} where isvalid = true and isimported is not true
                        and paymentday = {this.PaymentDay}) 
                TO STDOUT
                WITH DELIMITER ';'
                NULL ''
                QUOTE '}}'
                HEADER 
                CSV
                encoding 'windows-1251'";
            return sqlQuery;
        }
    }
}