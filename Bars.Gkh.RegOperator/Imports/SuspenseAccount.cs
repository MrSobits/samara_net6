namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;
    using Import.Impl;

    public class SuspenseAccount : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        private class Record
        {
            // Строка начала секции документа
            public int RowNum;

            public string DocumentNum;

            public DateTime? Date;

            public decimal? Sum;

            public string PayerAccountNum;

            public string ReceiverAccount;

            public string Purpose;
        }

        private List<Record> records = new List<Record>();

        private string account = string.Empty;

        private CultureInfo culture = CultureInfo.CreateSpecificCulture("ru-RU");

        public virtual IWindsorContainer Container { get; set; }

        private readonly IRepository<Entities.SuspenseAccount> _suspenseAccountRepository;
        private readonly IRepository<BankAccountStatement> _bankAccountStatementRepo;
        private readonly IRepository<BankAccountStatementGroup> _bankAccountStatementGroupRepo;

        public ILogImportManager LogManager { get; set; }

        private ILogImport logImport;

        private BankAccountStatementGroup _statementGroup;

        public SuspenseAccount(IRepository<Entities.SuspenseAccount> suspenseAccountRepository,
            IRepository<BankAccountStatement> bankAccountStatementRepo,
            IRepository<BankAccountStatementGroup> bankAccountStatementGroupRepo)
        {
            _suspenseAccountRepository = suspenseAccountRepository;
            _bankAccountStatementRepo = bankAccountStatementRepo;
            _bankAccountStatementGroupRepo = bankAccountStatementGroupRepo;
        }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "SuspenseAccountFrom1C"; }
        }

        public override string Name
        {
            get { return "Импорт в счета НВС из 1С"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "txt"; }
        }

        public override string PermissionName
        {
            get { return "Import.SuspenseAccountFrom1C"; }
        }

        #endregion Properties

        public void InitLog(string fileName)
        {
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;

            logImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            logImport.SetFileName(fileName);
            logImport.ImportKey = Key;
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var errorList = new List<string>();

            var file = baseParams.Files["FileImport"];
            var extention = file.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            using (var fileMemoryStream = new MemoryStream(file.Data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.GetEncoding("utf-8")))
                {
                    var line1 = reader.ReadLine().ToStr().Trim(); // 1CClientBankExchange
                    if (line1 != "1CClientBankExchange")
                    {
                        errorList.Add("Неизвестный формат обмена данными:" + line1);
                    }
                }
            }

            message = string.Join("; ", errorList);

            if (!string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            InitLog(file.FileName);

            this.ProcessData(file.Data);

            this.SaveData();

            LogManager.Add(file, logImport);
            LogManager.Save();

            var status = LogManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, LogManager.GetInfo(), string.Empty, LogManager.LogFileId);
        }
        
        private void ProcessData(byte[] data)
        {
            _statementGroup = new BankAccountStatementGroup();

            using (var fileMemoryStream = new MemoryStream(data))
            {
                using (var reader = new StreamReader(fileMemoryStream, Encoding.UTF8))
                {
                    var lines = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                    var splitChars = new[] { '=' };
                    var dict = lines
                        .Where(s => s.Contains("="))
                        .Select(s => s.Split(splitChars, 2))
                        .Select(s => new
                        {
                            Name = s[0].Trim().ToStr().ToLower(),
                            Value = s[1].Trim().ToStr().ToLower()
                        })
                        .Where(s => s.Name != string.Empty)
                        .GroupBy(s => s.Name)
                        .ToDictionary(s => s.Key, d => d.Select(s => s.Value).ToList());

                    if (dict.ContainsKey("датасоздания"))
                    {
                        var dateStr = dict["датасоздания"].FirstOrDefault();

                        DateTime date;

                        if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out date))
                        {
                            _statementGroup.DocumentDate = date;
                        }
                        else
                        {
                            logImport.Error(string.Format("Параметр {0}", "датасоздания"), "Некорректная дата:" + dateStr);
                        }
                    }
                    else
                    {
                        _statementGroup.DocumentDate = DateTime.Now;
                    }

                    int i = 0;
                    foreach (var number in dict["номер"])
                    {
                        var rec = new Record { RowNum = 1, DocumentNum = number };
                        var success = true;

                        if (!String.IsNullOrEmpty(dict["дата"][i]))
                        {
                            rec.Date = dict["дата"][i].ToDateTime();
                        }
                        else
                        {
                            success = false;
                            logImport.Warn(string.Format("Параметр {0}", "дата"), "Некорректная дата:" + dict["дата"][i]);
                        }

                        if (!String.IsNullOrEmpty(dict["сумма"][i]))
                        {
                            rec.Sum = dict["сумма"][i].ToDecimal();
                        }
                        else
                        {
                            success = false;
                            logImport.Warn(string.Format("Параметр {0}", "сумма"), "Некорректная сумма:" + dict["сумма"][i]);
                        }

                        rec.PayerAccountNum = dict["плательщиксчет"][i];

                        if (!String.IsNullOrEmpty(dict["получательсчет"][i]))
                        {
                            rec.ReceiverAccount = dict["получательсчет"][i];
                        }
                        else
                        {
                            success = false;
                            logImport.Warn(string.Format("Параметр {0}", "получательсчет"), "Некорректный получательсчет:" + dict["получательсчет"][i]);
                        }

                        rec.Purpose = dict["назначениеплатежа"][i];


                        i++;
                        if (success)
                        {
                            records.Add(rec);
                        }
                    }
                }
            }
        }

        private void SaveData()
        {
            if (logImport.CountError > 0)
            {
                return;
            }

            var identity = Container.Resolve<IUserIdentity>();
            var userDomain = Container.Resolve<IDomainService<User>>();

            using (Container.Using(identity, userDomain))
            {

                InTransaction(() =>
                {
                    var user = userDomain.FirstOrDefault(x => x.Id == identity.UserId);
                    _bankAccountStatementGroupRepo.Save(_statementGroup);
                    foreach (var record in records)
                    {
                        var suspenseAccount = new Entities.SuspenseAccount
                        {
                            DateReceipt = record.Date.Value,
                            DetailsOfPayment = record.Purpose,
                            MoneyDirection = MoneyDirection.Income,
                            Sum = record.Sum.Value,
                            AccountBeneficiary = record.ReceiverAccount,
                            DistributeState = DistributionState.NotDistributed,
                            SuspenseAccountTypePayment = SuspenseAccountTypePayment.Payment
                        };

                        var bankAccountStatement = new BankAccountStatement
                        {
                            Group = _statementGroup,
                            RecipientAccountNum = record.ReceiverAccount,
                            DocumentDate = record.Date.Value,
                            DocumentNum = record.DocumentNum,
                            PaymentDetails = record.Purpose,
                            IsROSP = false,
                            PayerAccountNum = record.PayerAccountNum,
                            OperationDate = DateTime.Now,
                            Sum = record.Sum.Value,
                            UserLogin = user == null ? "anonymus" : user.Login,
                            IsDistributable = YesNo.Yes
                        };

                        _bankAccountStatementRepo.Save(bankAccountStatement);
                        _suspenseAccountRepository.Save(suspenseAccount);

                        logImport.Info(string.Format("Начало секции документа - строка {0}", record.RowNum), "Успешно");
                        logImport.CountAddedRows++;
                    }

                    _statementGroup.Sum = records.Sum(x => x.Sum != null ? x.Sum.Value : 0);
                    _statementGroup.ImportDate = DateTime.Now;
                    _statementGroup.UserLogin = user == null ? "anonymus" : user.Login;
                    _bankAccountStatementGroupRepo.Update(_statementGroup);
                });
            }
        }

        private void InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}