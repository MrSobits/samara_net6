namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;

    using B4;

    using Entities;

    public interface IOverhaulViewModels
    {
        IDataResult<IEnumerable<FinanceSourceResourceProxy>> FinanceSourceResList(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams);

        IDataResult FinanceSourceResGet(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams);

        IDataResult DefectListList(IDomainService<DefectList> domainService, BaseParams baseParams);

        IDataResult DefectListGet(IDomainService<DefectList> domainService, BaseParams baseParams);

        IDataResult TypeWorkCrList(IDomainService<TypeWorkCr> domainService, BaseParams baseParams);

        IDataResult TypeWorkStage1List(BaseParams baseParams);
    }
}