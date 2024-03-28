namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    using NHibernate;
    using NHibernate.Linq;
    using NHibernate.Persister.Entity;

    public class RestoreUnacceptedPaymentAction : BaseExecutionAction
    {
        public static string Code = "RestoreUnacceptedPaymentAction";

        private readonly int _take = 100;

        private HashSet<long> _existsUnacceptedPayments = new HashSet<long>();

        private int _totalCount;

        public override string Description => "Восстановление непотвержденных оплат(Камчатка)";

        public override string Name => "Восстановление непотвержденных оплат(Камчатка)";

        public override Func<IDataResult> Action => this.Execute;

        public ILogger Logger { get; set; }

        private BaseDataResult NewExecute()
        {
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            /*session.CreateSQLQuery(@"truncate regop_unaccept_pay_packet cascade;").ExecuteUpdate();*/

            session.CreateSQLQuery(@"
                insert into regop_unaccept_pay_packet
                (object_version	, object_create_date, object_edit_date  , create_date				        , description	, state	, packet_sum	, bank_doc_id)
                select 
                0		        , now()			    ,now()			    , coalesce(bdi.document_date, now()), null		    , 20	, 0		        , bdi.id
                from regop_bank_doc_import bdi
                where not exists (select 1 from regop_unaccept_pay_packet p where p.bank_doc_id = bdi.id);")
                .ExecuteUpdate();

            var accountRepo = this.Container.ResolveRepository<BasePersonalAccount>();

            var unacceptedPaymentRepo = this.Container.ResolveRepository<UnacceptedPayment>();

            this._existsUnacceptedPayments =
                unacceptedPaymentRepo.GetAll()
                    .Select(x => x.Id)
                    .ToHashSet();

            var query = accountRepo.GetAll().OrderBy(x => x.Id);

            var count = query.Count();

            for (var skip = 0; skip < count; skip += this._take)
            {
                this.ProcessPortion(query, skip);

                session.Clear();
            }

            for (var skip = 0; skip < count; skip += this._take)
            {
                this.ProcessPortionSinglePayment(query, skip);

                session.Clear();
            }

            for (var skip = 0; skip < count; skip += this._take)
            {
                this.ProcessPortionDateNextMonth(query, skip);

                session.Clear();
            }

            /*session.CreateSQLQuery(
                @"delete from regop_unaccept_pay_packet upp where not exists (select 1 from regop_unaccept_pay up where up.packet_id = upp.id)")
                .ExecuteUpdate();*/

            return new BaseDataResult(this._totalCount);
        }

        private void ProcessPortion(IQueryable<BasePersonalAccount> query, int skip)
        {
            var transferRepo = this.Container.ResolveRepository<Transfer>();
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var unacceptedPaymentRepo = this.Container.ResolveRepository<UnacceptedPayment>();
            var unacceptedPacketRepo = this.Container.ResolveRepository<UnacceptedPaymentPacket>();

            var accounts = query.Skip(skip).Take(this._take).ToArray();

            var accountNumbers = accounts.Select(x => x.PersonalAccountNum).ToArray();

            var walletGuids = accounts.SelectMany(x => x.GetMainWalletGuids()).ToArray();

            var transferQuery = transferRepo.GetAll()
                .Where(x => walletGuids.Contains(x.TargetGuid));

            var unaccPayments = unacceptedPaymentRepo.GetAll()
                .Where(z => transferQuery.Any(x => x.SourceGuid == z.Guid))
                .ToDictionary(x => x.Guid);

            var paymentIds = unaccPayments.Select(x => x.Value.Id).ToArray();

            var importedPayments = importedPaymentRepo.GetAll()
                .Where(x => x.PaymentState == ImportedPaymentState.Rno)
                .Where(x => accountNumbers.Contains(x.Account))
                .Where(x => x.Account != null)
                .Where(x => !paymentIds.Contains(x.PaymentId.Value))
                .AsEnumerable()
                .GroupBy(this.GetKey)
                .ToDictionary(x => x.Key, z => z.ToList());

            var packetsByBankId = unacceptedPacketRepo.GetAll()
                .Where(x => x.BankDocumentId.HasValue)
                .AsEnumerable()
                .GroupBy(x => x.BankDocumentId)
                .ToDictionary(x => x.Key ?? 0L, z => z.First());

            var transfersByAccount = this.AggregateTransfersByAccount(accounts, transferQuery.ToArray());

            var itemsToCreate = new List<UnacceptedPayment>();

            foreach (var account in accounts)
            {
                if (!transfersByAccount.ContainsKey(account.PersonalAccountNum))
                {
                    continue;
                }

                var transfers = transfersByAccount[account.PersonalAccountNum];

                BasePersonalAccount account1 = account;
                foreach (var group in transfers
                    .Where(x => !unaccPayments.ContainsKey(x.SourceGuid))
                    .GroupBy(x => new {x.Operation.Id, PaymentType = this.GetPaymentType(account1, x)}))
                {
                    var transfer = group.First();
                    var amount = group.Sum(x => x.Amount);

                    var key = this.GetKey(account, transfer.PaymentDate, amount);

                    if (!importedPayments.ContainsKey(key))
                    {
                        continue;
                    }

                    var importedPayment = importedPayments[key]
                        .FirstOrDefault(x => x.Account == account1.PersonalAccountNum);

                    if (importedPayment == null)
                    {
                        continue;
                    }

                    if (this._existsUnacceptedPayments.Contains(importedPayment.PaymentId ?? 0L))
                    {
                        this._totalCount++;
                        continue;
                    }

                    this._existsUnacceptedPayments.Add(importedPayment.PaymentId ?? 0L);

                    var bankId = importedPayment.BankDocumentImport.Id;

                    if (!packetsByBankId.ContainsKey(bankId))
                    {
                        continue;
                    }

                    var paymentType = this.GetPaymentType(account, transfer);

                    if (paymentType == 0)
                    {
                        continue;
                    }

                    var unacceptedPayment = new UnacceptedPayment
                    {
                        Id = importedPayment.PaymentId ?? 0L,
                        Packet = packetsByBankId[bankId],
                        Accepted = true,
                        PaymentDate = importedPayment.PaymentDate,
                        PaymentType = paymentType,
                        PersonalAccount = account
                    };

                    unacceptedPayment.SetGuid(transfer.SourceGuid);

                    if (paymentType == PaymentType.Basic)
                    {
                        unacceptedPayment.Sum = amount;
                    }
                    else
                    {
                        unacceptedPayment.PenaltySum = amount;
                    }

                    unaccPayments[transfer.SourceGuid] = unacceptedPayment;

                    importedPayments[key].Remove(importedPayment);

                    if (importedPayments[key].Count == 0)
                    {
                        importedPayments.Remove(key);
                    }

                    itemsToCreate.Add(unacceptedPayment);
                }
            }

            this.NativeInsert(itemsToCreate);
        }

        private void NativeInsert(IEnumerable<UnacceptedPayment> items)
        {
            var query = @"insert into {0} ({1}) values({2});";

            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            var meta = session.SessionFactory.GetClassMetadata(typeof(UnacceptedPayment)) as AbstractEntityPersister;

            if (meta == null)
            {
                return;
            }

            var sb = new StringBuilder();

            var props = typeof(UnacceptedPayment).GetProperties();

            var colValues = new Dictionary<string, string>();

            foreach (var item in items)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == "Comment")
                    {
                        continue;
                    }

                    string columnName;
                    try
                    {
                        columnName = meta.GetPropertyColumnNames(prop.Name).First();
                    }
                    catch (HibernateException)
                    {
                        continue;
                    }

                    var value = this.GetValue(prop, item);

                    colValues.Add(columnName, value);
                }

                sb.AppendFormat(
                    query,
                    meta.TableName,
                    string.Join(",", colValues.Keys),
                    string.Join(",", colValues.Values),
                    item.Id);

                colValues = new Dictionary<string, string>();
            }

            if (sb.Length == 0)
            {
                return;
            }

            session.CreateSQLQuery(sb.ToString()).ExecuteUpdate();
        }

        private string GetValue<T>(PropertyInfo prop, T item)
        {
            var value = prop.GetValue(item, new object[0]);

            var propType = Nullable.GetUnderlyingType(prop.PropertyType);
            var isNullable = true;

            if (propType == null)
            {
                isNullable = false;
                propType = prop.PropertyType;
            }

            if (propType == typeof(DateTime))
            {
                return string.Format("to_date('{0}', '{1}')", value.ToDateTime(), "DD.MM.YYYY HH24.MI.SS");
            }
            if (propType == typeof(decimal))
            {
                return value.ToStr().IsEmpty()
                    ? "null"
                    : value.ToStr().Replace(",", ".");
            }
            if (propType == typeof(string))
            {
                if (value != null)
                {
                    return string.Format("'{0}'", value.ToStr());
                }

                return "null";
            }
            if (typeof(IEntity).IsAssignableFrom(propType))
            {
                var ientity = value as IEntity;
                if (ientity != null && ientity.Id.ToLong() > 0)
                {
                    return ientity.Id.ToStr();
                }

                return "null";
            }
            if (propType.IsEnum)
            {
                return ((int) value).ToStr();
            }

            return value != null ? value.ToStr() : isNullable ? "null" : "''";
        }

        private PaymentType GetPaymentType(BasePersonalAccount account, Transfer transfer)
        {
            if (account.BaseTariffWallet.WalletGuid == transfer.TargetGuid)
            {
                return PaymentType.Basic;
            }

            if (account.DecisionTariffWallet.WalletGuid == transfer.TargetGuid)
            {
                return PaymentType.Basic;
            }

            if (account.PenaltyWallet.WalletGuid == transfer.TargetGuid)
            {
                return PaymentType.Penalty;
            }

            return 0;
        }

        private string GetKey(ImportedPayment payment)
        {
            return string.Format("{0}#{1}#{2}", payment.Account.Trim(), payment.PaymentDate.ToShortDateString(), payment.Sum.RegopRoundDecimal(2));
        }

        private string GetKey(BasePersonalAccount account, DateTime paymentDate, decimal amount)
        {
            return string.Format("{0}#{1}#{2}", account.PersonalAccountNum.Trim(), paymentDate.ToShortDateString(), amount.RegopRoundDecimal(2));
        }

        private Dictionary<string, List<Transfer>> AggregateTransfersByAccount(BasePersonalAccount[] accounts, Transfer[] transfers)
        {
            var result = new Dictionary<string, List<Transfer>>();

            var byBaseTariffWallet = accounts.ToDictionary(x => x.BaseTariffWallet.WalletGuid, z => z.PersonalAccountNum);
            var byDecisionTariffWallet = accounts.ToDictionary(x => x.DecisionTariffWallet.WalletGuid, z => z.PersonalAccountNum);
            var byPenaltyWallet = accounts.ToDictionary(x => x.PenaltyWallet.WalletGuid, z => z.PersonalAccountNum);

            foreach (var transfer in transfers)
            {
                var accountNumber =
                    byBaseTariffWallet.Get(transfer.TargetGuid)
                        ?? byDecisionTariffWallet.Get(transfer.TargetGuid)
                            ?? byPenaltyWallet.Get(transfer.TargetGuid);

                if (accountNumber.IsEmpty())
                {
                    continue;
                }

                if (!result.ContainsKey(accountNumber))
                {
                    result.Add(accountNumber, new List<Transfer>());
                }

                result[accountNumber].Add(transfer);
            }

            return result;
        }

        private void ProcessPackets()
        {
            var transferRepo = this.Container.ResolveRepository<Transfer>();
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var unacceptedPaymentRepo = this.Container.ResolveRepository<UnacceptedPayment>();
            var unacceptedPacketRepo = this.Container.ResolveRepository<UnacceptedPaymentPacket>();

            var unaccPayments = unacceptedPaymentRepo.GetAll();

            var transfers = transferRepo.GetAll()
                .Where(z => !unaccPayments.Any(x => x.TransferGuid == z.SourceGuid))
                .AsEnumerable()
                .GroupBy(x => x.SourceGuid)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var imports = importedPaymentRepo.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.BankDocumentImport.Id)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var accountWallets = this.GetAccountWallets();

            foreach (var transfer in transfers)
            {
                foreach (var import in imports)
                {
                    this.CompareRecords(transfer.Value, import.Value, accountWallets);
                }
            }
        }

        private bool CompareRecords(Transfer[] transfers, ImportedPayment[] payments, Dictionary<string, string> accountByWalletGuid)
        {
            var groupped = payments.GroupBy(this.GetSimpleKey).ToDictionary(x => x.Key, z => z.ToList());

            foreach (var transfer in transfers)
            {
                var accNum = accountByWalletGuid.Get(transfer.TargetGuid);

                var key = this.GetSimpleKey(accNum, transfer.Amount);

                if (!groupped.ContainsKey(key))
                {
                    return false;
                }

                var date = transfer.PaymentDate.Month == 12
                    ? new DateTime(transfer.PaymentDate.Year + 1, 1, 1)
                    : new DateTime(transfer.PaymentDate.Year, transfer.PaymentDate.Month + 1, 1);
            }

            return true;
        }

        private Dictionary<string, string> GetAccountWallets()
        {
            var accountREpo = this.Container.ResolveRepository<BasePersonalAccount>();

            var accounts = accountREpo.GetAll()
                .Select(
                    x => new
                    {
                        x.PersonalAccountNum,
                        BT = x.BaseTariffWallet.WalletGuid,
                        DT = x.DecisionTariffWallet.WalletGuid,
                        P = x.PenaltyWallet.WalletGuid
                    })
                .ToArray();

            var result = new Dictionary<string, string>();

            foreach (var account in accounts)
            {
                result[account.BT] = account.PersonalAccountNum;
                result[account.DT] = account.PersonalAccountNum;
                result[account.P] = account.PersonalAccountNum;
            }

            return result;
        }

        private void ProcessPortionSinglePayment(IQueryable<BasePersonalAccount> query, int skip)
        {
            var accounts = query.Skip(skip).Take(this._take).ToArray();

            var transferRepo = this.Container.ResolveRepository<Transfer>();
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var unacceptedPaymentRepo = this.Container.ResolveRepository<UnacceptedPayment>();
            var unacceptedPacketRepo = this.Container.ResolveRepository<UnacceptedPaymentPacket>();

            var accountIds = accounts.Select(x => x.Id).ToArray();
            var walletGuids = accounts.SelectMany(x => x.GetMainWalletGuids()).ToArray();
            var accountNums = accounts.Select(x => x.PersonalAccountNum).ToArray();

            var packetsByBankId = unacceptedPacketRepo.GetAll()
                .Where(x => x.BankDocumentId.HasValue)
                .AsEnumerable()
                .ToDictionary(x => x.BankDocumentId ?? 0L);

            var unaccPaymentQuery = unacceptedPaymentRepo.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id));

            var transferQuery = transferRepo.GetAll()
                .Where(x => walletGuids.Contains(x.TargetGuid))
                .Where(z => !unaccPaymentQuery.Any(x => x.TransferGuid == z.SourceGuid));

            var paymentIds = unaccPaymentQuery.Select(x => x.Id).ToArray();

            var importedPayments = importedPaymentRepo.GetAll()
                .Where(x => !paymentIds.Contains(x.PaymentId.Value))
                .Where(x => accountNums.Contains(x.Account))
                .Where(x => x.Account != null)
                .AsEnumerable()
                .GroupBy(this.GetSimpleKey)
                .Select(
                    x => new
                    {
                        x.Key,
                        Count = x.Count(),
                        First = x.First()
                    })
                .Where(x => x.Count == 1)
                .ToDictionary(x => x.Key, z => z.First);

            var transfersByAccount = this.AggregateTransfersByAccount(accounts, transferQuery.ToArray());

            var itemsToCreate = new List<UnacceptedPayment>();

            foreach (var account in accounts)
            {
                if (!transfersByAccount.ContainsKey(account.PersonalAccountNum))
                {
                    continue;
                }

                var transfers = transfersByAccount[account.PersonalAccountNum];

                BasePersonalAccount account1 = account;
                var singletonTransfers = transfers
                    .GroupBy(x => this.GetSimpleKey(account1, x.Amount))
                    .Select(
                        x => new
                        {
                            x.Key,
                            Count = x.Count(),
                            First = x.First()
                        })
                    .Where(x => x.Count == 1)
                    .ToArray();

                foreach (var transfer in singletonTransfers)
                {
                    if (!importedPayments.ContainsKey(transfer.Key))
                    {
                        continue;
                    }

                    var importedPayment = importedPayments[transfer.Key];

                    if (importedPayment.Account != account1.PersonalAccountNum)
                    {
                        continue;
                    }

                    var paymentType = this.GetPaymentType(account, transfer.First);

                    if (paymentType == 0)
                    {
                        continue;
                    }

                    var unacceptedPayment = new UnacceptedPayment
                    {
                        Id = importedPayment.PaymentId ?? 0L,
                        Packet = packetsByBankId[importedPayment.BankDocumentImport.Id],
                        Accepted = true,
                        PaymentDate = importedPayment.PaymentDate,
                        PaymentType = paymentType,
                        PersonalAccount = account
                    };

                    unacceptedPayment.SetGuid(transfer.First.SourceGuid);

                    if (paymentType == PaymentType.Basic)
                    {
                        unacceptedPayment.Sum = transfer.First.Amount;
                    }
                    else
                    {
                        unacceptedPayment.PenaltySum = transfer.First.Amount;
                    }

                    itemsToCreate.Add(unacceptedPayment);
                }
            }

            this.NativeInsert(itemsToCreate);
        }

        private void ProcessPortionDateNextMonth(IQueryable<BasePersonalAccount> query, int skip)
        {
            var accounts = query.Skip(skip).Take(this._take).ToArray();

            var transferRepo = this.Container.ResolveRepository<Transfer>();
            var importedPaymentRepo = this.Container.ResolveRepository<ImportedPayment>();
            var unacceptedPaymentRepo = this.Container.ResolveRepository<UnacceptedPayment>();
            var unacceptedPacketRepo = this.Container.ResolveRepository<UnacceptedPaymentPacket>();

            var accountIds = accounts.Select(x => x.Id).ToArray();
            var walletGuids = accounts.SelectMany(x => x.GetMainWalletGuids()).ToArray();
            var accountNums = accounts.Select(x => x.PersonalAccountNum).ToArray();

            var packetsByBankId = unacceptedPacketRepo.GetAll()
                .Where(x => x.BankDocumentId.HasValue)
                .AsEnumerable()
                .ToDictionary(x => x.BankDocumentId ?? 0L);

            var unaccPaymentQuery = unacceptedPaymentRepo.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id));

            var transferQuery = transferRepo.GetAll()
                .Where(x => walletGuids.Contains(x.TargetGuid))
                .Where(z => !unaccPaymentQuery.Any(x => x.TransferGuid == z.SourceGuid));

            var paymentIds = unaccPaymentQuery.Select(x => x.Id).ToArray();

            var importedPayments = importedPaymentRepo.GetAll()
                .Where(x => !paymentIds.Contains(x.PaymentId.Value))
                .Where(x => accountNums.Contains(x.Account))
                .Where(x => x.Account != null)
                .Where(x => x.PaymentId.HasValue)
                .AsEnumerable()
                .GroupBy(this.GetSimpleKey)
                .ToDictionary(x => x.Key, z => z.ToList());

            var transfersByAccount = this.AggregateTransfersByAccount(accounts, transferQuery.ToArray());

            var itemsToCreate = new List<UnacceptedPayment>();

            foreach (var account in accounts)
            {
                if (!transfersByAccount.ContainsKey(account.PersonalAccountNum))
                {
                    continue;
                }

                var transfers = transfersByAccount[account.PersonalAccountNum];

                foreach (var group in transfers.GroupBy(x => x.Operation.Id))
                {
                    var amount = group.Sum(x => x.Amount);

                    var key = this.GetSimpleKey(account, amount);

                    if (!importedPayments.ContainsKey(key))
                    {
                        continue;
                    }

                    var payments = importedPayments[key];

                    foreach (var transfer in group)
                    {
                        DateTime date;

                        if (transfer.PaymentDate.Month == 12)
                        {
                            date = new DateTime(transfer.PaymentDate.Year + 1, 1, 1);
                        }
                        else
                        {
                            date = new DateTime(transfer.PaymentDate.Year, transfer.PaymentDate.Month, 1);
                        }

                        var first = payments.FirstOrDefault(x => x.PaymentDate.Date == date);

                        if (first == null)
                        {
                            continue;
                        }

                        if (this._existsUnacceptedPayments.Contains(first.PaymentId ?? 0L))
                        {
                            continue;
                        }

                        this._existsUnacceptedPayments.Add(first.PaymentId ?? 0L);

                        var paymentType = this.GetPaymentType(account, transfer);

                        if (paymentType == 0)
                        {
                            continue;
                        }

                        var unacceptedPayment = new UnacceptedPayment
                        {
                            Id = first.PaymentId ?? 0L,
                            Packet = packetsByBankId[first.BankDocumentImport.Id],
                            Accepted = true,
                            PaymentDate = first.PaymentDate,
                            PaymentType = paymentType,
                            PersonalAccount = account
                        };

                        unacceptedPayment.SetGuid(transfer.SourceGuid);

                        if (paymentType == PaymentType.Basic)
                        {
                            unacceptedPayment.Sum = transfer.Amount;
                        }
                        else
                        {
                            unacceptedPayment.PenaltySum = transfer.Amount;
                        }

                        itemsToCreate.Add(unacceptedPayment);

                        payments.Remove(first);
                    }
                }
            }

            this.NativeInsert(itemsToCreate);
        }

        private string GetSimpleKey(ImportedPayment payment)
        {
            return string.Format("{0}#{1}", payment.Account.Trim(), payment.Sum.RegopRoundDecimal(2));
        }

        private string GetSimpleKey(BasePersonalAccount account, decimal amount)
        {
            return string.Format("{0}#{1}", account.PersonalAccountNum.Trim(), amount.RegopRoundDecimal(2));
        }

        private string GetSimpleKey(string accNum, decimal amount)
        {
            return string.Format("{0}#{1}", accNum.ToStr().Trim(), amount.RegopRoundDecimal(2));
        }

        private BaseDataResult Execute()
        {
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            var persAccPaymentDomain = this.Container.ResolveDomain<PersonalAccountPayment>();
            var packetDomain = this.Container.ResolveDomain<UnacceptedPaymentPacket>();
            var unAccPaymentDomain = this.Container.ResolveDomain<UnacceptedPayment>();
            var transferDomain = this.Container.ResolveDomain<Transfer>();

            try
            {
                var importeds = importedPaymentDomain.GetAll()
                    .Fetch(x => x.BankDocumentImport)
                    .ToList()
                    .Distinct();

                var persPayments = persAccPaymentDomain.GetAll()
                    .Fetch(x => x.BasePersonalAccount)
                    .Where(x => !unAccPaymentDomain.GetAll().Any(y => y.TransferGuid == x.Guid))
                    .Where(x => !unAccPaymentDomain.GetAll().Any(y => y.Guid == x.Guid))
                    .Select(
                        x => new
                        {
                            x.BasePersonalAccount.Id,
                            Account = x.BasePersonalAccount.PersonalAccountNum,
                            x.PaymentDate,
                            x.Sum,
                            x.Type,
                            x.Guid
                        })
                    .ToArray()
                    .Distinct();

                var query = importeds
                    .Join(
                        persPayments,
                        x => new {x.Account, x.Sum, PaymentDate = x.PaymentDate.Date},
                        y => new {y.Account, y.Sum, PaymentDate = y.PaymentDate.Date},
                        (a, b) => new {Imp = a, PayAcc = b})
                    .ToList();

                var packets = query
                    .Select(x => x.Imp.BankDocumentImport)
                    .Distinct()
                    .Select(
                        x => new
                        {
                            x.Id,
                            Packet = new UnacceptedPaymentPacket
                            {
                                CreateDate = x.DocumentDate.ToDateTime(),
                                Type = UnacceptedPaymentPacketType.Payment,
                                Sum = x.ImportedSum
                            }
                        })
                    .ToDictionary(x => x.Id, y => y.Packet);

                var unacceptedPayments = new List<UnacceptedPayment>();
                var dictPayImported = new List<Tuple<UnacceptedPayment, ImportedPayment>>();

                var guids = new HashSet<string>();

                query.ForEach(
                    x =>
                    {
                        var val = new UnacceptedPayment
                        {
                            Packet = packets.Get(x.Imp.BankDocumentImport.Id),
                            PersonalAccount = new BasePersonalAccount {Id = x.PayAcc.Id},
                            Sum = x.PayAcc.Sum,
                            PaymentDate = x.PayAcc.PaymentDate,
                            PaymentType = x.PayAcc.Type,
                            Accepted = true,
                            DocNumber = x.Imp.BankDocumentImport.DocumentNumber,
                            DocDate = x.Imp.BankDocumentImport.DocumentDate
                        };

                        if (!guids.Contains(x.PayAcc.Guid))
                        {
                            val.SetGuid(x.PayAcc.Guid);

                            guids.Add(x.PayAcc.Guid);

                            unacceptedPayments.Add(val);

                            x.Imp.PaymentId = val.Id;

                            dictPayImported.Add(new Tuple<UnacceptedPayment, ImportedPayment>(val, x.Imp));
                        }
                    });

                foreach (var packet in packets.Values)
                {
                    packet.Accept();
                }

                TransactionHelper.InsertInManyTransactions(this.Container, packets.Values, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, unacceptedPayments, 10000, true, true);
                dictPayImported.ForEach(x => x.Item2.PaymentId = x.Item1.Id);
                TransactionHelper.InsertInManyTransactions(this.Container, dictPayImported.Select(x => x.Item2).ToList(), 10000, true, true);

                var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();
                var transferImportedPayQuery = session.CreateSQLQuery(this.GetSQLString());
                var sqlQueryResult = transferImportedPayQuery.List();

                var transferImportPayDict = new Dictionary<long, long>();
                var transferPersAccDict = new Dictionary<long, long>();
                var usedTransfer = new HashSet<long>();

                foreach (object[] record in sqlQueryResult)
                {
                    var impId = record[0].ToLong();
                    var transferId = record[1].ToLong();
                    var persAccId = record[2].ToLong();

                    if (!transferImportPayDict.ContainsKey(impId) && !usedTransfer.Contains(transferId))
                    {
                        usedTransfer.Add(transferId);
                        transferImportPayDict.Add(impId, transferId);
                    }

                    if (!transferPersAccDict.ContainsKey(transferId))
                    {
                        transferPersAccDict.Add(transferId, persAccId);
                    }
                }

                var importedIds = transferImportPayDict.Keys.ToList();

                var importedDict = importedPaymentDomain.GetAll()
                    .Fetch(x => x.BankDocumentImport)
                    .Where(x => importedIds.Contains(x.Id))
                    .ToDictionary(x => x.Id);

                var transferIds = transferImportPayDict.Values.ToList();

                var transfersDict = transferDomain.GetAll()
                    .Where(x => transferIds.Contains(x.Id))
                    .Where(x => !unAccPaymentDomain.GetAll().Any(y => y.Guid == x.SourceGuid))
                    .Where(x => !unAccPaymentDomain.GetAll().Any(y => y.TransferGuid == x.SourceGuid))
                    .ToDictionary(x => x.Id);

                packets = importedDict.Values
                    .Select(x => x.BankDocumentImport)
                    .Distinct()
                    .ToList()
                    .Select(
                        x => new
                        {
                            x.Id,
                            Packet = new UnacceptedPaymentPacket
                            {
                                CreateDate = x.DocumentDate.ToDateTime(),
                                Type = UnacceptedPaymentPacketType.Payment,
                                Sum = x.ImportedSum
                            }
                        })
                    .ToDictionary(x => x.Id, y => y.Packet);

                unacceptedPayments.Clear();
                dictPayImported.Clear();

                foreach (var transferImportPay in transferImportPayDict)
                {
                    var imp = importedDict.Get(transferImportPay.Key);
                    var tr = transfersDict.Get(transferImportPay.Value);

                    if (imp == null || tr == null)
                    {
                        continue;
                    }

                    var persId = transferPersAccDict.Get(transferImportPay.Value);
                    var val = new UnacceptedPayment
                    {
                        Packet = packets.Get(imp.BankDocumentImport.Id),
                        PersonalAccount = new BasePersonalAccount {Id = persId},
                        Sum = tr.Amount,
                        PaymentDate = tr.PaymentDate,
                        PaymentType = PaymentType.Basic,
                        Accepted = true,
                        DocNumber = imp.BankDocumentImport.DocumentNumber,
                        DocDate = imp.BankDocumentImport.DocumentDate
                    };

                    if (!guids.Contains(tr.SourceGuid))
                    {
                        val.SetGuid(tr.SourceGuid);

                        guids.Add(tr.SourceGuid);

                        unacceptedPayments.Add(val);

                        imp.PaymentId = val.Id;

                        dictPayImported.Add(new Tuple<UnacceptedPayment, ImportedPayment>(val, imp));
                    }
                }

                foreach (var packet in packets.Values)
                {
                    packet.Accept();
                }

                TransactionHelper.InsertInManyTransactions(this.Container, packets.Values, 10000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, unacceptedPayments, 10000, true, true);
                dictPayImported.ForEach(x => x.Item2.PaymentId = x.Item1.Id);
                TransactionHelper.InsertInManyTransactions(this.Container, dictPayImported.Select(x => x.Item2).ToList(), 10000, true, true);

                packetDomain.GetAll()
                    .Where(x => !unAccPaymentDomain.GetAll().Any(y => y.Packet.Id == x.Id))
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => packetDomain.Delete(x));
            }
            finally
            {
                this.Container.Release(importedPaymentDomain);
                this.Container.Release(persAccPaymentDomain);
                this.Container.Release(packetDomain);
                this.Container.Release(unAccPaymentDomain);
            }

            return new BaseDataResult();
        }

        private string GetSQLString()
        {
            return @"select distinct imp.id, tr.transfer, tr.pers_acc_id from regop_imported_payment imp join 

(select pers_acc_id, pers_acc,  summ,   pay_date, source_guid, uniq_pay_num,transfer from (SELECT
  ca.name                                             AS regop,
  CASE WHEN (r_calc_acc is not null) THEN 'Счет регионального оператора' ELSE 'Специальный счет' END AS acc_type ,
  calc_acc.account_number                             AS calc_acc,
  'Камчатский край'                                   AS region,
  mu.name                                             AS mr,
  settl.name                                          AS mo,
  fias_address.place_name                             AS place,
  fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '')  AS street,
  'Камчатский край' || ', ' || mu.name || ', ' || settl.name || ', ' || fias_address.place_name || ', ' || fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '') AS address,
  room.croom_num                                      AS room_num,
  pay_acc.acc_num                                     AS pay_acc,
  p_acc.id                                       AS pers_acc_id,
  p_acc.acc_num                                       AS pers_acc,
  CASE WHEN (p_acc_owner.owner_type = 0) THEN 'Физ. лицо' ELSE 'Юр. лицо' END AS pers_acc_type,
  p_acc_owner.name                                    AS owner,
  t.operation_date                                    AS oper_date,
  t.id                                                AS transfer,
  t.payment_date                                      AS pay_date,
   t.source_guid                                      AS source_guid,
  to_char(t.operation_date, 'TMMonth')                AS period,
  extract(YEAR FROM t.operation_date)                 AS year,
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS oper_type,
  t.amount                                            AS summ,
  'Счет ЛС'                                           AS source,
  coalesce(imp_sus.payment_agent_name, imp_sus_pay.payment_agent_name) as pay_agent,
  coalesce(imp_sus.id, imp_sus_pay.id) as register,
  coalesce(imp_item_sus.id, imp_item_pay.id)                       AS uniq_pay_num
FROM regop_calc_acc calc_acc
left join regop_calc_acc_regop r_calc_acc on r_calc_acc.id = calc_acc.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN b4_fias_address fias_address ON fias_address.id = ro.fias_address_id
  LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
  LEFT JOIN gkh_dict_municipality settl ON settl.id = ro.stl_municipality_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_pers_acc_owner p_acc_owner ON p_acc.acc_owner_id = p_acc_owner.id
  INNER JOIN regop_wallet w ON w.id = p_acc.bt_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id
    -- связь с платежными агентами со стороны НВС
  left join regop_suspense_account susp on susp.c_guid = t.source_guid
  left join regop_imported_payment imp_item_sus on imp_item_sus.payment_id = susp.id and imp_item_sus.payment_state = 20 /*нвс*/
  left join regop_bank_doc_import imp_sus on imp_sus.id = imp_item_sus.bank_doc_id
  -- связь с платежными агентами со стороны НВС
  left join regop_unaccept_pay pay on pay.transfer_guid = t.source_guid
  left join regop_imported_payment imp_item_pay on imp_item_pay.payment_id = pay.id and imp_item_pay.payment_state = 10 /*ноп*/
  left join regop_bank_doc_import imp_sus_pay on imp_sus_pay.id = imp_item_pay.bank_doc_id

UNION ALL

SELECT
  ca.name                                             AS regop,
  CASE WHEN (r_calc_acc is not null) THEN 'Счет регионального оператора' ELSE 'Специальный счет' END AS acc_type ,
  calc_acc.account_number                             AS calc_acc,
  'Камчатский край'                                   AS region,
  mu.name                                             AS mr,
  settl.name                                          AS mo,
  fias_address.place_name                             AS place,
  fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '')  AS street,
  'Камчатский край' || ', ' || mu.name || ', ' || settl.name || ', ' || fias_address.place_name || ', ' || fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '') AS address,
  room.croom_num                                      AS room_num,
  pay_acc.acc_num                                     AS pay_acc,
  p_acc.id                                       AS pers_acc_id,
  p_acc.acc_num                                       AS pers_acc,
  CASE WHEN (p_acc_owner.owner_type = 0) THEN 'Физ. лицо' ELSE 'Юр. лицо' END AS pers_acc_type,
  p_acc_owner.name                                    AS owner,
  t.operation_date                                    AS oper_date,
    t.id                                                AS transfer,
  t.payment_date                                      AS pay_date,
     t.source_guid                                      AS source_guid,
  to_char(t.operation_date, 'TMMonth')                AS period,
  extract(YEAR FROM t.operation_date)                 AS year,
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS oper_type,
  t.amount                                            AS summ,
  'Счет ЛС'                                           AS source,
  coalesce(imp_sus.payment_agent_name, imp_sus_pay.payment_agent_name) as pay_agent,
  coalesce(imp_sus.id, imp_sus_pay.id) as register,
  coalesce(imp_item_sus.id, imp_item_pay.id)                       AS uniq_pay_num
