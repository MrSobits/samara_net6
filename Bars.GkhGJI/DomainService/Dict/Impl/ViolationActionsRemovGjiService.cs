using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.DomainService
{
    public class ViolationActionsRemovGjiService : IViolationActionsRemovGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddViolationActionsRemov(BaseParams baseParams)
        {
            try
            {
                var violationId = baseParams.Params.ContainsKey("violationId") ? baseParams.Params["violationId"].ToLong() : 0;
                var actRemoveViolString = baseParams.Params.ContainsKey("actRemoveViolIds") ? baseParams.Params["actRemoveViolIds"].ToStr() : string.Empty;

                var listObjects = new List<long>();

                if (!string.IsNullOrEmpty(actRemoveViolString))
                {
                    var actRemoveViolIds = actRemoveViolString.Contains(',')
                      ? actRemoveViolString.Split(',')
                      : new[] { actRemoveViolString };

                    var serviceViolationActionsRemovGji = Container.Resolve<IDomainService<ViolationActionsRemovGji>>();

                    listObjects.AddRange(
                            serviceViolationActionsRemovGji.GetAll()
                            .Where(x => x.ViolationGji.Id == violationId)
                            .Select(x => x.ActionsRemovViol.Id)
                            .Distinct().ToList());

                    foreach (var id in actRemoveViolIds)
                    {
                        var newId = id.ToLong();

                        if (!listObjects.Contains(newId))
                        {
                            var newViolationActionsRemovGji = new ViolationActionsRemovGji
                            {
                                ActionsRemovViol = new ActionsRemovViol { Id = newId },
                                ViolationGji = new ViolationGji { Id = violationId }
                            };

                            serviceViolationActionsRemovGji.Save(newViolationActionsRemovGji);
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }
    }
}