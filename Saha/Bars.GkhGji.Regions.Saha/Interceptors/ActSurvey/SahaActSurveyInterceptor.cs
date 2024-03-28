namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class SahaActSurveyInterceptor: ActSurveyServiceInterceptor<SahaActSurvey>
    {
        public IDomainService<ActSurveyLongDescription> DescriptionDomain { get; set; }

        public IDomainService<ActSurveyConclusion> ConclusionDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SahaActSurvey> service, SahaActSurvey entity)
        {
            entity.ConclusionIssued = YesNo.No;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SahaActSurvey> service, SahaActSurvey entity)
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