FROM regop_calc_acc calc_acc
left join regop_calc_acc_regop r_calc_acc on r_calc_acc.id = calc_acc.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN b4_fias_address fias_address ON fias_address.id = ro.fias_address_id
  LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
  LEFT JOIN gkh_dict_municipality settl ON settl.id = ro.stl_municipality_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_pers_acc_owner p_acc_owner ON p_acc.acc_owner_id = p_acc_owner.id
  INNER JOIN regop_wallet w ON w.id = p_acc.dt_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id
    -- связь с платежными агентами со стороны НВС
  left join regop_suspense_account susp on susp.c_guid = t.source_guid
  left join regop_imported_payment imp_item_sus on imp_item_sus.payment_id = susp.id and imp_item_sus.payment_state = 20 /*нвс*/
  left join regop_bank_doc_import imp_sus on imp_sus.id = imp_item_sus.bank_doc_id
  -- связь с платежными агентами со стороны НВС
  left join regop_unaccept_pay pay on pay.transfer_guid = t.source_guid
  left join regop_imported_payment imp_item_pay on imp_item_pay.payment_id = pay.id and imp_item_pay.payment_state = 10 /*ноп*/
  left join regop_bank_doc_import imp_sus_pay on imp_sus_pay.id = imp_item_pay.bank_doc_id

