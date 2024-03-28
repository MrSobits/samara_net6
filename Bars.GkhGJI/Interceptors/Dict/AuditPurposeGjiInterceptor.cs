namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Entities.Dict;

    public class AuditPurposeGjiInterceptor : EmptyDomainInterceptor<AuditPurposeGji>
    {
        public IDomainService<AuditPurposeSurveySubjectGji> AuditPurposeSurveySubjectGjiService { get; set; }
        public override IDataResult BeforeDeleteAction(IDomainService<AuditPurposeGji> service, AuditPurposeGji entity)
        {
            try
            {
                var listToDel = AuditPurposeSurveySubjectGjiService.GetAll().Where(x => x.AuditPurpose.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var item in listToDel)
                {
                    AuditPurposeSurveySubjectGjiService.Delete(item);
                }
                return Success();
            }
            catch (ValidationException exc)
            {
                return Failure("Существуют связанные записи в следующих таблицах: Ответы по обращению граждан;");
            }
            finally
            {
                Container.Release(AuditPurposeSurveySubjectGjiService);
            }
        }
    }
}