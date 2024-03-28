namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class ProgramCrFinSourceService : IProgramCrFinSourceService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var programCrId = baseParams.Params["programCrId"].ToInt();

                if (!string.IsNullOrEmpty(baseParams.Params["objectIds"].ToStr()))
                {
                    var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');
                    var service = Container.Resolve<IDomainService<ProgramCrFinSource>>();

                    // получаем у контроллера источники что бы не добавлять их повторно
                    var exsisting = service.GetAll().Where(x => x.ProgramCr.Id == programCrId).Select(x => x.FinanceSource.Id).ToList();

                    foreach (var newId in objectIds.Where(id => !exsisting.Contains(id.ToInt())).Select(id => id.ToInt()))
                    {
                        var newProgramCrFinSource = new ProgramCrFinSource
                        {
                            FinanceSource = new FinanceSource { Id = newId },
                            ProgramCr = new ProgramCr { Id = programCrId }
                        };

                        service.Save(newProgramCrFinSource);
                    }
                }

                return new BaseDataResult
                {
                    Success = true
                }; 
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
    }
}
