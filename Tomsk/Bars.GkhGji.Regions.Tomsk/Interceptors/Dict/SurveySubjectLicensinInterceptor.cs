namespace Bars.GkhGji.Regions.Tomsk.Interceptors.Dict
{
	using System.Linq;
	using B4;
	using Entities;
	using GkhGji.Entities.Dict;

    public class SurveySubjectgLicensinInterceptor : EmptyDomainInterceptor<SurveySubjectLicensing>
    {
		public IDomainService<DisposalVerificationSubjectLicensing> DisposalSurveySubjectLicensingDomain { get; set; }

        /// <summary>
        /// Проверка на использование в одном из приказо
        /// </summary>
        /// <param name="domain">Домен</param>
        /// <param name="entity">Сущность</param>
        /// <returns></returns>
        public override IDataResult BeforeDeleteAction(IDomainService<SurveySubjectLicensing> domain, SurveySubjectLicensing entity)
        {
	        var disposalExists = this.DisposalSurveySubjectLicensingDomain.GetAll()
		        .Any(x => x.SurveySubject.Id == entity.Id);

	        if (disposalExists)
	        {
		        return Failure("Предмет проверки используется в одном из приказов ГЖИ");
	        }

	        return Success();
        }
    }
}