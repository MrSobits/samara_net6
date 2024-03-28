namespace Bars.Gkh.RegOperator.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Gkh.Entities;
    using Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Domain.ImportExport;
    using Domain.ProxyEntity;
    using DomainService;
    using DomainService.Impl;
    using Entities;
    using Enums;
    using Export;
    using Wcf.Contracts.PersonalAccount;
    using Microsoft.Extensions.Logging;

    // TODO: вынести методы в Service'ы
    public class PersonalAccountChargePaymentController : BaseController
    {
        public ImportExportDataProvider ImportDataProvider { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult ChargeOut(BaseParams baseParams)
        {
            var service = Container.Resolve<IChargeExportService>();

            var data = service.Export(baseParams);

            return JsSuccess(data);
        }

        public ActionResult PaymentIn(BaseParams baseParams)
        {
            var file = baseParams.Files.First();

            if (file.Value == null)
            {
                return JsFailure("Нет файла");
            }

            var providerCode = baseParams.Params.GetAs<string>("providerCode", ignoreCase: true);

            var paymentRows = ImportDataProvider.Deserialize<PaymentInProxy>(file.Value, baseParams.Params, providerCode);

            var payments = paymentRows.Rows.Select(x => x.Value).ToList();

            var result = ImportPaymentsFromFile(payments, Encoding.UTF8.GetBytes(Request.Headers["payments"]));

            return result.Success ? JsSuccess(result.Data) : JsFailure(result.Message);
        }

        private IDataResult ImportPaymentsFromFile(IEnumerable<PersonalAccountPaymentInfoIn> payments, byte[] data,
            TransitAccountProxy transitAccountProxy = null, bool overwrite = false)
        {
            var logImportManager = Container.Resolve<ILogImportManager>();
            var unaccpayProvider = Container.Resolve<IUnacceptedPaymentProvider>();

            var persaccDomain = Container.ResolveDomain<BasePersonalAccount>();
            var indivownerDomain = Container.ResolveDomain<IndividualAccountOwner>();
            var paymentAgentDomain = Container.ResolveDomain<PaymentAgent>();

            const string title = "Загрузка_из_РКЦ";
            long logFileId;
            const string message = "Импорт из РКЦ завершен успешно";

            var logImport = Container.Resolve<ILogImport>();
            logImport.ImportKey = title;

            using (Container.Using(paymentAgentDomain))
            {
                var correctAgent = transitAccountProxy == null 
                    || (!string.IsNullOrWhiteSpace(transitAccountProxy.AgentId)
                        && Resolve<IDomainService<PaymentAgent>>().GetAll()
                            .Any(x => x.Code == transitAccountProxy.AgentId));

                if (!correctAgent)
                {
                    logImport.Warn("Поле AgentId", "Переданный платежный агент не зарегистрирован в системе");
                }
            }

            if (transitAccountProxy != null && !transitAccountProxy.RegNum.IsEmpty())
            {
                var query = GetSameDocumentPayments(transitAccountProxy.RegNum, transitAccountProxy.RegDate);

                if (query.Any())
                {
                    logImport.Info("Информация",
                        "Имеются записи по документу с номером {0} от {1}".FormatUsing(
                            transitAccountProxy.RegNum, transitAccountProxy.RegDate.ToString("dd.MM.yyyy")));
                    if (overwrite)
                    {
                        CancelSameDocumentUnacceptedPayments(logImport, query);
                        CleanupSameDocumentImports(transitAccountProxy.RegNum, transitAccountProxy.RegDate);
                    }
                    else
                    {
                        logImport.Error("Ошибка", "Повторная загрузка реестра не разрешена");
                        return
                            new BaseDataResult(new ImportResult(StatusImport.CompletedWithError,
                                "Имеются записи по документу с такими номером и датой", title));
                    }
                }
            }

            using (Container.Using(logImportManager, persaccDomain, indivownerDomain, unaccpayProvider))
            {
                var accountNumbers =
                    payments
                        .Where(x => !string.IsNullOrWhiteSpace(x.AccountNumber))
                        .Select(x => x.AccountNumber.Trim())
                        .ToArray();

                var indivownerAccounts = persaccDomain.GetAll()
                    .Where(x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                    .Where(x => accountNumbers.Contains(x.PersonalAccountNum))
                    .Where(x => x.PersonalAccountNum != null)
                    .Select(x => new
                    {
                        x.PersonalAccountNum,
                        x.AccountOwner.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.PersonalAccountNum)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

                var owners = indivownerDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.FirstName,
                        x.Surname,
                        x.SecondName
                    })
                    .ToDictionary(x => x.Id);

                foreach (var payment in payments)
                {
                    long id;
                    if (payment.AccountNumber != null && indivownerAccounts.TryGetValue(payment.AccountNumber, out id))
                    {
                        var owner = owners.Get(id);

                        payment.Name = owner.Return(x => x.FirstName);
                        payment.Surname = owner.Return(x => x.Surname);
                        payment.Patronymic = owner.Return(x => x.SecondName);
                    }
                }

                unaccpayProvider.CreateUnacceptedPayments(logImport, payments, "Загрузка_из_РКЦ", transitAccountProxy);

                var fileName = string.Format("Загрузка_из_РКЦ_{0}", DateTime.Now.ToString("s"));

                using (var ms = new MemoryStream(data))
                {
                    logImportManager.FileNameWithoutExtention = fileName;
                    logImportManager.Add(ms, fileName + ".txt", logImport);
                    logImportManager.UploadDate = DateTime.Now;
                    logFileId = logImportManager.Save();
                }
            }

            return
                new BaseDataResult(new ImportResult
                {
                    LogFileId = logFileId,
                    Message = message,
                    StatusImport = StatusImport.CompletedWithoutError,
                    Title = title
                });
        }

        private IQueryable<UnacceptedPayment> GetSameDocumentPayments(string number, DateTime date)
        {
            var importedPaymentDomain = Container.ResolveDomain<ImportedPayment>();
            var unacceptedPaymentsDomain = Container.ResolveDomain<UnacceptedPayment>();
            using (Container.Using(importedPaymentDomain, unacceptedPaymentsDomain))
            {
                var importedPaymentIds =
                    importedPaymentDomain.GetAll()
                        .Where(x => x.BankDocumentImport.DocumentNumber == number)
                        .Where(x => x.BankDocumentImport.DocumentDate == date)
                        .Where(x => x.PaymentId.HasValue)
                        .Select(x => x.PaymentId.Value);

                return unacceptedPaymentsDomain.GetAll().Where(x => importedPaymentIds.Contains(x.Id));
            }
        }

        private void CancelSameDocumentUnacceptedPayments(ILogImport logImport, IQueryable<UnacceptedPayment> query)
        {
            var unacceptedPaymentsService = Container.Resolve<IUnacceptedPaymentService>();
            using (this.Container.Using(unacceptedPaymentsService))
            {
                var payments = query.Where(x => !x.Accepted).Select(x => x.Id).ToArray();
                if (payments.Length > 0)
                {
                    unacceptedPaymentsService.CancelPayments(payments);
                    logImport.Info("Отмена оплат",
                        "Отменено {0} неподтвержденных оплат по текущему документу".FormatUsing(payments.Length));
                }
            }
        }

        private void CleanupSameDocumentImports(string number, DateTime date)
        {
            var bankDocImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            using (this.Container.Using(bankDocImportDomain, importedPaymentDomain))
            {
                var paymentsByDocImport =
                    importedPaymentDomain.GetAll()
                        .Where(x => x.BankDocumentImport.DocumentNumber == number)
                        .Where(x => x.BankDocumentImport.DocumentDate == date)
                        .Select(x => new
                        {
                            x.Id,
                            x.BankDocumentImport,
                            removable = x.PaymentState == ImportedPaymentState.Rno && x.PaymentId.HasValue,
                            x.Sum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.BankDocumentImport)
                        .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var paymentGroup in paymentsByDocImport)
                {
                    paymentGroup.Value.Where(x => x.removable).ForEach(x => importedPaymentDomain.Delete(x.Id));
                    if (paymentGroup.Value.All(x => x.removable))
                    {
                        bankDocImportDomain.Delete(paymentGroup.Key.Id);
                    }
                    else
                    {
                        paymentGroup.Key.ImportedSum = paymentGroup.Value.Where(x => !x.removable).Sum(x => x.Sum);
                        bankDocImportDomain.Update(paymentGroup.Key);
                    }
                }
            }
        }
    }
}