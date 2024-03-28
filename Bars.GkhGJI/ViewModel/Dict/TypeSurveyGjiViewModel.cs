namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TypeSurveyGjiViewModel : BaseViewModel<TypeSurveyGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var ids = baseParams.Params.ContainsKey("Id")
                                   ? baseParams.Params["Id"].ToString()
                                   : "";

            // получаем вид проверки для загрузки типов обследования, содержащих этот вид проверки
            var kindCheckId = baseParams.Params.ContainsKey("kindCheckId")
                                   ? baseParams.Params["kindCheckId"].ToLong()
                                   : 0;

            List<long> listIds = null;

            if (!string.IsNullOrEmpty(ids))
            {
                listIds = new List<long>();
                listIds.AddRange(ids.Split(',').Select(x => x.ToLong()));
            }

            if (kindCheckId > 0)
            {
                var types = Container.Resolve<IDomainService<TypeSurveyKindInspGji>>().GetAll()
                    .Where(x => x.KindCheck.Id == kindCheckId)
                    .Select(x => x.TypeSurvey.Id)
                    .ToList();

                listIds = listIds ?? new List<long>();
                listIds.AddRange(types);
            }

            var data = domainService.GetAll()
                .WhereIf(listIds != null, x => listIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}