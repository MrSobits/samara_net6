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

    public class SpecialMonitoringSmrService : ISpecialMonitoringSmrService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveByObjectCrId(BaseParams baseParams)
        {
            try
            {
                var objectCrId = baseParams.Params["objectCrId"].ToLong();
                var serviceObj = this.Container.Resolve<IDomainService<SpecialMonitoringSmr>>();
                var objCrDomain = this.Container.Resolve<IDomainService<Entities.SpecialObjectCr>>();
                using (this.Container.Using(serviceObj, objCrDomain))
                {

                    var monitoringSmr = serviceObj.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCrId);

                    if (monitoringSmr == null)
                    {
                        var objCr = objCrDomain.FirstOrDefault(x => x.Id == objectCrId);
                        monitoringSmr = new SpecialMonitoringSmr { ObjectCr = objCr };
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
            var serviceObj = this.Container.Resolve<IDomainService<SpecialMonitoringSmr>>();
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