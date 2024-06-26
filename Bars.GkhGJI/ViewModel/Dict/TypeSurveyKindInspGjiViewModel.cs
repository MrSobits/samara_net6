﻿namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyKindInspGjiViewModel : BaseViewModel<TypeSurveyKindInspGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyKindInspGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.ContainsKey("typeSurveyId")
                                   ? baseParams.Params["typeSurveyId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    KindCheck = x.KindCheck.Name
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}