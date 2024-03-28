namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.DomainService;

    using Castle.Core.Internal;

    using Microsoft.AspNetCore.Http;

    public class RealityObjectTpSyncInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public IRealityObjectTpSyncService Service { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /*public override IDataResult BeforeUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            return HttpContext.Current.Session["noSync"].ToBool() || HttpContext.Current.Request["skipSyncValidation"].ToBool() ? new BaseDataResult() : this.Service.Validate(entity);
        }*/

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            ///Добавил проверку HttpContext.Current == null, потому, что в тестах нет HttpContext.Current
            var noSync = HttpContextAccessor.HttpContext.Session?.GetString("noSync");
            
            if (!noSync.IsNullOrEmpty() && Convert.ToBoolean(noSync))
            {
                return new BaseDataResult();
            }
            
            return this.Service.Sync(entity);
        }
    }
}