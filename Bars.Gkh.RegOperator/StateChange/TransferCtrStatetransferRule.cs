namespace Bars.Gkh.RegOperator.StateChange
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.Gkh.RegOperator.Enums;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;

    class TransferCtrStatetransferRule : IRuleChangeStatus
    {
        public IDomainService<RealityObjectPaymentAccountOperation> RealityObjectPaymentAccountOperationService { get; set; }
        public IDomainService<RealityObjectSupplierAccountOperation> RealityObjectSupplierAccountOperationService { get; set; }
        public IDomainService<RealityObjectPaymentAccount> RealityObjectPaymentAccountService { get; set; }
        public IDomainService<RealityObjectSupplierAccount> RealityObjectSupplierAccountService { get; set; }
        
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var entity = statefulEntity as TransferCtr;

            if (entity == null)
            {
                return ValidateResult.No("Заявка отсутствует");
            }

            var acc = RealityObjectPaymentAccountService.GetAll().FirstOrDefault(x => x.RealityObject.Id == entity.ObjectCr.RealityObject.Id);

            if (acc == null)
            {
                return ValidateResult.No("Отсутствует счет оплат дома");
            }

            var supAcc = RealityObjectSupplierAccountService.GetAll().FirstOrDefault(x => x.RealityObject.Id == entity.ObjectCr.RealityObject.Id);

            if (supAcc == null)
            {
                return ValidateResult.No("Отсутствует счет расчета с поставщиками дома");
            }

            var accOpp = new RealityObjectPaymentAccountOperation
            {
                Date = DateTime.Now,
                OperationSum = entity.PaidSum,
                Account = acc,
                OperationType = PaymentOperationType.OutcomeAccountPayment,
                OperationStatus  = OperationStatus.Approved
            };

            RealityObjectPaymentAccountOperationService.Save(accOpp);

            var suppAccOp = new RealityObjectSupplierAccountOperation
            {
                Date = DateTime.Now,
                OperationType  = PaymentOperationType.OutcomeAccountPayment,
                Credit = entity.PaidSum,
                Debt = entity.Sum,             
                Account = supAcc,
                Work = entity.TypeWorkCr.With(t => t.Work)
            };

            RealityObjectSupplierAccountOperationService.Save(suppAccOp);

            return ValidateResult.Yes();
        }

        public string Id { get { return "gkhrf_transferCtr_rule"; } }
        public string Name { get { return "Обновление данных операций счета расчета с поставщиками в паспорте дома"; } }
        public string TypeId { get { return "rf_transfer_ctr"; } }
        public string Description
        {
            get
            {
                return "Правило осуществляет проверку заполненности полей формы уведомления о принятии решения";
            }
        }
    }
}
