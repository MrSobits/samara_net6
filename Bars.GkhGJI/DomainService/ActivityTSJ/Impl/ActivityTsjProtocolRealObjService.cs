namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ActivityTsjProtocolRealObjService : IActivityTsjProtocolRealObjService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            try
            {
                var protocolId = baseParams.Params.ContainsKey("protocol") ? baseParams.Params["protocol"].ToLong() : 0;

                if (protocolId > 0)
                {
                    var objectIds = baseParams.Params["objectIds"].ToString().Split(',');
                    var service = Container.Resolve<IDomainService<ActivityTsjProtocolRealObj>>();

                    // получаем у контроллера дома что бы не добавлять их повторно
                    var exsistingActivityTsjGjiProtocolRealObj = service.GetAll().Where(x => x.ActivityTsjProtocol.Id == protocolId).Select(x => x.RealityObject.Id).ToList();

                    foreach (var id in objectIds)
                    {
                        if (exsistingActivityTsjGjiProtocolRealObj.Contains(id.ToLong()))
                            continue;

                        var newId = id.ToLong();

                        var newActivityTsjGjiProtocolRealObj = new ActivityTsjProtocolRealObj
                        {
                            RealityObject = new RealityObject { Id = newId },
                            ActivityTsjProtocol = new ActivityTsjProtocol { Id = protocolId }
                        };

                        service.Save(newActivityTsjGjiProtocolRealObj);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}