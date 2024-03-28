namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Web;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using Castle.Core.Internal;

    using Microsoft.AspNetCore.Http;

    public class TpRealityObjectSyncInterceptor : EmptyDomainInterceptor<TehPassportValue>
    {
        public IRealityObjectTpSyncService Service { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<TehPassportValue> service, TehPassportValue entity)
        {
            return this.Sync(entity);
        }

        private IDataResult Sync(TehPassportValue entity)
        {
            var noSync = HttpContextAccessor.HttpContext.Session?.GetString("noSync");
            
            if (!noSync.IsNullOrEmpty() && Convert.ToBoolean(noSync))
            {
                return new BaseDataResult();
            }
            
            return this.Service.Sync(entity);
        }
    }
}