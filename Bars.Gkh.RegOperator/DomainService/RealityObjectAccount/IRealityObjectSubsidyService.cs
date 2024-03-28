namespace Bars.Gkh.RegOperator.DomainService.RealityObjectAccount
{
    using System;
    using System.Collections.Generic;
    using B4;
    using Entities;

    public interface IRealityObjectSubsidyService
    {
        IDataResult GetPlanSubsidyOperations(BaseParams baseParams);

        List<PlanSubsidyOperationProxy> GetListPlanSubsidyOperations(RealityObjectSubsidyAccount account);
    }

    public class PlanSubsidyOperationProxy
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public decimal FederalStandardFee { get; set; }
        public decimal Tariff { get; set; }
        public decimal Area { get; set; }
        public decimal Days { get; set; }
        public decimal Sum { get; set; }
    }
}