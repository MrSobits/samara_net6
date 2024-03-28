namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class FinanceSourceWorkService : IFinanceSourceWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var financeSourceId = baseParams.Params["financeSourceId"].ToInt();

                if (!string.IsNullOrEmpty(baseParams.Params["objectIds"].ToStr()))
                {
                    var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');
                    var service = Container.Resolve<IDomainService<FinanceSourceWork>>();

                    // получаем у контроллера работы что бы не добавлять их повторно
                    var exsistingFinanceSourceWork = 
                        service.GetAll()
                            .Where(x => x.FinanceSource.Id == financeSourceId)
                            .Select(x => x.Work.Id)
                            .Distinct()
                            .ToList();

                    foreach (var id in objectIds.Select(x => x.ToInt()))
                    {
                        if (exsistingFinanceSourceWork.Contains(id.ToInt()))
                        {
                            continue;
                        }

                        var newFinanceSourceWork = new FinanceSourceWork
                        {
                            Work = new Work { Id = id },
                            FinanceSource = new FinanceSource { Id = financeSourceId }
                        };

                        service.Save(newFinanceSourceWork);
                    }
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

        public IDataResult ListWorksByFinSource(BaseParams baseParams)
        {
            try
            {
                var financeSourceId = baseParams.Params["financeSourceId"].ToInt();

                var ids = Container.Resolve<IDomainService<FinanceSourceWork>>()
                        .GetAll()
                        .Where(x => x.FinanceSource.Id == financeSourceId)
                        .ToList()
                        .Select(x => x.Work.Id.ToStr())
                        .Distinct();

                return new BaseDataResult
                {
                    Success = true,
                    Data = new { success = true, ids }
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
