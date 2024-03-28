namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using System;

    public interface IPrescriptionViolService
    {
        IDataResult ListRealityObject(BaseParams baseParams);

        IDataResult AddViolations(BaseParams baseParams);

        IDataResult AddPrescriptionViolations(BaseParams baseParams);

        IDataResult ListPrescriptionViolation(BaseParams baseParams);

        IDataResult SetNewDatePlanRemoval(BaseParams baseParams, DateTime paramdate, long documentId);
    }
}
