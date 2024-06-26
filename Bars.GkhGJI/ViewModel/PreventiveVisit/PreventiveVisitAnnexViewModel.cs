﻿namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PreventiveVisitAnnexViewModel : BaseViewModel<PreventiveVisitAnnex>
    {
        public override IDataResult List(IDomainService<PreventiveVisitAnnex> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                               ? baseParams.Params["documentId"].ToLong()
                               : 0;

            var data = domain
                .GetAll()
                .Where(x => x.PreventiveVisit.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.TypeAnnex,
                    x.Description,
                    x.File,
                    x.SignedFile,
                    x.Signature
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}