UNION ALL

SELECT
  ca.name                                             AS regop,
  CASE WHEN (r_calc_acc is not null) THEN 'Счет регионального оператора' ELSE 'Специальный счет' END AS acc_type ,
  calc_acc.account_number                             AS calc_acc,
  'Камчатский край'                                   AS region,
  mu.name                                             AS mr,
  settl.name                                          AS mo,
  fias_address.place_name                             AS place,
  fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '')  AS street,
  'Камчатский край' || ', ' || mu.name || ', ' || settl.name || ', ' || fias_address.place_name || ', ' || fias_address.street_name || ', ' || fias_address.house || COALESCE(fias_address.letter, '') || COALESCE(fias_address.housing, '') AS address,
  room.croom_num                                      AS room_num,
  pay_acc.acc_num                                     AS pay_acc,
    p_acc.id                                       AS pers_acc_id,
  p_acc.acc_num                                       AS pers_acc,
  CASE WHEN (p_acc_owner.owner_type = 0) THEN 'Физ. лицо' ELSE 'Юр. лицо' END AS pers_acc_type,
  p_acc_owner.name                                    AS owner,
  t.operation_date                                    AS oper_date,
    t.id                                                AS transfer,
  t.payment_date                                      AS pay_date,
     t.source_guid                                      AS source_guid,
  to_char(t.operation_date, 'TMMonth')                AS period,
  extract(YEAR FROM t.operation_date)                 AS year,
  coalesce(coalesce(t.reason, tt.reason), mop.reason) AS oper_type,
  t.amount                                            AS summ,
  'Счет ЛС'                                           AS source,
  coalesce(imp_sus.payment_agent_name, imp_sus_pay.payment_agent_name) as pay_agent,
  coalesce(imp_sus.id, imp_sus_pay.id) as register,
  coalesce(imp_item_sus.id, imp_item_pay.id)                       AS uniq_pay_num
