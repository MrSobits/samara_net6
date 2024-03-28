namespace Bars.GkhGji.Regions.Saha.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class DisposalSurveySubjectViewModel : BaseViewModel<DisposalSurveySubject>
    {
        public IDomainService<DisposalSurveySubject> Service { get; set; }

        public override IDataResult List(IDomainService<DisposalSurveySubject> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var dispId = baseParams.Params.GetAs<long>("documentId");

            var data = this.Service.GetAll()
                .Where(x => x.Disposal.Id == dispId)
                .Select(x => new { x.Id, x.SurveySubject.Code, x.SurveySubject.Name })
                .Filter(loadParam, this.Container)
                .Order(loadParam);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }
    }
}