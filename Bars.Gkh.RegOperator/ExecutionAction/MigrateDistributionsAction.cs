namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    using NHibernate.Transform;

    /// <summary>
    /// MigrateDistributionsAction
    /// </summary>
    public class MigrateDistributionsAction : BaseExecutionAction
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public static string Code = "MigrateDistributionsAction";

        private Dictionary<string, DistributionCode> codeDictionary;
        private Dictionary<string, List<MoneyOperation>> operationsGrouped;

        private Dictionary<string, Tuple<long, string>> bankAccountStatementByTransferGuid;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description { get; } =
            "Создание связей для банковских распределений. Действие выполняет создание в базе данных для корректной отмены распределения";

        /// <summary>
        /// Имя действия
        /// </summary>
        public override string Name { get; } = "РегОператор - Создание связей для банковских распределений";

        /// <summary>
        /// Action
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Container
        /// </summary>
        /// <summary>
        /// Интерфейс работы с менеджером логов
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Банковская выписка
        /// </summary>
        public IDomainService<BankAccountStatement> BankAccountStatementDomain { get; set; }

        /// <summary>
        /// Трансфер между источником и получателем денег
        /// </summary>
        public IDomainService<Transfer> TransferDomain { get; set; }

        /// <summary>
        /// Денежные операции
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Универсальный классификатор
        /// </summary>
        public IDomainService<MultipurposeGlossary> MultipurposeGlossaryDomain { get; set; }

        /// <summary>
        /// Сущность связи Кода банковского распределения и распределения
        /// </summary>
        public IDomainService<DistributionOperation> DistributionOperationDomain { get; set; }

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            this.InitCache();

            var listToSave = new List<PersistentObject>();

            foreach (var operationsByBankAccountStatement in this.operationsGrouped)
            {
                this.ProcessBankAccountStatement(operationsByBankAccountStatement, listToSave);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, listToSave, useStatelessSession: true);

            this.ValidateStamenets();

            return new BaseDataResult();
        }

        private void InitCache()
        {
            this.InitCodes();

            this.bankAccountStatementByTransferGuid = this.BankAccountStatementDomain.GetAll()
                .Select(
                    x => new
                    {
                        x.TransferGuid,
                        x.DistributionCode,
                        x.Id
                    })
                .AsEnumerable()
                .ToDictionary(x => x.TransferGuid, x => new Tuple<long, string>(x.Id, x.DistributionCode));

            // нам нужны только прямые операции, не отмены
            var notCancelOperationsQuery = this.MoneyOperationDomain.GetAll().Where(x => x.CanceledOperation == null);

            // получаем все операции связанные с БО
            this.operationsGrouped = notCancelOperationsQuery
                .Where(x => this.BankAccountStatementDomain.GetAll().Any(y => y.TransferGuid == x.OriginatorGuid))
                .Where(x => !this.DistributionOperationDomain.GetAll().Any(y => y.Operation.Id == x.Id))
                .AsEnumerable()
                .GroupBy(x => x.OriginatorGuid)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        private void InitCodes()
        {
            this.codeDictionary = Enum.GetValues(typeof(DistributionCode))
                .Cast<DistributionCode>()
                .Select(
                    x => new
                    {
                        Key = x.GetDisplayName(),
                        Value = x
                    })
                .ToDictionary(x => x.Key, x => x.Value);

            var glossary = this.MultipurposeGlossaryDomain.GetAll()
                .FirstOrDefault(x => x.Code == DistributionNameLocalizer.DistributionNameGlossaryCode);

            if (glossary.IsNotNull())
            {
                foreach (var value in glossary.Items)
                {
                    DistributionCode code;

                    if (Enum.TryParse(value.Key, out code))
                    {
                        this.codeDictionary[value.Value] = code;
                    }
                }
            }

            this.codeDictionary["Оплата по базовому тарифу"] = DistributionCode.TransferCrDistribution;
            this.codeDictionary["Оплата пени"] = DistributionCode.TransferCrDistribution;
            this.codeDictionary["Оплата по тарифу решения"] = DistributionCode.TransferCrDistribution;
            this.codeDictionary["Поступление за проделанные работы"] = DistributionCode.PreviousWorkPaymentDistribution;
            this.codeDictionary["Оплата акта выполненных работ"] = DistributionCode.PerformedWorkActsDistribution;
            this.codeDictionary["Оплата заявки на перечисление средств подрядчику"] = DistributionCode.TransferContractorDistribution;
            this.codeDictionary["Поступленее ранее накопленных средств"] = DistributionCode.AccumulatedFundsDistribution;
            this.codeDictionary["Поступление ранее накопленных средств"] = DistributionCode.AccumulatedFundsDistribution;
            this.codeDictionary["Поступление оплаты аренды"] = DistributionCode.RentPaymentDistribution;

            this.codeDictionary["Зачисление по базовому тарифу в счет отмены возврата средств"] = DistributionCode.RefundDistribution;
            this.codeDictionary["Зачисление по тарифу решения в счет отмены возврата средств"] = DistributionCode.RefundDistribution;
            this.codeDictionary["Зачисление по пеням в счет отмены возврата"] = DistributionCode.RefundDistribution;

            this.codeDictionary["Возврат пени"] = DistributionCode.RefundPenaltyDistribution;

            this.codeDictionary["Возврат взносов на КР по тарифу решения"] = DistributionCode.RefundDistribution;
            this.codeDictionary["Возврат взносов на КР по базовому тарифу"] = DistributionCode.RefundDistribution;
            this.codeDictionary["Возврат средств по базовому тарифу"] = DistributionCode.RefundDistribution;
            this.codeDictionary["Возврат средств по тарифу решения"] = DistributionCode.RefundDistribution;
        }

        private void ValidateStamenets()
        {
            var currentSession = this.SessionProvider.GetCurrentSession();

            var invalidStatments = currentSession.CreateSQLQuery(@"SELECT
                distinct on (bo.id) 
                bo.id AS Id,
                bo.doc_num AS Num,
                bo.doc_sum AS Sum,
                bo.doc_date AS DocDate,
                bo.op_date AS OpDate
                FROM REGOP_BANK_ACC_STMNT bo
                JOIN regop_money_operation op on op.originator_guid = bo.transfer_guid and op.canceled_op_id is null
                left join REGOP_BANK_STMNT_OP stop on stop.BANK_STMNT_ID = bo.id and stop.op_id = op.id
                where bo.state <> 40 and stop.id is null")
                .SetTimeout(900000)
                .SetResultTransformer(Transformers.AliasToBean<QueryDto>())
                .List<QueryDto>();

            foreach (var invalidStatment in invalidStatments)
            {
                this.Logger.LogError(
                    $"По указанной распределенной банковской операции с id {invalidStatment.Id} отсутствуют операции движения денег " +
                        $"[{invalidStatment.Num};{invalidStatment.DocDate};{invalidStatment.OpDate};{invalidStatment.Sum}]");
            }
        }

        private void ProcessBankAccountStatement(
            KeyValuePair<string, List<MoneyOperation>> operationsByBankAccountStatement,
            List<PersistentObject> listToSave)
        {
            var operationIds = operationsByBankAccountStatement.Value.Select(x => x.Id).ToArray();

            // берем только те трансферы, по которым нету сущности DistributionOperation
            var transferDict = this.TransferDomain.GetAll()
                .Where(x => operationIds.Contains(x.Operation.Id))
                .AsEnumerable()
                .GroupBy(x => x.Operation.Id)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            foreach (var moneyOperation in operationsByBankAccountStatement.Value)
            {
                var transfer = transferDict.Get(moneyOperation.Id);

                if (transfer.IsNotNull())
                {
                    var distributionCode = this.GetCode(transfer);
                    var statmentId = this.bankAccountStatementByTransferGuid[operationsByBankAccountStatement.Key].Item1;

                    if (distributionCode > 0)
                    {
                        listToSave.Add(this.CreateTransferDistrCode(statmentId, distributionCode, moneyOperation));
                    }
                    else
                    {
                        this.Logger.LogError(
                            "Ошибка определения кода распределения для банковской операции с id " +
                                $"{statmentId}, наименование операции: {transfer.Reason}, {transfer.Operation.Reason}," +
                                $"id операции: {moneyOperation.Id}");
                    }
                }
            }
        }

        private DistributionCode GetCode(Transfer transfer)
        {
            DistributionCode result;

            if (transfer.Reason.IsNotEmpty() && this.codeDictionary.TryGetValue(transfer.Reason, out result))
            {
                return result;
            }

            if (transfer.Operation.Reason.IsNotEmpty())
            {
                return this.codeDictionary.Get(transfer.Operation.Reason);
            }

            return 0;
        }

        private DistributionOperation CreateTransferDistrCode(long statementId, DistributionCode distrCode, MoneyOperation operation)
        {
            return new DistributionOperation
            {
                BankAccountStatement = new BankAccountStatement {Id = statementId},
                Code = distrCode,
                Operation = operation
            };
        }

        private struct QueryDto
        {
            public long Id { get; set; }

            public decimal Sum { get; set; }

            public string Num { get; set; }

            public DateTime? DocDate { get; set; }

            public DateTime? OpDate { get; set; }
        }
    }
}