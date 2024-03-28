namespace Bars.Gkh.RegOperator.Wcf.Services
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Gkh.Entities;
    using Gkh.Enums.Import;
    using Import;
    using DomainService;
    using Entities;
    using Castle.Windsor;
    using Contracts.PersonalAccount;

    
    // TODO: WCF
   // [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class NullPersonalAccountService : IPersonalAccountService
    {
        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public PersonalAccountInfoOut GetPersonalAccountInfo(PersonalAccountInfoIn arg)
        {
            return Container.Resolve<IPersonalAccountInfoService>().GetPersonalAccountInfo(arg);
        }

        public PersonalAccountPaymentInfoOut PayBill(PersonalAccountPaymentInfoIn arg)
        {
            var logImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            logImport.SetFileName("BankService");
            logImport.ImportKey = "BankService";

            var bankImportLog = Container.Resolve<IUnacceptedPaymentProvider>().CreateUnacceptedPayments(logImport, new[] {arg}, "BankService");

            LogManager.FileNameWithoutExtention = "BankService";
            LogManager.AddLog(logImport);
            LogManager.Save();

            var logImportRecord =
                Container.Resolve<IRepository<LogImport>>().GetAll()
                    .FirstOrDefault(x => x.LogFile.Id == LogManager.LogFileId);

            bankImportLog.LogImport = logImportRecord;

            Container.Resolve<IDomainService<BankDocumentImport>>().Update(bankImportLog);

            var message = LogManager.GetInfo();
            var status = LogManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);

            var result = new PersonalAccountPaymentInfoOut();

            switch (status)
            {
                case StatusImport.CompletedWithError:
                    result.Error = message;
                    result.PaymentStatus = "Отклонен";
                    break;
                case StatusImport.CompletedWithWarning:
                case StatusImport.CompletedWithoutError:
                    result.PaymentStatus = "Принят";
                    break;
            }

            return result;
        }
    }
}