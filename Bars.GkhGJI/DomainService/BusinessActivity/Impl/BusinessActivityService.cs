namespace Bars.GkhGji.DomainService
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BusinessActivityService : IBusinessActivityService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult CheckDateNotification(BaseParams baseParams)
        {
            var dateNotification = baseParams.Params.ContainsKey("dateNotification") ? baseParams.Params["dateNotification"].ToDateTime() : DateTime.MinValue;
            var businessActivityId = baseParams.Params.ContainsKey("businessActivityId") ? baseParams.Params["businessActivityId"].ToLong() : 0;

            var registered = false;
            var state = new State();

            if (businessActivityId > 0)
            {
                var businessActivity = Container.Resolve<IDomainService<BusinessActivity>>().Load(businessActivityId);

                if (businessActivity != null)
                {
                    registered = businessActivity.Registered;
                    state = businessActivity.State;
                }
            }

            if (dateNotification != DateTime.MinValue)
            {
                if (!registered && dateNotification < DateTime.Now.Date)
                {
                    return new BaseDataResult(new { success = false, registered = false, state });
                }
            }

            return new BaseDataResult(new {success = true, registered, state});
        }
    }
}