FROM regop_calc_acc calc_acc
left join regop_calc_acc_regop r_calc_acc on r_calc_acc.id = calc_acc.id
  LEFT JOIN gkh_contragent ca ON ca.id = calc_acc.account_owner_id
  LEFT JOIN regop_calc_acc_ro calc_acc_ro ON calc_acc_ro.account_id = calc_acc.id
  LEFT JOIN gkh_reality_object ro ON ro.id = calc_acc_ro.ro_id
  LEFT JOIN b4_fias_address fias_address ON fias_address.id = ro.fias_address_id
  LEFT JOIN gkh_dict_municipality mu ON mu.id = ro.municipality_id
  LEFT JOIN gkh_dict_municipality settl ON settl.id = ro.stl_municipality_id
  LEFT JOIN regop_ro_payment_account pay_acc ON pay_acc.ro_id = ro.id
  INNER JOIN gkh_room room ON room.ro_id = ro.id
  INNER JOIN regop_pers_acc p_acc ON p_acc.room_id = room.id
  INNER JOIN regop_pers_acc_owner p_acc_owner ON p_acc.acc_owner_id = p_acc_owner.id
  INNER JOIN regop_wallet w ON w.id = p_acc.p_wallet_id
  INNER JOIN regop_transfer t ON t.target_guid = w.wallet_guid
  INNER JOIN regop_money_operation mop ON mop.id = t.op_id AND (mop.reason is null or mop.reason != 'Слияние счетов')
  LEFT JOIN regop_transfer tt ON tt.id = t.originator_id
      -- связь с платежными агентами со стороны НВС
  left join regop_suspense_account susp on susp.c_guid = t.source_guid
  left join regop_imported_payment imp_item_sus on imp_item_sus.payment_id = susp.id and imp_item_sus.payment_state = 20 /*нвс*/
  left join regop_bank_doc_import imp_sus on imp_sus.id = imp_item_sus.bank_doc_id
  -- связь с платежными агентами со стороны НВС
  left join regop_unaccept_pay pay on pay.transfer_guid = t.source_guid
  left join regop_imported_payment imp_item_pay on imp_item_pay.payment_id = pay.id and imp_item_pay.payment_state = 10 /*ноп*/
  left join regop_bank_doc_import imp_sus_pay on imp_sus_pay.id = imp_item_pay.bank_doc_id
                         ) as query
where oper_type != 'Перенос долга при слиянии' and register is null) tr

on  imp.payment_sum = tr.summ  and imp.payment_account = tr.pers_acc and imp.payment_state = 10
left join REGOP_UNACCEPT_PAY uap on uap.id = imp.payment_id or tr.source_guid = uap.transfer_guid
where uap.id is null and tr.pay_date >= imp.payment_date
";
        }

        private string GetSQLString2()
        {
            return @"
                select p.id, i.id from regop_pers_acc_payment p
                  left join regop_unaccept_pay u on p.pguid = u.pguid
                  join regop_imported_payment i on i.payment_sum = p.payment_sum and  i.payment_date = p.payment_date and i.payment_state = 10
                where u.id is null";
        }
    }
}