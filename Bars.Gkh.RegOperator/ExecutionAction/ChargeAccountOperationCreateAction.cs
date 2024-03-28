namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    public class ChargeAccountOperationCreateAction : BaseExecutionAction
    {
        public IDomainService<RealityObjectChargeAccountOperation> RoChargeOperationDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IRealtyObjectRegopOperationService RealtyObjectRegopOperationService { get; set; }

        public IChargePeriodRepository ChargePeriodRepo { get; set; }

        public override string Description => @"Создание операций по счету начислений дома (если отсутствуют)";

        public override string Name => "Создание операций по счету начислений дома (если отсутствуют)";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var period = this.ChargePeriodRepo.GetCurrentPeriod();

            if (period != null)
            {
                var roIds = this.RoChargeOperationDomain.GetAll().Select(x => x.Account.RealityObject.Id).Distinct().ToArray();

                var ros = this.RoDomain.GetAll().AsEnumerable().Where(x => !roIds.Contains(x.Id)).ToList();

                foreach (var ro in ros)
                {
                    this.RealtyObjectRegopOperationService.CreateRealtyObjectChargeOperations(period, ro);
                }
            }
            else
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Отсутствует открытый период"
                };
            }

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}