﻿namespace Bars.GkhGji.Regions.Khakasia.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    public class KhakasiaProtocolDefinitionViewModel : GkhGji.ViewModel.ProtocolDefinitionViewModel<KhakasiaProtocolDefinition>
    {
        public override IDataResult List(IDomainService<KhakasiaProtocolDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition,
                    x.TimeDefinition,
                    x.DateOfProceedings,
                    x.TimeStart,
                    x.TimeEnd,
                    x.FileDescription
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<KhakasiaProtocolDefinition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    Protocol = x.Protocol.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    x.IssuedDefinition,
                    x.Description,
                    x.TypeDefinition,
                    TimeDefinition = x.TimeDefinition.HasValue ? x.TimeDefinition.Value.ToShortTimeString() : "",
                    x.DateOfProceedings,
                    TimeStart = x.TimeStart.HasValue ? x.TimeStart.Value.ToShortTimeString() : "",
                    TimeEnd = x.TimeEnd.HasValue ? x.TimeEnd.Value.ToShortTimeString() : "",
                    x.FileDescription
                })
                .FirstOrDefault());
        }
    }
}