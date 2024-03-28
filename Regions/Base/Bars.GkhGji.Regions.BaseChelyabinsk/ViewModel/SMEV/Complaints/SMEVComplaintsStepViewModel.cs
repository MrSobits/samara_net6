namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVComplaintsStepViewModel : BaseViewModel<SMEVComplaintsStep>
    {
        public override IDataResult List(IDomainService<SMEVComplaintsStep> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var complaintId = baseParams.Params.GetAs<long>("complaintId");
            var data = domainService.GetAll()
                .Where(x=> x.SMEVComplaints.Id == complaintId)
                .Select(x=> new
                {
                    x.Id,
                    x.DOPetitionResult,
                    x.DOTypeStep,
                    x.YesNo,
                    x.FileInfo
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


       
    }
}
