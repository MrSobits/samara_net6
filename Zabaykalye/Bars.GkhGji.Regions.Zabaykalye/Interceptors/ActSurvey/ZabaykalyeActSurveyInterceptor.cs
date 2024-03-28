namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class ZabaykalyeActSurveyInterceptor: ActSurveyServiceInterceptor<ZabaykalyeActSurvey>
    {
        public IDomainService<ActSurveyLongDescription> DescriptionDomain { get; set; }

        public IDomainService<ActSurveyConclusion> ConclusionDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ZabaykalyeActSurvey> service, ZabaykalyeActSurvey entity)
        {
            entity.ConclusionIssued = YesNo.No;
            entity.DocumentDate = DateTime.Today;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ZabaykalyeActSurvey> service, ZabaykalyeActSurvey entity)
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
