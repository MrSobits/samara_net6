using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.RealityObj;
using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;
using System;
using System.Linq;

namespace Bars.Gkh.Interceptors
{
    class RealityObjectTechnicalMonitoringInterceptor : EmptyDomainInterceptor<RealityObjectTechnicalMonitoring>
    {
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectTechnicalMonitoring> service, RealityObjectTechnicalMonitoring entity)
        {
            var realObjService = Container.Resolve<IDomainService<RealityObject>>();
            try
            {
                if (entity.MonitoringTypeDict.Name == "Справка БТИ")
                {
                    var ro = realObjService.Get(entity.RealityObject.Id);
                    if (entity.PhysicalWear != null)
                    {
                        ro.PhysicalWear = entity.PhysicalWear;
                    }
                    if (entity.CapitalGroup != null)
                    {
                        ro.CapitalGroup = entity.CapitalGroup;
                    }
                    if (entity.File != null)
                    {
                        ro.FileInfo = entity.File;
                    }
                    realObjService.Update(ro);
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(realObjService);
            }
            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectTechnicalMonitoring> service, RealityObjectTechnicalMonitoring entity)
        {
            var realObjService = Container.Resolve<IDomainService<RealityObject>>();
            try
            {
                if (entity.MonitoringTypeDict.Name == "Справка БТИ")
                {
                    var ro = realObjService.Get(entity.RealityObject.Id);
                    if (entity.PhysicalWear != null)
                    {
                        ro.PhysicalWear = entity.PhysicalWear;
                    }
                    if (entity.CapitalGroup != null)
                    {
                        ro.CapitalGroup = entity.CapitalGroup;
                    }
                    if (entity.File != null)
                    {
                        ro.FileInfo = entity.File;
                    }
                    realObjService.Update(ro);
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(realObjService);
            }
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectTechnicalMonitoring> service, RealityObjectTechnicalMonitoring entity)
        {
            var realObjService = Container.Resolve<IDomainService<RealityObject>>();
            try
            {
                if (entity.MonitoringTypeDict.Name == "Справка БТИ")
                {
                    var ro = realObjService.Get(entity.RealityObject.Id);
                    if (entity.File == ro.FileInfo)
                    {
                        ro.FileInfo = null;
                    }
                    realObjService.Update(ro);
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(realObjService);
            }
            return Success();
        }
    }
}
