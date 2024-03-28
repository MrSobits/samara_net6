namespace Bars.Gkh.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectDirectManagContractViewModel : BaseViewModel<RealityObjectDirectManagContract>
    {
       public override IDataResult Get(IDomainService<RealityObjectDirectManagContract> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAs<long>("id"));

           return obj != null
               ? new BaseDataResult(new
               {
                   obj.Id,
                   obj.DocumentName,
                   obj.DocumentNumber,
                   obj.StartDate,
                   obj.EndDate,
                   obj.PlannedEndDate,
                   obj.FileInfo,
                   obj.Note,
                   obj.IsServiceContract,
                   obj.ServContractFile,
                   obj.DateStartService,
                   obj.DateEndService,
                   ManagingOrganization = obj.ManagingOrganization != null
                       ? new {obj.ManagingOrganization.Id, ContragentName = obj.ManagingOrganization.Contragent.Name}
                       : null,
                   obj.TypeContractManOrgRealObj
               })
               : new BaseDataResult();
        }
    }
}