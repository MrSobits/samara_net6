namespace Bars.GkhGji.Regions.Khakasia.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    public class KhakasiaActSurveyInterceptor: ActSurveyServiceInterceptor<KhakasiaActSurvey>
    {
        public IDomainService<ActSurveyLongDescription> DescriptionDomain { get; set; }

        public IDomainService<ActSurveyConclusion> ConclusionDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<KhakasiaActSurvey> service, KhakasiaActSurvey entity)
        {
            entity.ConclusionIssued = YesNo.No;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<KhakasiaActSurvey> service, KhakasiaActSurvey entity)
        {
            var descriptions = this.DescriptionDomain.GetAll().Where(x => x.ActSurvey.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var descId in descriptions)
            {
                this.DescriptionDomain.Delete(descId);
            }

            var conclusions = this.ConclusionDomain.GetAll().Where(x => x.ActSurvey.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var descId in conclusions)
            {
                this.ConclusionDomain.Delete(descId);
            }
            return base.BeforeDeleteAction(service, entity);
        }

    }
}
