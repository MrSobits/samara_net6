using System.Linq;
using Castle.Windsor;

namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using B4;
    using Entities.Dict;
    using Gkh.Domain;

    public class AuditPurposeSurveySubjectGjiService : IAuditPurposeSurveySubjectGjiService
    {
        public IDomainService<AuditPurposeGji> AuditPurposeService { get; set; }
        public IDomainService<SurveySubject> SurveySubjectService { get; set; }
        public IDomainService<AuditPurposeSurveySubjectGji> Service { get; set; }

        public IDataResult AddAuditPurposeSurveySubjectGji(BaseParams baseParams)
        {
            long auditPurposeId = baseParams.Params.GetAs<long>("auditPurposeId");
            if (auditPurposeId == 0)
            {
                return new BaseDataResult();
            }
            var surveySubjectIds = baseParams.Params.GetAs<string>("surveySubjectIds", string.Empty).ToLongArray();

            AuditPurposeGji auditPurpose = AuditPurposeService.Get(auditPurposeId);
            List<long> existingSurveySubjectIds = Service.GetAll()
                .Where(x => x.AuditPurpose.Id == auditPurpose.Id)
                .Select(x => x.SurveySubject.Id).ToList();
            List<long> listToAdd = surveySubjectIds.Where(x => !existingSurveySubjectIds.Contains(x)).ToList();
            List<long> listToRemove = existingSurveySubjectIds.Where(x => !surveySubjectIds.Contains(x)).ToList();

            foreach (var surveySubjectId in listToAdd)
            {
                Service.Save(new AuditPurposeSurveySubjectGji { AuditPurpose = auditPurpose, SurveySubject = SurveySubjectService.Get(surveySubjectId) });
            }

            foreach (var surveySubjectId in listToRemove)
            {
                var auditPurposeSurveySubjectGji = Service.GetAll().FirstOrDefault(x => x.AuditPurpose.Id == auditPurpose.Id && x.SurveySubject.Id == surveySubjectId);
                if (auditPurposeSurveySubjectGji != null)
                {
                    Service.Delete(auditPurposeSurveySubjectGji.Id);
                }
            }

            return new BaseDataResult();
        }
    }
}