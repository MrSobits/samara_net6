namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;

    using Entities;
    using Castle.Windsor;

    public class ControlDateService : IControlDateService
    {
        public IWindsorContainer Container { get; set; }


        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var programCrId = baseParams.Params.GetAs<long>("programCrId",0);

                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();

                var controlDateService = Container.Resolve<IDomainService<ControlDate>>();
                var programCrService = Container.Resolve<IDomainService<ProgramCr>>();
                var workService = Container.Resolve<IDomainService<Work>>();

                var exsistingControlDates =
                        controlDateService.GetAll()
                            .Where(x => x.ProgramCr.Id == programCrId)
                            .Select(x => x.Work.Id)
                            .ToList();

                foreach (var id in objectIds.Where(x => !exsistingControlDates.Contains(x)))
                {
                    var newControlDate = new ControlDate
                    {
                        ProgramCr = programCrService.Load(programCrId),
                        Work = workService.Load(id),
                    };

                    controlDateService.Save(newControlDate);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }

        public IDataResult AddStageWorks(BaseParams baseParams)
        {
            try
            {
                var controlDateId = baseParams.Params.GetAs<long>("controlDateId", 0);

                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();

                if (objectIds.Any())
                {
                    var controlDate = Container.Resolve<IDomainService<ControlDate>>().Load(controlDateId);

                    var service = Container.Resolve<IDomainService<ControlDateStageWork>>();
                    var stageWorkService = Container.Resolve<IDomainService<StageWorkCr>>();

                    var exsistingStageWorks = service.GetAll().Where(x => x.ControlDate.Id == controlDateId).Select(x => x.StageWork.Id).ToList();

                    foreach (var id in objectIds.Where(x => !exsistingStageWorks.Contains(x)))
                    {
                        var newStageWork = new ControlDateStageWork
                        {
                            ControlDate = controlDate,
                            StageWork = stageWorkService.Load(id),
                        };

                        service.Save(newStageWork);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}