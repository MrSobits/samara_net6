namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Entities;

    using Castle.Windsor;

    public class MonitoringSmrService : IMonitoringSmrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveByObjectCrId(BaseParams baseParams)
        {
            try
            {
                var objectCrId = baseParams.Params["objectCrId"].ToLong();
                var serviceObj = Container.Resolve<IDomainService<MonitoringSmr>>();
                var objCrDomain = Container.Resolve<IDomainService<Entities.ObjectCr>>();
                using (Container.Using(serviceObj, objCrDomain))
                {

                    var monitoringSmr = serviceObj.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCrId);

                    if (monitoringSmr == null)
                    {
                        var objCr = objCrDomain.FirstOrDefault(x => x.Id == objectCrId);
                        monitoringSmr = new MonitoringSmr { ObjectCr = objCr };
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

        public IDataResult GetByObjectCrId(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params["objectCrId"].ToInt();
            var serviceObj = Container.Resolve<IDomainService<MonitoringSmr>>();
            var monitoringSmr = serviceObj
               .GetAll()
               .FirstOrDefault(x => x.ObjectCr.Id == objectCrId);

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