namespace Bars.GkhCr.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Serialization;

    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Ionic.Zip;

    /// <summary>
    /// Импорт платежных поручений
    /// </summary>
    public class PaymentOrderImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ObjectCr objectCr;

        private int programId;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "PaymentOrder"; }
        }

        public override string Name
        {
            get { return "Импорт платежных поручений"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xml,zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.PaymentOrder.View"; }
        }

        public PaymentOrderImport(ILogImportManager logImportManager)
        {
            this.LogImportManager = logImportManager;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            programId = baseParams.Params["ProgramCr"].ToInt();
            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);

                switch (fileData.Extention)
                {
                    case "xml":
                            this.ImportFile(fileData.FileName, memoryStreamFile);
                        break;
                    case "zip":
                        using (var zipFile = ZipFile.Read(memoryStreamFile))
                        {
                            var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".xml")).ToList();

                            if (zipEntries.Count == 0)
                            {
                                var logImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
                                if (logImport == null)
                                {
                                    throw new Exception("Не найдена реализация интерфейса ILogImport");
                                }

                                logImport.SetFileName(fileData.FileName);
                                logImport.ImportKey = Key;
                                logImport.IsImported = false;
                                logImport.Error(this.Name, "Не удалось обнаружить xml файлы для импорта");
                                LogImportManager.Add(fileData, logImport);
                            }

                            foreach (var zipEntry in zipEntries)
                            {
                                using (var fileImport = new MemoryStream())
                                {
                                    zipEntry.Extract(fileImport);
                                    fileImport.Seek(0, SeekOrigin.Begin);
                                    this.ImportFile(zipEntry.FileName, fileImport);
                                }
                            }
                        }

                        break;
                }
            }

            LogImportManager.Save();

            return new ImportResult(LogImportManager.GetStatus(), this.LogImportManager.GetInfo(), string.Empty, LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                switch (fileData.Extention)
                {
                    case "xml":
                        {
                            return ValidateFile(fileData.FileName, ref message, memoryStreamFile);
                        }

                    case "zip":
                        using (var zipFile = ZipFile.Read(memoryStreamFile))
                        {
                            var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".xml")).ToList();

                            if (zipEntries.Count == 0)
                            {
                                message = "Не найдены файлы для импорта";
                            }

                            foreach (var zipEntry in zipEntries)
                            {
                                using (var fileImport = new MemoryStream())
                                {
                                    fileImport.Seek(0, SeekOrigin.Begin);
                                    zipEntry.Extract(fileImport);

                                    if (!ValidateFile(zipEntry.FileName, ref message, fileImport))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        break;
                }
            }

            return true;
        }

        private static bool ValidateFile(string fileName, ref string message, Stream stream)
        {
               stream.Seek(0, SeekOrigin.Begin);
               ExtrWt file;
               try
               {
                   using (var reader = new StreamReader(stream, Encoding.GetEncoding(1251)))
                   {
                       var ser = new XmlSerializer(typeof(ExtrWt));
                       file = (ExtrWt)ser.Deserialize(reader);
                   }
               }
               catch (Exception)
               {
                   message = string.Format("Файл {0} не соответствует формату выгрузки АЦК-Финансы вер.1.0", fileName);
                   return false;
               }

               if (file.Item.StatementsDocs.Length != 1 || file.Item.StatementsDocs[0] == null
                   || file.Item.StatementsDocs[0].StatementDoc.Length < 1)
               {
                   message = string.Format("Файл {0} не содержит банковских выписок", fileName);
                   return false;
               }

            return true;
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;
        }

        private void ImportFile(string fileName, Stream memoryStreamFile)
        {
            var logFile = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logFile == null) throw new Exception("Не найдена реализация интерфейса ILogImport");
            logFile.SetFileName(fileName);
            logFile.ImportKey = Key;

            ExtrWt fileExtrWt;
            try
            {
                using (var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251)))
                {
                    var ser = new XmlSerializer(typeof(ExtrWt));
                    fileExtrWt = (ExtrWt)ser.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                logFile.Error(this.Name, string.Format("Выбранный файл не соответствует формату выгрузки АЦК-Финансы вер.1.0"));
                return;
            }

            foreach (var statementDoc in fileExtrWt.Item.StatementsDocs[0].StatementDoc)
            {
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        this.ProcessingBankStatement(statementDoc, logFile);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        logFile.Error(this.Name, string.Format("Произошла неизвестная ошибка. {0}", ex.Message));
                        transaction.Rollback();
                    }
                }
            }

            LogImportManager.AddLog(logFile);
        }

        private void ProcessingBankStatement(StatementDoc record, ILogImport logFile)
        {
            var bankStatementService = Container.Resolve<IDomainService<BankStatement>>();

            var docNum = record.DocumentNumber.Trim();
            var personalAccountNumber = record.PayerAccountNumber.Trim();
            var statementDate = record.StatementDate.ToDateTime();

            // получаем лицевой счет
            var personalAccounts = Container.Resolve<IDomainService<PersonalAccount>>().GetAll()
                         .Where(x => x.ObjectCr.ProgramCr.Id == programId && x.Account == personalAccountNumber)
                         .ToArray();

            if (personalAccounts.Length != 1)
            {
                logFile.Warn(this.Name, "Лицевой cчет объекта КР не может быть идентифицирован однозначно по данной программе КР и лицевому счету");
                return;
            }

            var personalAccount = personalAccounts[0];
            objectCr = personalAccount.ObjectCr;

            // Получим Банковские выписки по объекту КР и дате выписки
            var bankStatements = bankStatementService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCr.Id && x.PersonalAccount == personalAccountNumber && x.DocumentDate.Value == statementDate && x.DocumentDate.HasValue)
                .ToArray();

            if (bankStatements.Length > 0)
            {
                var bankStatement = bankStatements[0];

                // Сравниваем импортируемую банковскую выписку с имеющийся в системе
                if (СompareBankStatement(record, bankStatement))
                {
                    bankStatement.TypeFinanceGroup = personalAccount.FinanceGroup;
                    bankStatementService.Update(bankStatement);

                    logFile.Warn(Name, string.Format("Выписка {0} от {1} по объекту '{2}' ранее уже загружена. Выписка будет пропущена.", docNum, statementDate.ToShortDateString(), objectCr.RealityObject.Address));
                    return;
                }

                // Если данные отличаются то перед загрузкой удалим старые данные
                this.DeleteBankStatement(bankStatement, bankStatementService);
            }

            // Если последний день операции по счету не задан, то считаем что не было предыдущих банковских выписок связанной с импортируемой, сразу грузим ее
            if (string.IsNullOrEmpty(record.LastStatementDate))
            {
                this.ImportBankStatement(record, personalAccount, logFile);
                return;
            }

            var operLastDate = record.LastStatementDate.ToDateTime();

            // Получим банковские выписки объекта КР по последниму дню операции по счету
            // Проверяем лицевые счета по всем окр без учета программы а сажаем в тот окр программа которого указана в файле импорта(#5638)
            var lastBankStatement = bankStatementService.GetAll()
                                   .Where(x => x.ObjectCr.RealityObject.Id == this.objectCr.RealityObject.Id && x.DocumentDate.HasValue && x.PersonalAccount == personalAccountNumber)
                                   .FirstOrDefault(x => x.DocumentDate.Value.Date == operLastDate.Date);

            if (lastBankStatement == null)
            {
                var msg =
                    string.Format(
                        "Загрузка предыдущей выписки от '{0}' по объекту '{1}' ранее не производилась. Загрузка текущей выписки '{2}' от '{3}' возможна только после загрузки предыдущих. Выписка будет пропущена.",
                        operLastDate.ToShortDateString(),
                        this.objectCr.RealityObject.Address,
                        docNum,
                        statementDate.ToShortDateString());
                logFile.Warn(this.Name, msg);
                return;
            }

            var openingBalance = !string.IsNullOrEmpty(record.OpeningBalance)
                                     ? record.OpeningBalance.Trim().ToDecimal()
                                     : 0M;

            if (lastBankStatement.OutgoingBalance != openingBalance)
            {
                var msg =
                    string.Format(
                        "Несовпадение исходящего остатка выписки '{0}' от '{1}' с входящим остатком по выписке '{2}' от '{3}'. Выписка будет пропущена.",
                        lastBankStatement.DocumentNum,
                        lastBankStatement.DocumentDate.ToDateTime().ToShortDateString(),
                        docNum,
                        statementDate.ToShortDateString());

                logFile.Warn(this.Name, msg);
                return;
            }

            this.ImportBankStatement(record, personalAccount, logFile);
        }

        private void DeleteBankStatement(BankStatement bankStatement, IDomainService<BankStatement> bankStatementService)
        {
            // Рекурсивное удаление банковских выписок начиная с (OperLastDate == DocumentDate)
            var bankStatementForDelete = this.GetBankStatementRecursively(bankStatement);
            foreach (var tempBankStatement in bankStatementForDelete.Distinct())
            {
                var bankStatId = tempBankStatement.Id;

                var paymentOrderInService = Container.Resolve<IDomainService<PaymentOrderIn>>();
                var paymentOrderOutService = Container.Resolve<IDomainService<PaymentOrderOut>>();
                foreach (var id in paymentOrderInService.GetAll().Where(x => x.BankStatement.Id == bankStatId).Select(x => x.Id))
                {
                    paymentOrderInService.Delete(id);
                }

                foreach (var id in paymentOrderOutService.GetAll().Where(x => x.BankStatement.Id == bankStatId).Select(x => x.Id))
                {
                    paymentOrderOutService.Delete(id);
                }

                bankStatementService.Delete(tempBankStatement.Id);
            }
        }

        /// <summary>
        /// Импорт Банковской выписки
        /// </summary>
        /// <param name="record">Импортируемые данные</param>
        /// <param name="personalAccount">Лицевой счет</param>
        /// <param name="logFile"></param>
        private void ImportBankStatement(StatementDoc record, PersonalAccount personalAccount, ILogImport logFile)
        {
            var bankStatementService = Container.Resolve<IDomainService<BankStatement>>();
            var manOrgService = Container.Resolve<IDomainService<ManagingOrganization>>();
            var contragentService = Container.Resolve<IDomainService<Contragent>>();
            var paymentOrderOutService = this.Container.Resolve<IDomainService<PaymentOrderOut>>();
            var paymentOrderInService = this.Container.Resolve<IDomainService<PaymentOrderIn>>();

            var documentDate = record.StatementDate.ToDateTime();
            var operLastDate = record.LastStatementDate.ToDateTime();

            var bankStatement = new BankStatement
            {
                Period = objectCr.ProgramCr.Period,
                ObjectCr = objectCr,
                DocumentDate = documentDate,
                BudgetYear = record.BudgetYear.ToInt(),
                OutgoingBalance = record.ClosingBalance.ToDecimal(),
                DocumentNum = !string.IsNullOrEmpty(record.DocumentNumber) ? record.DocumentNumber.Trim() : string.Empty,
                OperLastDate = operLastDate != DateTime.MinValue ? (DateTime?)operLastDate : null,
                IncomingBalance = record.OpeningBalance.ToDecimal(),
                PersonalAccount = personalAccount.Account,
                TypeFinanceGroup = personalAccount.FinanceGroup
            };                       

            if (string.IsNullOrEmpty(record.PayerName))
            {
                logFile.Warn(this.Name, "По выписке отсутствует контрагент");
            }
            else
            {
                var contragent = contragentService.GetAll().FirstOrDefault(x => x.Inn == record.PayerInn && x.Kpp == record.PayerKpp);
                if (contragent == null)
                {
                    var msg = string.Format(
                        "Не найден контрагент – {0} (ИНН: {1}, КПП: {2}).",
                        record.PayerName,
                        record.PayerInn,
                        record.PayerKpp);

                    logFile.Warn(this.Name, msg);
                }
                else
                {
                    var managingOrganization = manOrgService.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                    if (managingOrganization != null)
                    {
                        bankStatement.ManagingOrganization = managingOrganization;
                    }
                }
            }

            bankStatementService.Save(bankStatement);

            // 76 и 030101Р - Критерий определения спецсредств, к-е не должны учитываться
            foreach (var debet in record.DebetDocuments.Where(x => x != null && (x.DocClassId != "76" || x.OperKindLs != "030101Р")))
            {
                var debDocumentDate = debet.CompDocDate.ToDateTime();
                var bidDate = debet.DocumentDate.ToDateTime();
                var bidNum = !string.IsNullOrEmpty(debet.DocumentNumber) ? debet.DocumentNumber.Trim() : string.Empty;

                var inn = (debet.ContragentInn ?? string.Empty).Trim();
                var kpp = (debet.ContragentKpp ?? string.Empty).Trim();
                var name = debet.ContragentName ?? string.Empty;
                Contragent receiverContragent;
                if (string.IsNullOrEmpty(name))
                {
                    logFile.Warn(Name, string.Format("По выписке отсутствует контрагент {0} (ИНН: {1}, КПП: {2}).", name, inn, kpp));
                    receiverContragent = null;
                }
                else
                {
                    receiverContragent = !string.IsNullOrEmpty(kpp) ? contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp)
                        : contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && (x.Kpp == null || x.Kpp == kpp));
                }

                var outgoing = new PaymentOrderOut
                {
                    PayerContragent = this.GetContragentPayer(debet, logFile),
                    ReceiverContragent = receiverContragent,
                    DocumentDate = debDocumentDate != DateTime.MinValue ? debDocumentDate : bidDate,
                    DocumentNum = !string.IsNullOrEmpty(debet.CompDocNumber) ? debet.CompDocNumber.Trim() : bidNum,
                    BidDate = bidDate,
                    DocId = debet.DocumentId,
                    BidNum = bidNum,
                    PayPurpose = debet.Ground,
                    Sum = !string.IsNullOrEmpty(debet.Amount) ? (decimal?)debet.Amount.Trim().ToDecimal() : null,
                    BankStatement = bankStatement,
                    TypeFinanceSource = TypeFinanceSource.Not,
                    TypePaymentOrder = TypePaymentOrder.Out
                };

                paymentOrderOutService.Save(outgoing);

                logFile.Info(this.Name, string.Format("Добавлен расход банковской выписке c номером: {0}", bankStatement.DocumentNum), LogTypeChanged.Added);
            }

            foreach (var credit in record.CreditDocuments.Where(x => x != null && (x.DocClassId != "76" || x.OperKindLs != "030101Р")))
            {
                var credDocumentDate = credit.CompDocDate.ToDateTime();
                var bidDate = credit.DocumentDate.ToDateTime();
                var bidNum = !string.IsNullOrEmpty(credit.DocumentNumber) ? credit.DocumentNumber.Trim() : string.Empty;

                var inn = (credit.ContragentInn ?? string.Empty).Trim();
                var kpp = (credit.ContragentKpp ?? string.Empty).Trim();
                var name = credit.ContragentName ?? string.Empty;
                Contragent payerContragent;
                if (string.IsNullOrEmpty(name))
                {
                    logFile.Warn(Name, string.Format("По выписке отсутствует контрагент {0} (ИНН: {1}, КПП: {2}).", name, inn, kpp));
                    payerContragent = null;
                }
                else
                {
                    payerContragent = !string.IsNullOrEmpty(kpp) ? contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp) 
                        : contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && (x.Kpp == null || x.Kpp == kpp));
                }

                var incoming = new PaymentOrderIn
                {
                    PayerContragent = payerContragent,
                    ReceiverContragent = this.GetContragentReceiver(credit, logFile),
                    DocumentDate = credDocumentDate != DateTime.MinValue ? credDocumentDate : bidDate,
                    PayPurpose = credit.Ground,
                    TypeFinanceSource = GetTypeFinanceSourceByDict(credit.Kdr),
                    Sum = !string.IsNullOrEmpty(credit.Amount) ? credit.Amount.Trim().ToDecimal() : (decimal?)null,
                    DocumentNum = !string.IsNullOrEmpty(credit.CompDocNumber) ? credit.CompDocNumber.Trim() : bidNum,
                    BidDate = bidDate,
                    DocId = credit.DocumentId,
                    BidNum = bidNum,
                    BankStatement = bankStatement,
                    TypePaymentOrder = TypePaymentOrder.In
                };

                paymentOrderInService.Save(incoming);
                logFile.Info(Name, string.Format("Добавлен приход банковской выписке c номером: {0}", bankStatement.DocumentNum), LogTypeChanged.Added);
            }
        }

        /// <summary>
        /// Получить контрагента плательщика
        /// </summary>
        /// <param name="debet"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        private Contragent GetContragentPayer(DebetDocument debet, ILogImport logFile)
        {
            var inn = (debet.BudgReceiverInn ?? string.Empty).Trim();
            var kpp = (debet.BudgReceiverKpp ?? string.Empty).Trim();
            var name = debet.BudgReceiverName ?? string.Empty;
            var contragentService = Container.Resolve<IDomainService<Contragent>>();

            var managingOrgRealityObject = this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>().GetAll().FirstOrDefault(x => x.RealityObject.Id == this.objectCr.RealityObject.Id);
            if (managingOrgRealityObject != null)
            {
                var managingOrganization = managingOrgRealityObject.ManagingOrganization;
                if (managingOrganization.Contragent.Inn == inn && managingOrganization.Contragent.Kpp == kpp)
                {
                    return managingOrganization.Contragent;
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                logFile.Warn(this.Name, string.Format("По выписке отсутствует контрагент {0} (ИНН: {1}, КПП: {2}).", name, inn, kpp));
                return null;
            }

            return !string.IsNullOrEmpty(kpp) ? contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp)
                        : contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && (x.Kpp == null || x.Kpp == kpp));
        }

        /// <summary>
        /// Получить контрагента получателя
        /// </summary>
        /// <param name="credit"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        private Contragent GetContragentReceiver(CreditDocument credit, ILogImport logFile)
        {
            var inn = (credit.BudgReceiverInn ?? string.Empty).Trim();
            var kpp = (credit.BudgReceiverKpp ?? string.Empty).Trim();
            var name = credit.BudgReceiverName ?? string.Empty;
            var contragentService = Container.Resolve<IDomainService<Contragent>>();

            var managingOrgRealityObject = this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>().GetAll().FirstOrDefault(x => x.RealityObject.Id == this.objectCr.RealityObject.Id);
            var managingOrganization = new ManagingOrganization();

            if (managingOrgRealityObject != null)
            {
                managingOrganization = managingOrgRealityObject.ManagingOrganization;
            }

            if (managingOrganization != null)
            {
                if (managingOrganization.Contragent.Inn == inn && managingOrganization.Contragent.Kpp == kpp)
                {
                    return managingOrganization.Contragent;
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                logFile.Warn(Name, string.Format("По выписке отсутствует контрагент {0} (ИНН: {1}, КПП: {2}).", name, inn, kpp));
                return null;
            }

            return !string.IsNullOrEmpty(kpp) ? contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && x.Kpp == kpp)
                        : contragentService.GetAll().FirstOrDefault(x => x.Inn == inn && (x.Kpp == null || x.Kpp == kpp));
        }

        private IEnumerable<BankStatement> GetBankStatementRecursively(BankStatement bankStatement)
        {
            var result = new List<BankStatement> { bankStatement };

            // Ищем банковские выписки по объекту КР, лицевому счету и последнему дню операции по счету последний день операции по счету берем По Дате выписке bankStatemen
            var lastBankStatements = Container.Resolve<IDomainService<BankStatement>>().GetAll()
                         .Where(x => x.DocumentDate.HasValue)
                         .Where(x => x.ObjectCr.Id == objectCr.Id && x.OperLastDate.Value.Date == bankStatement.DocumentDate.Value.Date 
                             && x.PersonalAccount == bankStatement.PersonalAccount)
                         .ToArray();

            if (lastBankStatements.Length > 0)
            {
                result.AddRange(GetBankStatementRecursively(lastBankStatements[0]));
            }

            return result;
        }

        private bool СompareBankStatement(StatementDoc record, BankStatement bankStatement)
        {
            var documentNum = !string.IsNullOrEmpty(record.DocumentNumber) ? record.DocumentNumber.Trim() : string.Empty;
            var budgetYear = record.BudgetYear;

            var docDate = record.StatementDate.ToDateTime();
            var operLastDate = record.LastStatementDate.ToDateTime();

            var incomingBalance = record.OpeningBalance.ToDecimal();
            var outgoingBalance = record.ClosingBalance.ToDecimal();

            return documentNum == bankStatement.DocumentNum
                   && budgetYear == bankStatement.BudgetYear.ToStr()
                   && docDate == bankStatement.DocumentDate
                   && operLastDate == bankStatement.OperLastDate
                   && incomingBalance == bankStatement.IncomingBalance
                   && outgoingBalance == bankStatement.OutgoingBalance
                   && objectCr.Id == bankStatement.ObjectCr.Id
                   && this.CheckPaymentOrderByBankStat(record, bankStatement);
        }

        private bool CheckPaymentOrderByBankStat(StatementDoc bankStatementRecord, BankStatement bankStatement)
        {
            var incomingCount = 0;
            var incomingPaymentService = Container.Resolve<IDomainService<PaymentOrderIn>>();

            // 76 и 030101Р - Критерий определения спецсредств, которые не должны учитываться
            foreach (var tempIncoming in bankStatementRecord.CreditDocuments.Where(x => x != null).Where(x => x.DocClassId != "76" || x.OperKindLs != "030101Р"))
            {
                incomingCount++;
                var documentId = tempIncoming.DocumentId;

                var incomingPayOrder = incomingPaymentService.GetAll().FirstOrDefault(x => x.BankStatement.Id == bankStatement.Id && x.DocId == documentId);
                if (incomingPayOrder == null)
                {
                    return false;
                }

                if (!this.СomparePaymentOrder(tempIncoming, incomingPayOrder))
                {
                    return false;
                }
            }

            if (incomingPaymentService.GetAll().Count(x => x.BankStatement.Id == bankStatement.Id) != incomingCount)
            {
                return false;
            }

            var outgoingCount = 0;
            var outgoingPaymentService = Container.Resolve<IDomainService<PaymentOrderOut>>();
            foreach (var tempOutgoing in bankStatementRecord.DebetDocuments.Where(x => x != null).Where(x => x.DocClassId != "76" || x.OperKindLs != "030101Р"))
            {
                outgoingCount++;
                var documentId = tempOutgoing.DocumentId;

                var outgoingPayOrder = outgoingPaymentService.GetAll().FirstOrDefault(x => x.BankStatement.Id == bankStatement.Id && x.DocId == documentId);
                if (outgoingPayOrder == null)
                {
                    return false;
                }

                if (!this.СomparePaymentOrder(tempOutgoing, outgoingPayOrder))
                {
                    return false;
                }
            }

            return outgoingPaymentService.GetAll().Count(x => x.BankStatement.Id == bankStatement.Id) == outgoingCount;
        }

        private bool СomparePaymentOrder(DebetDocument record, PaymentOrderOut paymentOrder)
        {
            var bidNum = record.DocumentNumber;
            var bidDate = record.DocumentDate.ToDateTime();

            var docNum = !string.IsNullOrEmpty(record.CompDocNumber) ? record.CompDocNumber.Trim() : bidNum;
            var docDate = record.CompDocDate.ToDateTime() != DateTime.MinValue ? record.CompDocDate.ToDateTime() : bidDate;

            var sum = record.Amount.ToDecimal();
            return docNum == paymentOrder.DocumentNum
                   && docDate == paymentOrder.DocumentDate
                   && bidNum == paymentOrder.BidNum
                   && bidDate == paymentOrder.BidDate
                   && sum == paymentOrder.Sum;
        }

        private bool СomparePaymentOrder(CreditDocument record, PaymentOrderIn paymentOrder)
        {
            var bidNum = record.DocumentNumber;
            var bidDate = record.DocumentDate.ToDateTime();

            var docNum = !string.IsNullOrEmpty(record.CompDocNumber) ? record.CompDocNumber.Trim() : bidNum; 
            var docDate = record.CompDocDate.ToDateTime() != DateTime.MinValue ? record.CompDocDate.ToDateTime() : bidDate;
            var finSource = GetTypeFinanceSourceByDict(record.Kdr);

            var sum = record.Amount.ToDecimal();

            return docNum == paymentOrder.DocumentNum
                   && docDate == paymentOrder.DocumentDate
                   && bidNum == paymentOrder.BidNum
                   && bidDate == paymentOrder.BidDate
                   && sum == paymentOrder.Sum
                   && finSource == paymentOrder.TypeFinanceSource;
        }

        /// <summary>
        /// Вернуть тип источника финансирования
        /// </summary>
        /// <param name="code">
        /// </param>
        /// <returns>
        /// </returns>
        private TypeFinanceSource GetTypeFinanceSourceByDict(string code)
        {
            // Ищем разрезы финансирования муниципального образование. Их наличие говорит о том что используются не стандартные коды в поле Kdr
            var finSourcesMo = Container.Resolve<IDomainService<MunicipalitySourceFinancing>>().GetAll()
                         .Where(x => x.Municipality.Id == objectCr.RealityObject.Municipality.Id)
                         .ToArray();

            if (finSourcesMo.Length > 0)
            {
                var standartCode = string.Empty;

                // По полю Доп.КР находит источник и у него берем Разрез финансирования. Возможны изменения например вместо поля AddKr может использоваться другое поле
                var finSourceMo = finSourcesMo.FirstOrDefault(x => x.AddKr == code);
                if (finSourceMo != null)
                {
                    standartCode = finSourceMo.SourceFinancing;
                }

                switch (standartCode)
                {
                    case "301":
                        return TypeFinanceSource.FederalFunds;

                    case "302":
                        return TypeFinanceSource.SubjectBudgetFunds;

                    case "303":
                        return TypeFinanceSource.PlaceBudgetFunds;

                    case "304":
                        return TypeFinanceSource.OccupantFunds;
                }
            }

            // Если дошли до этого места, значит  в поле Kdr используются стандартные коды (301,302,303,304)
            switch (code)
            {
                case "301":
                    return TypeFinanceSource.FederalFunds;

                case "302":
                    return TypeFinanceSource.SubjectBudgetFunds;

                case "303":
                    return TypeFinanceSource.PlaceBudgetFunds;

                case "304":
                    return TypeFinanceSource.OccupantFunds;

                default:
                    return TypeFinanceSource.NotSet;
            }
        }
    }
}
