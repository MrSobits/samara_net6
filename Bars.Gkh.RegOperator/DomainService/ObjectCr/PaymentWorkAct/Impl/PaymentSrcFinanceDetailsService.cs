namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;

    using B4.Modules.FileStorage;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using Gkh.Domain;
    using GkhCr.Entities;

    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class PaymentSrcFinanceDetailsService : IPaymentSrcFinanceDetailsService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PaymentSrcFinanceDetails> Domain { get; set; }

        public IDomainService<PerformedWorkActPayment> ActPaymentDomain { get; set; }

        public IDomainService<PerformedWorkAct> ActDomain { get; set; }

        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        public IFileManager FileManagerService { get; set; }

        public IDataResult SaveRecords(BaseParams baseParams)
        {
            var records = baseParams.Params.Read<SaveParam<PaymentSrcFinanceDetails>>()
                .Execute(container => B4.DomainService.BaseParams.Converter.ToSaveParam<PaymentSrcFinanceDetails>(container, true));

            var payment = baseParams.Params.GetAs<PerformedWorkActPayment>("payment");
            var workActId = baseParams.Params.GetAsId("workActId");

            payment.PerformedWorkAct = ActDomain.Load(workActId);

            var overdraftService = Container.Resolve<ICalcAccountOverdraftService>();

            var overdraft = overdraftService.GetLastOverdraft(payment.PerformedWorkAct.ObjectCr.RealityObject);

            if (records != null && records.Any())
            {
                // Сначала вычисляем сумму которую надо типа вернуть на счет чтобы якобы восстановить ситуацию которая была бы без этих сумм
                // Потому что в этом сохранении могли бы изменится суммы 
                // Если вы ничего не поняли, то читайте дальше
                var existSums = Domain.GetAll()
                    .Where(x => x.ActPayment.Id == payment.Id)
                    .ToDictionary(x => x.Id, y => Math.Max(y.Payment - y.Balance, 0m));

                var availableSum = overdraft.Return(x => x.AvailableSum);

                var listToSave = new List<PaymentSrcFinanceDetails>();

                // Сохраняем все записи, которые были изменены
                foreach (var rec in records.Select(x => x.AsObject()))
                {
                    if (rec.ActPayment == null)
                    {
                        rec.ActPayment = payment;
                    }

                    var existSum = existSums.Get(rec.Id);
                    var currentSum = Math.Max(rec.Payment - rec.Balance, 0m);
                    availableSum = availableSum + existSum - currentSum;

                    listToSave.Add(rec);
                }

                if (availableSum < 0)
                {
                    throw new ValidationException("Недостаточно средств для оплаты акта!");
                }

                // Пересчитываем сумму оплаты
                payment.Sum = listToSave.Sum(x => x.Payment);

                Container.InTransaction(() =>
                {
                    if (payment.Id > 0)
                        ActPaymentDomain.Update(payment);
                    else
                        ActPaymentDomain.Save(payment);

                    foreach (var rec in listToSave)
                    {
                        if (rec.Id > 0)
                            Domain.Update(rec);
                        else
                            Domain.Save(rec);
                    }

                    overdraftService.UpdateAccountOverdraft(overdraft, availableSum);
                });

            }

            // Генерируем документ и прикрепляем его 
            CreateReport(payment, baseParams);

            return new BaseDataResult();
        }

        private void CreateReport(PerformedWorkActPayment payment, BaseParams baseParams)
        {
            IPrintForm printForm;

            if (Container.Kernel.HasComponent("PaymentSrcFinanceDetailsReport"))
            {
                printForm = Container.Resolve<IPrintForm>("PaymentSrcFinanceDetailsReport");
            }
            else
            {
                return;
            }

            // такой костыль нужен для отчета потомуч то он принимает параметр такой
            baseParams.Params.Add("paymentId", payment.Id);

            var rp = new ReportParams();
            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            using (var result = new MemoryStream())
            {
                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                var fileInfo = FileManagerService.SaveFile(result, "PaymentSrcFinanceDetailsReport.xlsx");
                FileInfoDomain.Save(fileInfo);

                payment.Document = fileInfo;
                // TODO убрать костыль (29236)
                payment.HandMade = true;
                ActPaymentDomain.Update(payment);
            }
        }
    }
}