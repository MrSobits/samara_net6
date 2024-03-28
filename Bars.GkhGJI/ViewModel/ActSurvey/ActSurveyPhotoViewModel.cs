namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActSurveyPhotoViewModel : BaseViewModel<ActSurveyPhoto>
    {
        public override IDataResult List(IDomainService<ActSurveyPhoto> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActSurvey.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.IsPrint,
                    x.Name,
                    x.Description,
                    x.ImageDate,
                    x.Group,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}