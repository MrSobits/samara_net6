namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
	using System;
	using System.Collections.Generic;
	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Regions.Nso.Entities;
	using Castle.Windsor;

	using Microsoft.Extensions.Logging;

	public class Protocol197ViolationService : IProtocol197ViolationService
    {
        public IWindsorContainer Container { get; set; }

		public virtual IDataResult Save(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
				var serviceInspectionViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
				var serviceProtViol = Container.Resolve<IDomainService<Protocol197Violation>>();
				var serviceProt197 = Container.Resolve<IDomainService<Protocol197>>();
				var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
				var serviceViolation = Container.Resolve<IDomainService<ViolationGji>>();

	            try
	            {
		            var protocol197Id = baseParams.Params.GetAs<long>("protocol197Id");
		            var violations = baseParams.Params.GetAs<List<Protocol197ViolationProxy>>("violations");
		            var protocol197 = serviceProt197.Get(protocol197Id);

		            if (protocol197 != null && violations != null)
		            {
			            foreach (var viol in violations)
			            {
				            if (viol.Id == 0)
				            {
					            var newViol = new InspectionGjiViol
					            {
						            Inspection = serviceInspection.Load(protocol197.Inspection.Id),
						            Violation = serviceViolation.Load(viol.ViolationGjiId),
						            DatePlanRemoval = viol.DatePlanRemoval
					            };

					            serviceInspectionViol.Save(newViol);

					            var newObj = new Protocol197Violation
					            {
						            TypeViolationStage = TypeViolationStage.Detection,
						            InspectionViolation = newViol,
						            Document = protocol197,
						            DatePlanRemoval = viol.DatePlanRemoval
					            };

					            serviceProtViol.Save(newObj);
				            }
			            }

		            }

		            transaction.Commit();
		            return new BaseDataResult();
	            }
	            catch (ValidationException e)
	            {
		            transaction.Rollback();
		            return new BaseDataResult {Success = false, Message = e.Message};
	            }
	            catch (Exception e)
	            {
		            transaction.Rollback();
		            Container.Resolve<ILogger>().LogError(e, e.Message);
		            return new BaseDataResult {Success = false, Message = e.Message};
	            }
	            finally
	            {
		            Container.Release(serviceInspectionViol);
		            Container.Release(serviceProtViol);
		            Container.Release(serviceProt197);
		            Container.Release(serviceInspection);
		            Container.Release(serviceViolation);
	            }
            }						
        }							
									
		private class Protocol197ViolationProxy
		{
			public long Id { get; set; }
			public long ViolationGjiId { get; set; }
			public DateTime? DatePlanRemoval { get; set; }
		}
    }
}