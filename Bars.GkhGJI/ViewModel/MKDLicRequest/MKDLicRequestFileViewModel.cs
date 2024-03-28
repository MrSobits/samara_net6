namespace Bars.GkhGji.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class MKDLicRequestFileViewModel : BaseViewModel<MKDLicRequestFile>
    {
        public override IDataResult List(IDomainService<MKDLicRequestFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs<long>("mkdlicrequestId");
            var requestId = baseParams.Params.GetAs<long>("mkdlicrequestId");

            var data = domain.GetAll()
             .Where(x => x.MKDLicRequest.Id == (id == 0 ? requestId : id))
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.SignedFile,
                x.Description,
                x.LicStatementDocType,
                x.DocumentName,
                x.DocDate
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}