namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;

    using DomainService.RealityObjectAccount;

    using Entities;

    using Enums;

    using Gkh.Domain.CollectionExtensions;

    using GkhCr.Entities;

    public class PerformedWorkActPaymentInterceptor : EmptyDomainInterceptor<PerformedWorkActPayment>
    {
        public IDomainService<RealObjPaymentAccOperPerfAct> RealObjPaymentAccOperPerfActDomain { get; set; }
        public IDomainService<RealityObjectPaymentAccountOperation> RealObjPaymentAccOperDomain { get; set; }
        public IDomainService<RealityObjectPaymentAccount> RealObjPaymentAccDomain { get; set; }
        public IDomainService<RealObjSupplierAccOperPerfAct> RealObjSupplierAccOperPerfActDomain { get; set; }
        public IDomainService<RealityObjectSupplierAccountOperation> RealObjSupplierAccOperDomain { get; set; }
        public IDomainService<RealityObjectSupplierAccount> RealObjSupplierAccDomain { get; set; }
        public IDomainService<PaymentOrderDetail> PaymentOrderDetailDomain { get; set; }
        public IDomainService<MoneyLock> MoneyLockDomain { get; set; }
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }
        public IRealityObjectPaymentAccountRepository RealityObjectPaymentAccountRepository { get; set; }
        public IRealityObjectPaymentService RoPaymentService { get; set; }
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<PerformedWorkActPayment> service, PerformedWorkActPayment entity)
        {
            return this.ChangeOperationData(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<PerformedWorkActPayment> service, PerformedWorkActPayment entity)
        {
            return this.ChangeOperationData(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PerformedWorkActPayment> service, PerformedWorkActPayment entity)
        {
            if (!entity.DatePayment.HasValue && entity.Paid == 0)
            {
                if (entity.Sum > 0)
                {
                    var acc = this.RealityObjectPaymentAccountRepository.GetByRealtyObject(entity.PerformedWorkAct.Realty);

                    if (acc != null)
                    {
                        var locks = this.MoneyLockDomain.GetAll().Where(x => x.TargetGuid == entity.TransferGuid && x.IsActive).ToList();

                        foreach (var moneyLock in locks)
                        {
                            var cancelOp = new MoneyOperation(entity.TransferGuid, moneyLock.Operation, this.ChargePeriodRepository.GetCurrentPeriod(), moneyLock.Amount);
                            this.MoneyOperationDomain.Save(cancelOp);
                            acc.UnlockMoney(moneyLock.Wallet, cancelOp, moneyLock);
                        }

                        this.RealityObjectPaymentAccountRepository.SaveOrUpdate(acc);
                    }

                }

                this.RealObjPaymentAccOperPerfActDomain.GetAll()
                    .Where(x => x.PerformedWorkActPayment.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => this.RealObjPaymentAccOperPerfActDomain.Delete(x));

                this.PaymentOrderDetailDomain.GetAll()
                    .Where(x => x.PaymentOrder.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => this.PaymentOrderDetailDomain.Delete(x));

                return this.Success();

            }

            if(this.RealObjPaymentAccOperPerfActDomain.GetAll().Any(x => x.PerformedWorkActPayment.Id == entity.Id))
            {
                return this.Failure("Существуют связанная запись в счете оплат жилого дома");
            }

            return this.Success();
        }

        private IDataResult ChangeOperationData(IDomainService<PerformedWorkActPayment> service, PerformedWorkActPayment entity)
        {
            if (!entity.HandMade)
            {
                // Получаем счет оплат дома
                var realObjPaymentAcc =
                    this.RealObjPaymentAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == entity.PerformedWorkAct.ObjectCr.RealityObject.Id);

                // Получаем счет с поставщиками 
                var realObjSupplierAcc =
                    this.RealObjSupplierAccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == entity.PerformedWorkAct.ObjectCr.RealityObject.Id);

                if (realObjPaymentAcc == null || realObjSupplierAcc == null)
                {
                    return this.Success();
                }

                // Получаем операцию оплаты по оплате акта выполненных работ
                var realObjPaymentAccOper =
                    this.RealObjPaymentAccOperPerfActDomain.GetAll()
                        .Where(x => x.PerformedWorkActPayment.Id == entity.Id)
                        .Select(x => x.PayAccOperation)
                        .FirstOrDefault();

                if (realObjPaymentAccOper == null)
                {
                    realObjPaymentAccOper = this.RoPaymentService.CreatePaymentOperation(
                        realObjPaymentAcc.RealityObject,
                        entity.Paid,
                        PaymentOperationType.OutcomeAccountPayment,
                        entity.DatePayment.ToDateTime(),
                        realObjPaymentAcc);

                    this.RealObjPaymentAccOperPerfActDomain.Save(
                        new RealObjPaymentAccOperPerfAct { PayAccOperation = realObjPaymentAccOper, PerformedWorkActPayment = entity });
                }
                else
                {
                    realObjPaymentAccOper.OperationSum = entity.Paid;
                    realObjPaymentAccOper.Date = entity.DatePayment.ToDateTime();
                    this.RealObjPaymentAccOperDomain.Update(realObjPaymentAccOper);
                }

                // Получаем операцию  рассчета с поставщиками по акту выполненных работ
                var realObjSupplierAccOper =
                    this.RealObjSupplierAccOperPerfActDomain.GetAll()
                        .Where(x => x.PerformedWorkAct.Id == entity.PerformedWorkAct.Id)
                        .Select(x => x.SupplierAccOperation)
                        .FirstOrDefault();

                // Получаем сумму оплат по данному акту
                var paymentOpersSum =
                    this.RealObjPaymentAccOperPerfActDomain.GetAll()
                        .Where(
                            x => x.PerformedWorkActPayment.PerformedWorkAct.Id == entity.PerformedWorkAct.Id && x.PerformedWorkActPayment.Id != entity.Id)
                        .Select(x => x.PerformedWorkActPayment.Paid)
                        .SafeSum() + entity.Paid;

                if (realObjSupplierAccOper == null)
                {
                    realObjSupplierAccOper = new RealityObjectSupplierAccountOperation
                    {
                        Date = entity.PerformedWorkAct.DateFrom.ToDateTime(),
                        Account = realObjSupplierAcc,
                        Credit = paymentOpersSum,
                        OperationType = PaymentOperationType.OutcomeAccountPayment,
                        Debt = 0
                    };

                    this.RealObjSupplierAccOperDomain.Save(realObjSupplierAccOper);

                    this.RealObjSupplierAccOperPerfActDomain.Save(
                        new RealObjSupplierAccOperPerfAct { SupplierAccOperation = realObjSupplierAccOper, PerformedWorkAct = entity.PerformedWorkAct });
                }
                else
                {
                    realObjSupplierAccOper.Credit = paymentOpersSum;
                    realObjSupplierAccOper.Date = entity.PerformedWorkAct.DateFrom.ToDateTime();

                    this.RealObjSupplierAccOperDomain.Update(realObjSupplierAccOper);
                }
            }

            return this.Success();
        }
    }
}