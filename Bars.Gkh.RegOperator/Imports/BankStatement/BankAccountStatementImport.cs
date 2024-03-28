namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.BankStatement;
    using global::Quartz.Util;

    /// <summary>
    /// Импорт банковских выписок
    /// </summary>
    public class BankAccountStatementImport : GkhImportBase
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties & Services

        /// <summary>
        /// Запись
        /// </summary>
        public class Record
        {
            /// <summary>
            /// Ключ
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Дата
            /// </summary>
            public string DocumentDate { get; set; }

            /// <summary>
            /// Номер документа
            /// </summary>
            public string DocumentNum { get; set; }

            /// <summary>
            /// Номер личного счета
            /// </summary>
            public string AccountNum { get; set; }

        }

        /// <summary>
        /// Словарь соответствий полям файла полям класса посредника
        /// </summary>
        private readonly Dictionary<string, string> headersBankAccountStatement = new Dictionary<string, string>()
        {
            {"дата", "DocumentDate"},
            {"датапоступило", "DateReceipt"},
            {"датасписано", "DateReceipt"},
            {"номер", "DocumentNum"},
            {"плательщик", "PayerFull"},
            {"плательщик1", "PayerName"},
            {"получатель1", "RecipientName"},
            {"плательщиксчет", "PayerAccountNum"},
            {"получательсчет", "RecipientAccountNum"},
            {"сумма", "Sum"},
            {"назначениеплатежа", "PaymentDetails"},
            {"получательинн", "RecipientInn"},
            {"получательбик", "RecipientBik"},
            {"получателькорсчет", "RecipientCorrAccount"},
            {"получательбанк1", "RecipientBank"},
            {"получателькпп", "RecipientKpp"},
            {"плательщикинн", "PayerInn"},
            {"плательщикбик", "PayerBik"},
            {"плательщиккорсчет", "PayerCorrAccount"},
            {"плательщикбанк1", "PayerBank"},
            {"плательщиккпп", "PayerKpp"}
        };

        /// <summary>
        /// Словарь соответствий для чтения информации о расчетных счетах
        /// </summary>
        private readonly Dictionary<string, string> headersBankAccountBaseInfo = new Dictionary<string, string>()
        {
            {"датаначала", "StartDate"},
            {"датаконца", "EndDate"},
            {"начальныйостаток", "StartBalance"},
            {"расчсчет", "AccountNum"},
            {"всегосписано", "AllWrittenOff"},
            {"всегопоступило", "AllReceived"},
            {"конечныйостаток", "EndBalance"}
        };

        /// <summary>
        /// Коллекция для сохранения
        /// </summary>
        private readonly List<BankAccountStatement> bankAccountStatementsToSave = new List<BankAccountStatement>();

        /// <summary>
        /// Коллекция контрагентов
        /// </summary>
        private Dictionary<string, Contragent> contragentsDict;

        /// <summary>
        /// Коллекция существующих записей для проверки
        /// </summary>
        private Dictionary<string, Record> existingRecords;

        /// <summary>
        /// Ключ
        /// </summary>
        public override string Key
        {
            get { return BankAccountStatementImport.Id; }
        }

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport
        {
            get { return "BankAccountStatementImport"; }
        }

        /// <summary>
        /// Название
        /// </summary>
        public override string Name
        {
            get { return "Импорт банковских выписок"; }
        }

        /// <summary>
        /// Возможные расширения файла
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Имя разрешений
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.BankAccountStatementImport"; }
        }

        #endregion

        /// <summary>
        /// Проверка файла
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат проверки</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var file = baseParams.Files["FileImport"];

            using (var fileMemoryStream = new MemoryStream(file.Data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.GetEncoding("utf-8")))
                {
                    var line1 = reader.ReadLine().ToStr().Trim(); // 1CClientBankExchange
                    if (line1 != "1CClientBankExchange")
                    {
                        message = "Невозможно прочитать файл. Неверный формат файла";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Импорт банковских выписок
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            try
            {
                var file = baseParams.Files["FileImport"];
                var outcomesOnly = baseParams.Params.GetAs<bool>("outcomesOnlyParam", false);
                var withoutRegister = baseParams.Params.GetAs<bool>("withoutRegisterParam", false);
                var distributeOnPenalty = baseParams.Params.GetAs<bool>("distributeOnPenaltyParam", false);

                this.InitLog(file.FileName);

                this.InitDictionaries();
                try
                {
                    this.ProcessData(file.Data);
                }
                catch (Exception ex)
                {
                    this.LogImport.Error("Ошибка", "Не удалось осуществить импорт:" + ex.Message);
                }

                if (outcomesOnly)
                {
                    this.SaveOutcomesData(file);
                }
                else
                {
                    if (withoutRegister)
                    {
                        this.SaveWithoutRegister(file);
                    }
                    else
                    {
                        this.SaveData(file, distributeOnPenalty);
                    }
                }

                this.LogImportManager.Add(file, this.LogImport);
                this.LogImportManager.Save();

                var status = this.LogImportManager.CountError > 0
                    ? StatusImport.CompletedWithError
                    : (this.LogImportManager.CountWarning > 0
                        ? StatusImport.CompletedWithWarning
                        : StatusImport.CompletedWithoutError);

                return new ImportResult(status, this.LogImportManager.GetInfo(), string.Empty, this.LogImportManager.LogFileId);
            }
            finally
            {
                this.Container.Release(this.LogImport);
            }
        }

        private void InitDictionaries()
        {
            var contragentRepo = this.Container.ResolveRepository<Contragent>();
            var bankAccStatementDomain = this.Container.ResolveDomain<BankAccountStatement>();

            using (this.Container.Using(contragentRepo, bankAccStatementDomain))
            {
                this.contragentsDict = contragentRepo.GetAll()
                    .Where(x => x.ContragentState != ContragentState.Bankrupt)
                    .Where(x => x.ContragentState != ContragentState.Liquidated)
                    .Select(x => new { Contragent = x, x.Inn, x.Kpp })
                    .ToList()
                    .Where(x => !x.Inn.IsNullOrWhiteSpace() && !x.Kpp.IsNullOrWhiteSpace())
                    .Select(x => new
                    {
                        x.Contragent,
                        mixedkey = string.Format(
                            "{0}#{1}",
                            x.Inn.Trim(),
                            x.Kpp.Trim()).ToLower()
                    })
                    .GroupBy(x => x.mixedkey)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Contragent).First());

                this.existingRecords = bankAccStatementDomain.GetAll()
                    .Where(x => x.DistributeState != DistributionState.Deleted)
                    .Select(x => new
                    {
                        x.DocumentDate,
                        x.DocumentNum,
                        AccountNum = x.RecipientAccountNum
                    })
                    .ToList()
                    .Select(x => new Record
                    {
                        Key = string.Format("{0}#{1}#{2}", x.DocumentDate.ToShortDateString(), x.DocumentNum, x.AccountNum),
                        DocumentDate = x.DocumentDate.ToShortDateString(),
                        DocumentNum = x.DocumentNum,
                        AccountNum = x.AccountNum
                    })
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.First());
            }
        }

        /// <summary>
        /// Обрабатываем данные
        /// </summary>
        /// <param name="data">Данные</param>
        private void ProcessData(byte[] data)
        {
            // сначала получаем базовую информацию по счета прихода ухода
            var bankAccountsBaseInfo = this.ReadBaseAccountInfo(data);

            // далее получаем информацию о выписках
            var bankAccountsStatement = this.ReadBankAccountStatement(data);

            var transferSuccess = true;
            try
            {
                foreach (var bankAccountProxyCharge in bankAccountsStatement)
                {
                    var bankAccStatement = new BankAccountStatement
                    {
                        OperationDate = DateTime.Today,
                        DistributeState = DistributionState.NotDistributed,
                        IsROSP = false,
                        IsDistributable = YesNo.Yes
                    };

                    foreach (var headers in this.headersBankAccountStatement)
                    {
                        // откуда берем
                        var bankAccountProxyChargePropertyInfo = bankAccountProxyCharge.GetType().GetProperty(headers.Value);

                        var value = bankAccountProxyChargePropertyInfo.GetValue(bankAccountProxyCharge, null);

                        // куда кладем
                        var bankAccStatementPropertyInfo = bankAccStatement.GetType().GetProperty(headers.Value);

                        if (bankAccStatementPropertyInfo != null)
                        {
                            bankAccStatementPropertyInfo.SetValue(bankAccStatement, value, null);
                        }
                    }

                    bankAccountProxyCharge.BankStatement = bankAccStatement;
                    this.bankAccountStatementsToSave.Add(bankAccStatement);
                }
            }
            catch (Exception ex)
            {
                // что-то не сопоставилось
                this.LogImport.Error("Ошибка", "Описание:" + ex.Message);
                transferSuccess = false;
            }

            // если все норм сопоставилось, то можно начинать перенос
            if (transferSuccess)
            {
                foreach (var bankAccountProxyCharge in bankAccountsStatement)
                {
                    // вообще сомневаюсь, что здесь может быть null
                    // но пусть останется проверка
                    if (bankAccountProxyCharge.BankStatement.IsNotNull())
                    {
                        var bankStatement = bankAccountProxyCharge.BankStatement;

                        if (bankAccountsBaseInfo.Any(x => x.AccountNum == bankAccountProxyCharge.PayerAccountNum))
                        {
                            // нашли себя как плательщик
                            bankStatement.MoneyDirection = MoneyDirection.Outcome;
                        }
                        else if (bankAccountsBaseInfo.Any(x => x.AccountNum == bankAccountProxyCharge.RecipientAccountNum))
                        {
                            // нашли себя как получатель
                            bankStatement.MoneyDirection = MoneyDirection.Income;
                        }
                    }
                    else
                    {
                        this.LogImport.Error("Ошибка", "Не найдена банковская выписка с номером: " + bankAccountProxyCharge.DocumentNum);
                    }
                }
            }
            else
            {
                this.LogImport.Error("Результат работы импорта", "Не выполнен из-за ошибок! Возможно изменились имена полей.");
            }

            
        }


        private List<BankAccountBaseInfoProxyCharge> ReadBaseAccountInfo(byte[] data)
        {
            var bankAccountBaseInfoProxyList = new List<BankAccountBaseInfoProxyCharge>();
            var settlAccSectionFound = false;
            using (var fileMemoryStream = new MemoryStream(data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        // отсекаем все до секциядокумент там вроде все равно ничего интересного
                        settlAccSectionFound = settlAccSectionFound || line.ToLower().StartsWith("секциярасчсчет");
                        if (!settlAccSectionFound)
                        {
                            continue;
                        }

                        var bankAccountBaseInfo = new BankAccountBaseInfoProxyCharge();
                        while (!reader.EndOfStream && line.ToLower() != "конецрасчсчет")
                        {
                            if (!line.Contains("="))
                            {
                                line = reader.ReadLine();
                                continue;
                            }

                            var pair = line.Split('=');

                            if (this.headersBankAccountBaseInfo.ContainsKey(pair[0].ToLower()) && !string.IsNullOrEmpty(pair[1]))
                            {
                                var propertyName = this.headersBankAccountBaseInfo[pair[0].ToLower()];
                                var propertyInfo = bankAccountBaseInfo.GetType().GetProperty(propertyName);

                                switch (propertyInfo.PropertyType.FullName.ToLower())
                                {
                                    case "system.string":
                                        propertyInfo.SetValue(bankAccountBaseInfo, pair[1], null);
                                        break;
                                    case "system.decimal":
                                        var decimalValue = Convert.ToDecimal(pair[1], new CultureInfo("en-US"));
                                        propertyInfo.SetValue(bankAccountBaseInfo, decimalValue, null);
                                        break;
                                    default:

                                        // так делаем потому как fullname дополняется инфой при использовании nullable
                                        if (propertyInfo.PropertyType.FullName.ToLower().Contains("system.datetime"))
                                        {
                                            DateTime value;
                                            if (DateTime.TryParseExact(pair[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                                            {
                                                propertyInfo.SetValue(bankAccountBaseInfo, value, null);
                                            }
                                        }
                                        else if (propertyInfo.PropertyType.FullName.ToLower().Contains("system.decimal"))
                                        {
                                            var value = Convert.ToDecimal(pair[1], new CultureInfo("en-US"));
                                            propertyInfo.SetValue(bankAccountBaseInfo, value, null);
                                        }

                                        break;
                                }
                            }

                            line = reader.ReadLine();
                        }

                        bankAccountBaseInfoProxyList.Add(bankAccountBaseInfo);
                    }
                }
            }

            return bankAccountBaseInfoProxyList;
        }


        /// <summary>
        /// Читаем данные о банковских выписках
        /// </summary>
        /// <param name="data">Входные данные</param>
        /// <returns>Набор накопленных данных о банковских выписках</returns>
        private List<BankAccountStatementProxyCharge> ReadBankAccountStatement(byte[] data)
        {
            var bankAccounts = new List<BankAccountStatementProxyCharge>();
            var settlAccSectionFound = false;
            using (var fileMemoryStream = new MemoryStream(data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        // отсекаем все до секциядокумент там вроде все равно ничего интересного
                        settlAccSectionFound = settlAccSectionFound || line.ToLower().StartsWith("секциядокумент");
                        if (!settlAccSectionFound)
                        {
                            continue;
                        }

                        var bankAccount = new BankAccountStatementProxyCharge();
                        var filled = false;
                        while (!reader.EndOfStream && line.ToLower() != "конецдокумента")
                        {
                            if (!line.Contains("="))
                            {
                                line = reader.ReadLine();
                                continue;
                            }

                            var pare = line.Split('=');
                          //  Только для челябинска
                            if (this.headersBankAccountStatement.ContainsKey(pare[0].ToLower()) && string.IsNullOrEmpty(pare[1]))
                            {
                                if (pare[0].ToLower() == "получательинн")
                                {
                                    pare[1] = "7451990794";
                                }
                            }
                            if (this.headersBankAccountStatement.ContainsKey(pare[0].ToLower()) && !string.IsNullOrEmpty(pare[1]))
                            {
                                var propertyName = this.headersBankAccountStatement[pare[0].ToLower()];
                                var propertyInfo = bankAccount.GetType().GetProperty(propertyName);
                                filled = true;
                                switch (propertyInfo.PropertyType.FullName.ToLower())
                                {
                                    case "system.string":
                                        propertyInfo.SetValue(bankAccount, pare[1], null);
                                        break;
                                    case "system.decimal":
                                        var decimalValue = Convert.ToDecimal(pare[1], new CultureInfo("en-US"));
                                        propertyInfo.SetValue(bankAccount, decimalValue, null);
                                        break;
                                    case "system.datetime":
                                        DateTime dateValue;
                                        DateTime.TryParseExact(pare[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue);
                                        propertyInfo.SetValue(bankAccount, dateValue, null);
                                        break;
                                    default:

                                        // так делаем потому как fullname дополняется инфой при использовании nullable
                                        if (propertyInfo.PropertyType.FullName.ToLower().Contains("system.datetime"))
                                        {
                                            DateTime value;
                                            if (DateTime.TryParseExact(pare[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
                                            {
                                                propertyInfo.SetValue(bankAccount, value, null);
                                            }
                                        }
                                        else if (propertyInfo.PropertyType.FullName.ToLower().Contains("system.decimal"))
                                        {
                                            var value = Convert.ToDecimal(pare[1], new CultureInfo("en-US"));
                                            propertyInfo.SetValue(bankAccount, value, null);
                                        }

                                        break;
                                }
                            }

                            line = reader.ReadLine();
                        }

                        if (filled && !this.IsErrorBankAccount(bankAccount))
                        {
                            bankAccounts.Add(bankAccount);
                        }
                    }
                }
            }

            return bankAccounts;
        }

        /// <summary>
        /// Проверяем собранные данные
        /// </summary>
        /// <param name="bankAccountProxyCharge">Объект посредник для проверки</param>
        /// <returns>Вернем false если не заполнены необходимые поля</returns>
        private bool IsErrorBankAccount(BankAccountStatementProxyCharge bankAccountProxyCharge)
        {
            var error = false;

            var docNum = bankAccountProxyCharge.DocumentNum ?? "Номер документа отсутствует в выписке!";

            if (!bankAccountProxyCharge.DocumentDate.HasValue)
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "Дата");
            }

            if (!bankAccountProxyCharge.DateReceipt.HasValue)
            {
                error = true;
                this.LogImport.Warn("Документ: " + docNum + "." + string.Format("Параметр {0}", "ДатаПоступило/ДатаСписано "), "Поле ДатаПоступило/ДатаСписано пусто или некорректно");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.DocumentNum))
            {
                error = true;
                this.LogImport.Warn(string.Format("Параметр {0}", "Номер"), "Номер пустой или некорректный");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerAccountNum))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПлательщикСчет");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.RecipientAccountNum))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПолучательСчет");
            }

            var shortDate = bankAccountProxyCharge.DocumentDate.HasValue
                ? bankAccountProxyCharge.DocumentDate.Value.ToShortDateString()
                : string.Empty;

            var existRecKey = string.Format("{0}#{1}#{2}",
                            shortDate,
                            bankAccountProxyCharge.DocumentNum,
                            bankAccountProxyCharge.RecipientAccountNum);

            if (this.existingRecords.ContainsKey(existRecKey))
            {
                error = true;
                this.LogImport.Warn("Внимание!",
                    string.Format("В системе уже имеется документ с датой {0}, номером документа {1}, Р/С получателя {2}. Данные не загружены.",
                    shortDate,
                    bankAccountProxyCharge.DocumentNum,
                    bankAccountProxyCharge.RecipientAccountNum));
            }

            if (!bankAccountProxyCharge.Sum.HasValue)
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "Сумма");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PaymentDetails))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "НазначениеПлатежа");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.RecipientInn))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПолучательИНН");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.RecipientBik))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПолучательБИК");
            }

            // получателькорсчет - необязательный
            if (string.IsNullOrEmpty(bankAccountProxyCharge.RecipientCorrAccount))
            {
                this.LogImport.Warn("Документ: " + docNum + "." + string.Format("Параметр {0}", "ПолучательКорсчет"), "Поле ПолучательКорсчет  пусто или некорректно");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.RecipientBank))
            {
              //  error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПолучательБанк1");
            }

            if(!string.IsNullOrEmpty(bankAccountProxyCharge.Recipient) && !this.contragentsDict.ContainsKey(bankAccountProxyCharge.Recipient))
            {
                this.LogImport.Warn("Документ: " + bankAccountProxyCharge.DocumentNum + "." + "Внимание!",
                                string.Format("Не найден контрагент по указанным ИНН {0} и КПП {1}", bankAccountProxyCharge.RecipientInn, bankAccountProxyCharge.RecipientKpp));
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerInn))
            {
                this.LogImport.Warn(string.Format("Документ: {0}. Параметр {1}", docNum, "ПлательщикИНН"), "Поле ПлательщикИНН пусто или некорректно");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerBik))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПлательщикБИК");
            }

            // плательщиккорсчет - необязательный
            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerCorrAccount))
            {
                this.LogImport.Warn("Документ: " + docNum + "." + string.Format("Параметр {0}", "ПлательщикКорсчет"), "Поле ПлательщикКорсчет пусто или некорректно");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerBank))
            {
                error = true;
                this.LogRequiredParamNotFoundError(docNum, "ПлательщикБанк1");
            }

            if (string.IsNullOrEmpty(bankAccountProxyCharge.PayerKpp))
            {
                this.LogImport.Warn(string.Format("Документ: {0}. Параметр {1}", docNum, "ПлательщикКПП"), "Поле ПлательщикКПП пусто или некорректно");
            }

            if (!this.contragentsDict.ContainsKey(bankAccountProxyCharge.Payer))
            {
                this.LogImport.Warn("Документ: " + docNum + "." + "Внимание!",
                                string.Format("Не найден контрагент по указанным ИНН {0} и КПП {1}", bankAccountProxyCharge.PayerInn, bankAccountProxyCharge.PayerKpp));
            }

            return error;
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="distributeOnPenalty"></param>
        private void SaveData(FileData file, bool distributeOnPenalty)
        {
            var identity = this.Container.Resolve<IUserIdentity>();
            var userDomain = this.Container.ResolveDomain<User>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var bankAccStatementRepo = this.Container.ResolveDomain<BankAccountStatement>();
            var bankDocImportDomain = this.Container.Resolve<IDomainService<Entities.BankDocumentImport>>();
            var importedPaymentDomain = this.Container.Resolve<IDomainService<ImportedPayment>>();
            var persAccDomain = this.Container.Resolve<IDomainService<BasePersonalAccount>>();
            var logImportDomain = this.Container.Resolve<IDomainService<LogImport>>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var stateHistoryManager = this.Container.Resolve<IGeneralStateHistoryManager>();
            var importedPaymentsToSave = new List<ImportedPayment>();

            var bankDocImport = new RegOperator.Entities.BankDocumentImport
            {
                ImportDate = DateTime.Now,
                DocumentNumber = file.FileName,
                DocumentType = "Импорт из Банковских операций",
                Status = BankDocumentImportStatus.SuccessfullyImported,
                PersonalAccountDeterminationState = PersonalAccountDeterminationState.Defined,
                PaymentConfirmationState = PaymentConfirmationState.NotDistributed,
                ImportType = "Импорт из Банковских операций"
            };

            var logImportForBankDoc = new LogImport
            {
                Operator = userManager.GetActiveOperator(),
                Login = userManager.GetActiveOperator().Login ?? "anonymus",
                UploadDate = DateTime.Now,
                FileName = file.FileName,
                ImportKey = "Импорт из Банковских операций"
            };

            using (this.Container.Using(identity, userDomain, bankAccStatementRepo, fileManager, stateHistoryManager))
            {
                var fileInfo = fileManager.SaveFile(file);
                stateHistoryManager.Init(typeof(BankAccountStatement));
                List<string> foundLs = new List<string>();
                string lsNum = "";
                string details = "";
                string tryLs = "";
                string[] detailsSplit;
                bool isOneLsMultiple = true;

                this.Container.InTransaction(() =>
                {
                    var user = userDomain.FirstOrDefault(x => x.Id == identity.UserId);
                    foreach (var bankAccountStatement in this.bankAccountStatementsToSave)
                    {
                        bankAccountStatement.UserLogin = user == null ? "anonymus" : user.Login;

                        if (file.Data != null)
                        {
                            bankAccountStatement.File = fileInfo;
                        }

                        if (bankAccountStatement.MoneyDirection == MoneyDirection.Income)
                        {
                            lsNum = "";
                            foundLs.Clear();

                            details = bankAccountStatement.PaymentDetails
                            .Replace('-', ' ')
                            .Replace(';', ' ')
                            .Replace('.', ' ')
                            .Replace(',', ' ')
                            .Replace('/', ' ');
                            detailsSplit = details.Split(' ');

                            foreach (string splitted in detailsSplit)
                            {
                                if (splitted.Length >= 9)
                                {
                                    tryLs = Regex.Replace(splitted, @"[^\d]", "");
                                    if (tryLs.Length == 9)
                                    {
                                        if (persAccDomain.GetAll().Where(x => x.PersonalAccountNum == tryLs).FirstOrDefault() != null)
                                        {
                                            foundLs.Add(tryLs);
                                        }
                                    }
                                }
                            }

                            if (foundLs.Count == 1)
                            {
                                lsNum = foundLs[0];
                            }
                            else if (foundLs.Count > 1)
                            {
                                isOneLsMultiple = true;
                                foreach (string ls in foundLs)
                                {
                                    if (ls != foundLs[0])
                                    {
                                        isOneLsMultiple = false;
                                    }
                                }

                                if (isOneLsMultiple)
                                {
                                    lsNum = foundLs[0];
                                }
                            }

                            if (lsNum != "")
                            {
                                var persAcc = persAccDomain.GetAll().Where(x => x.PersonalAccountNum == lsNum).FirstOrDefault();
                                var importedPayment = new ImportedPayment
                                {
                                    Account = lsNum,
                                    ExternalAccountNumber = lsNum,
                                    PersonalAccount = persAcc,
                                    ReceiverNumber = bankAccountStatement.RecipientAccountNum,
                                    OwnerByImport = bankAccountStatement.PayerName,
                                    FactReceiverNumber = bankAccountStatement.RecipientAccountNum,
                                    PaymentType = ImportedPaymentType.Payment,
                                    Sum = bankAccountStatement.Sum,
                                    PaymentDate = bankAccountStatement.DocumentDate,
                                    PaymentState = ImportedPaymentState.Rno,
                                    Accepted = false,
                                    PersonalAccountDeterminationState = ImportedPaymentPersAccDeterminateState.Defined,
                                    PaymentConfirmationState = ImportedPaymentPaymentConfirmState.NotDistributed,
                                    IsDeterminateManually = false
                                };

                                importedPaymentsToSave.Add(importedPayment);
                            }
                            else
                            {
                                bankAccStatementRepo.Save(bankAccountStatement);

                                stateHistoryManager.CreateStateHistory(bankAccountStatement, null, DistributionState.NotDistributed);

                                this.LogImport.Info(string.Format(
                                    "Документ №{0} от {1} загружен, р/с получателя {2} ",
                                    bankAccountStatement.DocumentNum,
                                    bankAccountStatement.DocumentDate,
                                    bankAccountStatement.RecipientAccountNum),
                                    "Успешно");
                                this.LogImport.CountAddedRows++;
                            }
                        }
                        else
                        {
                            bankAccStatementRepo.Save(bankAccountStatement);

                            stateHistoryManager.CreateStateHistory(bankAccountStatement, null, DistributionState.NotDistributed);

                            this.LogImport.Info(string.Format(
                                "Документ №{0} от {1} загружен, р/с получателя {2} ",
                                bankAccountStatement.DocumentNum,
                                bankAccountStatement.DocumentDate,
                                bankAccountStatement.RecipientAccountNum),
                                "Успешно");
                            this.LogImport.CountAddedRows++;
                        }
                    }

                    if (importedPaymentsToSave.Count > 0)
                    {
                        logImportForBankDoc.File = fileInfo;
                        logImportDomain.Save(logImportForBankDoc);

                        var lastLogImport = logImportDomain.GetAll().OrderByDescending(x => x.Id).First();

                        bankDocImport.LogImport = lastLogImport;
                        bankDocImport.ImportedSum = importedPaymentsToSave.SafeSum(x => x.Sum);
                        bankDocImport.DistributePenalty = distributeOnPenalty ? YesNo.Yes : YesNo.No;
                        bankDocImportDomain.Save(bankDocImport);

                        var lastBankDocImport = bankDocImportDomain.GetAll().OrderByDescending(x => x.Id).First();

                        foreach (var importedPayment in importedPaymentsToSave)
                        {
                            importedPayment.BankDocumentImport = lastBankDocImport;
                            importedPaymentDomain.Save(importedPayment);
                        }
                    }

                    stateHistoryManager.SaveStateHistories();
                });
            }
        }

        /// <summary>
        /// Сохранение только расходов
        /// </summary>
        /// <param name="file">Файл</param>
        private void SaveOutcomesData(FileData file)
        {
            var identity = this.Container.Resolve<IUserIdentity>();
            var userDomain = this.Container.ResolveDomain<User>();
            var bankAccStatementRepo = this.Container.ResolveDomain<BankAccountStatement>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var stateHistoryManager = this.Container.Resolve<IGeneralStateHistoryManager>();

            using (this.Container.Using(identity, userDomain, bankAccStatementRepo, fileManager, stateHistoryManager))
            {
                var fileInfo = fileManager.SaveFile(file);
                stateHistoryManager.Init(typeof(BankAccountStatement));

                this.Container.InTransaction(() =>
                {
                    var user = userDomain.FirstOrDefault(x => x.Id == identity.UserId);
                    foreach (var bankAccountStatement in this.bankAccountStatementsToSave)
                    {
                        bankAccountStatement.UserLogin = user == null ? "anonymus" : user.Login;

                        if (file.Data != null)
                        {
                            bankAccountStatement.File = fileInfo;
                        }

                        if (bankAccountStatement.MoneyDirection != MoneyDirection.Income)
                        {
                            bankAccStatementRepo.Save(bankAccountStatement);

                            stateHistoryManager.CreateStateHistory(bankAccountStatement, null, DistributionState.NotDistributed);

                            this.LogImport.Info(string.Format(
                                "Документ №{0} от {1} загружен, р/с получателя {2} ",
                                bankAccountStatement.DocumentNum,
                                bankAccountStatement.DocumentDate,
                                bankAccountStatement.RecipientAccountNum),
                                "Успешно");
                            this.LogImport.CountAddedRows++;
                        }
                    }

                    stateHistoryManager.SaveStateHistories();
                });
            }
        }

        /// <summary>
        /// Сохранение без реестра платежных агентов 
        /// </summary>
        /// <param name="file">Файл</param>
        private void SaveWithoutRegister(FileData file)
        {
            var identity = this.Container.Resolve<IUserIdentity>();
            var userDomain = this.Container.ResolveDomain<User>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var bankAccStatementRepo = this.Container.ResolveDomain<BankAccountStatement>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var stateHistoryManager = this.Container.Resolve<IGeneralStateHistoryManager>();

            using (this.Container.Using(identity, userDomain, bankAccStatementRepo, fileManager, stateHistoryManager))
            {
                var fileInfo = fileManager.SaveFile(file);
                stateHistoryManager.Init(typeof(BankAccountStatement));

                this.Container.InTransaction(() =>
                {
                    var user = userDomain.FirstOrDefault(x => x.Id == identity.UserId);
                    foreach (var bankAccountStatement in this.bankAccountStatementsToSave)
                    {
                        bankAccountStatement.UserLogin = user == null ? "anonymus" : user.Login;

                        if (file.Data != null)
                        {
                            bankAccountStatement.File = fileInfo;
                        }

                        bankAccStatementRepo.Save(bankAccountStatement);

                        stateHistoryManager.CreateStateHistory(bankAccountStatement, null, DistributionState.NotDistributed);

                        this.LogImport.Info(string.Format(
                            "Документ №{0} от {1} загружен, р/с получателя {2} ",
                            bankAccountStatement.DocumentNum,
                            bankAccountStatement.DocumentDate,
                            bankAccountStatement.RecipientAccountNum),
                            "Успешно");
                        this.LogImport.CountAddedRows++;
                    }

                    stateHistoryManager.SaveStateHistories();
                });
            }
        }

        private void LogRequiredParamNotFoundError(string param, string docNumber)
        {
            this.LogImport.Error("Документ: {0}. Параметр: {1}".FormatUsing(param, docNumber), "Параметр '{0}' отсутствует в исходном файле".FormatUsing(param));
        }
    }
}