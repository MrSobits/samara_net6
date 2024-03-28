namespace Bars.GkhDi.Regions.Tatarstan.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    public class UpdateRepairServiceToBeProvidedByManOrgAction : BaseExecutionAction
    {
        public ISessionProvider SessionPrivider { get; set; }

        public override string Description => @"Задача 21231. 
для всех УО в 2013г по каждому дому в управлении УО:
если в сведениях об услугах есть услуга с видом='ремонт' и типом предоставления= 'услуга предоставляется без участия УО', 
то поменять тип предоставления на  'услуга предоставляется через УО'";

        public override string Name => "Смена типа предоставления услуг 'ремонт'";

        public override Func<IDataResult> Action => this.UpdateRepairServiceToBeProvidedByManOrg;

        public BaseDataResult UpdateRepairServiceToBeProvidedByManOrg()
        {
            var query = this.SessionPrivider.GetCurrentSession()
                .CreateSQLQuery(@"UPDATE di_repair_service
SET type_of_provision_service = 10
WHERE type_of_provision_service = 20
AND id in (
        SELECT base_service.id 
        FROM di_base_service base_service
        JOIN di_disinfo_realobj diRo ON diRo.id = base_service.disinfo_ro_id
        WHERE diRo.period_di_id = 3)");

            query.ExecuteUpdate();

            return new BaseDataResult();
        }
    }
}