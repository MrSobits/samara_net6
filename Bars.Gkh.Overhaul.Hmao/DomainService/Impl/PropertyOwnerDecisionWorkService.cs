namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    public class PropertyOwnerDecisionWorkService : IPropertyOwnerDecisionWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var propertyOwnerDecisionId = baseParams.Params["propertyOwnerDecisionId"].ToLong();

                if (!string.IsNullOrEmpty(baseParams.Params["objectIds"].ToStr()))
                {
                    var objectIds = baseParams.Params["objectIds"].ToStr().Split(',');
                    var service = Container.Resolve<IDomainService<PropertyOwnerDecisionWork>>();

                    // получаем у контроллера работы что бы не добавлять их повторно
                    var existingPropertyOwnerDecisionWorks =
                        service.GetAll()
                            .Where(x => x.Decision.Id == propertyOwnerDecisionId)
                            .Select(x => x.Work.Id)
                            .Distinct()
                            .ToList();

                    foreach (var id in objectIds.Select(x => x.ToLong()))
                    {
                        if (existingPropertyOwnerDecisionWorks.Contains(id))
                        {
                            continue;
                        }

                        var newFinanceSourceWork = new PropertyOwnerDecisionWork
                        {
                            Work = new Work { Id = id },
                            Decision = new SpecialAccountDecision { Id = propertyOwnerDecisionId }
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
    }
}