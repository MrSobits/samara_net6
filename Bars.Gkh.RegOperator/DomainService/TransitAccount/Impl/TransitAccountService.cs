namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using B4;

    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Bars.Gkh.Utils;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Gkh.Entities;
    using Overhaul.Entities;
    using Entities;

    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using NHibernate;

    public class TransitAccountService : ITransitAccountService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ExportToTxt(BaseParams baseParams)
        {
            var creditService = Container.Resolve<IDomainService<ControlTransitAccCreditProxy>>();
            var regoperatorCalcAccService = Container.Resolve<IDomainService<RegopCalcAccount>>();
            var regoperatorService = Container.Resolve<IDomainService<RegOperator>>();

            var fileManager = Container.Resolve<IFileManager>();
            var recId = baseParams.Params.GetAs<long>("recId");
            var creditRec = creditService.Get(recId);

            var regoperator = regoperatorService.GetAll().FirstOrDefault().Return(x => new { x.Contragent });

            var accNum =
                regoperatorCalcAccService.GetAll().FirstOrDefault(x => x.IsTransit)
                .Return(x => 
                    new
                        {
                            AccountNumber = x.Return(y => y.AccountNumber),
                            CreditOrg = x.Return(y => y.CreditOrg).Return(y => y.Name)
                        });

            try
            {
                var text = new StringBuilder();
                text.AppendFormat("ДатаСоздания={0}\r\n", DateTime.Now.ToShortDateString());
                text.AppendLine("Документ=Платежное поручение");
                text.AppendFormat(
                    "ДатаРаспоряжения={0}\r\n",
                    creditRec.Date.HasValue ? creditRec.Date.Value.ToShortDateString() : string.Empty);
                text.AppendFormat("СуммаРаспоряжения={0}\r\n", creditRec.Sum.RegopRoundDecimal(2));
                text.AppendFormat("ПлательщикРасчСчет={0}\r\n", accNum.Return(x => x.AccountNumber));
                text.AppendFormat("ПлательщикБанк={0}\r\n", accNum.Return(x => x.CreditOrg));
                text.AppendFormat("ПолучательРасчСчет={0}\r\n", creditRec.CalcAccount);
                text.AppendFormat("ПолучательИНН={0}\r\n", regoperator.Return(x => x.Contragent).Return(x => x.Inn));
                text.AppendFormat("Получатель={0}\r\n", regoperator.Return(x => x.Contragent).Return(x => x.Name));
                text.AppendFormat("ПолучательКПП={0}\r\n", regoperator.Return(x => x.Contragent).Return(x => x.Kpp));
                text.AppendFormat("НазначениеПлатежа\r\n");
                text.AppendLine("КонецДокумента");
                text.AppendLine("КонецФайла");
                var byteArray = Encoding.UTF8.GetBytes(text.ToStr());
                return new BaseDataResult(fileManager.SaveFile("Выгрузка", "txt", byteArray));
            }
            finally
            {
                Container.Release(creditService);
                Container.Release(regoperatorCalcAccService);
                Container.Release(regoperatorService);
            }
        }

        // Получение дополнительной инфомрации для контроля транзитного счета
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var debetService = Container.Resolve<IDomainService<ControlTransitAccDebetProxy>>();
            var creditService = Container.Resolve<IDomainService<ControlTransitAccCreditProxy>>();

            var debet = debetService.GetAll().Select(x => (decimal?)x.ConfirmSum).Sum() ?? 0m;
            var credit = creditService.GetAll().Select(x => (decimal?)x.ConfirmSum).Sum() ?? 0m;

            Container.Release(debetService);
            Container.Release(creditService);

            return new BaseDataResult(new { unallocatedBalance = debet - credit });
        }

        public void MakeDebetList()
        {
            var bankDocImportService = Container.Resolve<IDomainService<BankDocumentImport>>();
            var bankAccStatementService = Container.Resolve<IDomainService<BankAccountStatement>>();
            var regoperatorCalcAccService = Container.Resolve<IDomainService<RegopCalcAccount>>();
            var contragentBankCreditOrgService = Container.Resolve<IDomainService<ContragentBankCreditOrg>>();
            var paymentAgentService = Container.Resolve<IDomainService<PaymentAgent>>();
            var debetService = Container.Resolve<IDomainService<ControlTransitAccDebetProxy>>();
            var sessionProvider = Container.Resolve<ISessionProvider>();

            var paymentAgentDict =
                paymentAgentService.GetAll()
                    .Where(x => x.Contragent != null)
                    .Select(x => new { x.Code, x.Contragent.Id })
                    .ToList()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id));

            var contragentAccs = contragentBankCreditOrgService.GetAll()
                .Where(x => x.Contragent != null)
                .Select(y => new { y.SettlementAccount, y.Contragent.Id })
                .ToList()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.SettlementAccount).ToList());

            var accNum =
                regoperatorCalcAccService.GetAll()
                    .FirstOrDefault(x => x.IsTransit)
                    .Return(x => x.AccountNumber, string.Empty);

            var bankStatements = bankAccStatementService.GetAll()
                .GroupBy(x => x.DocumentDate.Date)
                .ToDictionary(x => x.Key, z => z.ToList());

            var session = sessionProvider.GetCurrentSession();
            session.FlushMode = FlushMode.Commit;

            try
            {
                var data = bankDocImportService.GetAll()
                    .Where(x => x.DocumentDate != null)
                    .ToList()
                    .Select(x =>
                    {
                        var contragentId = !x.PaymentAgentCode.IsEmpty()
                            ? paymentAgentDict.Get(x.PaymentAgentCode)
                            : 0L;

                        var accs = contragentAccs.Get(contragentId) ?? new List<string>();

                        return new
                        {
                            Number = x.Return(y => y.DocumentNumber),
                            Date = x.Return(y => y.DocumentDate),
                            PaymentAgentName = x.Return(y => y.PaymentAgentName),
                            PaymentAgentCode = x.Return(y => y.PaymentAgentCode),
                            Sum = x.ImportedSum.HasValue ? x.ImportedSum.Value : 0m,
                            ConfirmSum = x.DocumentDate.HasValue
                                ? (bankStatements.Get(x.DocumentDate.Value.Date) ?? new List<BankAccountStatement>())
                                    .Where(z => z.RecipientAccountNum == accNum && accs.Contains(z.PayerAccountNum))
                                    .SafeSum(z => z.Sum)
                                : 0m
                        };
                    })
                    .Select(x => new ControlTransitAccDebetProxy
                    {
                        Number = x.Number,
                        Date = x.Date,
                        PaymentAgentName = x.PaymentAgentName,
                        PaymentAgentCode = x.PaymentAgentCode,
                        Sum = x.Sum,
                        ConfirmSum = x.ConfirmSum,
                        Divergence = x.Sum - x.ConfirmSum
                    })
                    .ToList();
                
                Container.InTransaction(() =>
                {
                    debetService.GetAll().ToList().ForEach(x => debetService.Delete(x.Id));
                    foreach (var controlTransitAccDebetProxy in data)
                    {
                        debetService.Save(controlTransitAccDebetProxy);
                    }
                });
            }
            finally
            {
                Container.Release(sessionProvider);

                session.FlushMode = FlushMode.Auto;
            }
        }

        public void MakeCreditList()
        {
            var bankDocImportService = Container.Resolve<IDomainService<BankDocumentImport>>();
            var importPaymentService = Container.Resolve<IDomainService<ImportedPayment>>();
            var personalAccountService = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var calcAccRoService = Container.Resolve<IDomainService<CalcAccountRealityObject>>();
            var bankAccStatementService = Container.Resolve<IDomainService<BankAccountStatement>>();
            var creditService = Container.Resolve<IDomainService<ControlTransitAccCreditProxy>>();

            var accNum =
                personalAccountService.GetAll()
                    .Join(
                        calcAccRoService.GetAll(),
                        x => x.Room.RealityObject.Id,
                        y => y.RealityObject.Id,
                        (x, y) => new { x.PersonalAccountNum, y.Account.AccountNumber, y.Account.CreditOrg.Name })
                    .ToList()
                    .GroupBy(x => x.PersonalAccountNum)
                    .ToDictionary(x => x.Key);

            var importedPayments = importPaymentService.GetAll()
                .Select(x => new { x.Account, ImportId = x.BankDocumentImport.Id, x.Sum })
                .ToList()
                .GroupBy(x => x.ImportId)
                .ToDictionary(x => x.Key);

            var result = new List<ControlTransitAccCreditProxy>();

            foreach (var bankDocumentImport in bankDocImportService.GetAll().Where(x => x.DocumentDate != null))
            {
                var documentImport = bankDocumentImport;

                if (importedPayments.ContainsKey(bankDocumentImport.Id))
                {
                    var imports = importedPayments[bankDocumentImport.Id];

                    foreach (var import in imports)
                    {
                        if (accNum.ContainsKey(import.Account))
                        {
                            var accnum = accNum[import.Account];
                            var accNumFirst = accnum.FirstOrDefault(x => x.AccountNumber.IsNotEmpty() && x.Name.IsNotEmpty());
                            var calcAccNum = accNumFirst.Return(x => x.AccountNumber);
                            var creditOrgName = accNumFirst.Return(x => x.Name);
                            var proxy = result.FirstOrDefault(x => x.CalcAccount == calcAccNum && x.Date == documentImport.DocumentDate);
                            if (proxy == null)
                            {
                                var confirmSum = documentImport.DocumentDate != null &&
                                    bankAccStatementService.GetAll()
                                        .Any(
                                            x =>
                                            x.RecipientAccountNum == calcAccNum  && documentImport.DocumentDate.HasValue
                                            && x.DocumentDate.Date == documentImport.DocumentDate.Value.Date)
                                        ? bankAccStatementService.GetAll()
                                              .Where(
                                                  x =>
                                                  x.RecipientAccountNum == calcAccNum && documentImport.DocumentDate.HasValue
                                                  && x.DocumentDate.Date == documentImport.DocumentDate.Value.Date)
                                              .Sum(x => x.Sum)
                                        : 0;

                                proxy = new ControlTransitAccCreditProxy
                                {
                                    CalcAccount = calcAccNum,
                                    Sum = import.Sum,
                                    CreditOrgName = creditOrgName,
                                    Date = bankDocumentImport.DocumentDate,
                                    ConfirmSum = confirmSum,
                                    Divergence = import.Sum - confirmSum
                                };

                                result.Add(proxy);
                            }
                            else
                            {
                                proxy.Sum += import.Sum;
                                proxy.Divergence += import.Sum;
                            }
                        }
                    }
                }
            }

            creditService.GetAll().Select(x => x.Id).ForEach(x => creditService.Delete(x));

            foreach (var controlTransitAccCreditProxy in result)
            {
                creditService.Save(controlTransitAccCreditProxy);
            }
        }

        // Список дебетовых записей для контроля транзитного счета
        public IList DebetList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var service = Container.Resolve<IDomainService<ControlTransitAccDebetProxy>>();
            var loadParams = baseParams.GetLoadParam();

            var data = service.GetAll().Filter(loadParams, Container);
            totalCount = data.Count();
            data = paging ? data.Order(loadParams).Paging(loadParams) : data.Order(loadParams);

            return data.ToList();
        }

        // Список кредетовых записей для контроля транзитного счета
        public IList CreditList(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var service = Container.Resolve<IDomainService<ControlTransitAccCreditProxy>>();
            var loadParams = baseParams.GetLoadParam();

            var data = service.GetAll().Filter(loadParams, Container);
            totalCount = data.Count();
            data = paging ? data.Order(loadParams).Paging(loadParams) : data.Order(loadParams);

            return data.ToList();
        }
    }
}