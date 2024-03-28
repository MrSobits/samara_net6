namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class ConstructObjMonitoringSmrService : IConstructObjMonitoringSmrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveByConstructObjectId(BaseParams baseParams)
        {
            try
            {
                var constructObjId = baseParams.Params["constructObjId"].ToLong();
                var serviceObj = this.Container.Resolve<IDomainService<ConstructObjMonitoringSmr>>();
                var constructObjDomain = this.Container.Resolve<IDomainService<ConstructionObject>>();
                using (this.Container.Using(serviceObj, constructObjDomain))
                {

                    var monitoringSmr = serviceObj.GetAll().FirstOrDefault(x => x.ConstructionObject.Id == constructObjId);

                    if (monitoringSmr == null)
                    {
                        var constructObj = constructObjDomain.FirstOrDefault(x => x.Id == constructObjId);
                        monitoringSmr = new ConstructObjMonitoringSmr { ConstructionObject = constructObj };
                        serviceObj.Save(monitoringSmr);
                    }

                    return new BaseDataResult
                    {
                        Data = new { MonitoringSmrId = monitoringSmr.Id },
                        Success = true
                    };
                }

            }
            catch (Exception exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }

        }

        public IDataResult GetByConstructObjectId(BaseParams baseParams)
        {
            var constructObjId = baseParams.Params["constructObjId"].ToInt();
            var serviceObj = this.Container.Resolve<IDomainService<ConstructObjMonitoringSmr>>();
            var monitoringSmr = serviceObj
               .GetAll()
               .FirstOrDefault(x => x.ConstructionObject.Id == constructObjId);

            if (monitoringSmr != null)
            {
                return new BaseDataResult { Data = new { MonitoringSmrId = monitoringSmr.Id }, Success = true };
            }

            return new BaseDataResult
            {
                Success = false,
                Message = "Ошибка"
            };
        }
    }
}
