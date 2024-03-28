namespace Bars.Gkh.Regions.Perm.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class EmergencyObjectInterceptor : EmptyDomainInterceptor<EmergencyObject>
    {
        public IEmergencyObjectSyncService EmergencyObjectSyncService { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<EmergencyObject> service, EmergencyObject entity)
        {
            this.EmergencyObjectSyncService.SyncRealityObject(entity);
            return this.Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<EmergencyObject> service, EmergencyObject entity)
        {
            this.EmergencyObjectSyncService.QuiteSyncEmergencyObject(entity);

            return this.Success();
        }
    }
}
