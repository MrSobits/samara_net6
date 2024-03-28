namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class PrescriptionCancelViolReferenceService : IPrescriptionCancelViolReferenceService
    {
        public IWindsorContainer Container { get; set; }
        
        public IDataResult AddPrescriptionCancelViolReference(BaseParams baseParams)
        {
            try
            {
                var prescriptionCancelId = baseParams.Params.GetAs("prescriptionCancelId", 0L);
                var violIds = baseParams.Params.GetAs("violIds", new long[] { });

                var serviceParts = this.Container.Resolve<IDomainService<PrescriptionCancelViolReference>>();

                var listIds = serviceParts.GetAll()
                        .Where(x => x.PrescriptionCancel.Id == prescriptionCancelId)
                        .Select(x => x.InspectionViol.Id)
                        .Distinct()
                        .ToList();

                var notExist = violIds.Where(x => !listIds.Contains(x)).ToList();

                if (notExist.Any())
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var id in violIds)
                            {
                                if (listIds.Contains(id))
                                {
                                    continue;
                                }

                                var newObj = new PrescriptionCancelViolReference
                                {
                                    PrescriptionCancel = new PrescriptionCancel { Id = prescriptionCancelId },
                                    InspectionViol = new InspectionGjiViolStage { Id = id }
                                };

                                serviceParts.Save(newObj);
                            }

                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}
