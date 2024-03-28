namespace Bars.GkhGji.Regions.Nso.Interceptors
{
	using Bars.B4;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Interceptors;
	using Bars.GkhGji.Regions.Nso.Entities;
	using System.Linq;
	using Bars.B4.Utils;

	public class Protocol197ServiceInterceptor : DocumentGjiInterceptor<Protocol197>
	{
		public override IDataResult BeforeCreateAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
			var domainServiceInspection = Container.Resolve<IDomainService<BaseDefault>>();
            var domainStage = Container.Resolve<IDomainService<InspectionGjiStage>>();

            try
            {
				var newInspection = new BaseDefault()
                {
                    TypeBase = TypeBase.Protocol197
                };

                domainServiceInspection.Save(newInspection);

                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.Protocol197,
                    Position = 0
                };

                domainStage.Save(newStage);

				entity.TypeDocumentGji = TypeDocumentGji.Protocol197;
                entity.Inspection = newInspection;
                entity.Stage = newStage;

                return base.BeforeCreateAction(service, entity);
            }
            finally 
            {
                Container.Release(domainServiceInspection);
                Container.Release(domainStage);
            }
        }

		public override IDataResult BeforeDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
		{
			var annexService = Container.Resolve<IDomainService<Protocol197Annex>>();
			var lawService = Container.Resolve<IDomainService<Protocol197ArticleLaw>>();
			var violationService = Container.Resolve<IDomainService<Protocol197Violation>>();
			var activitiyDirectionService = Container.Resolve<IDomainService<Protocol197ActivityDirection>>();
			var surveySubjectReqService = Container.Resolve<IDomainService<Protocol197SurveySubjectRequirement>>();
			var longTextService = Container.Resolve<IDomainService<Protocol197LongText>>();

			try
			{
				var result = base.BeforeDeleteAction(service, entity);

				if (!result.Success)
				{
					return Failure(result.Message);
				}

				annexService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => annexService.Delete(x));

				lawService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => lawService.Delete(x));

				violationService.GetAll().Where(x => x.Document.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => violationService.Delete(x));

				activitiyDirectionService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => activitiyDirectionService.Delete(x));

				surveySubjectReqService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => surveySubjectReqService.Delete(x));

				longTextService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
					.Select(x => x.Id).ForEach(x => longTextService.Delete(x));

				return result;
			}
			finally
			{
				Container.Release(annexService);
				Container.Release(lawService);
				Container.Release(violationService);
				Container.Release(activitiyDirectionService);
				Container.Release(surveySubjectReqService);
			}
		}

		public override IDataResult AfterDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
		{
			return Success();
		}
	}
}