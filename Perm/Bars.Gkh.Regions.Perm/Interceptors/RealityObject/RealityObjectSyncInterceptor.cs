namespace Bars.Gkh.Regions.Perm.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    using Bars.Gkh.DomainService;

    /// <summary>
    /// Интерсептор синхронизации домов и аварийных домов (для Перми)
    /// </summary>
    public class RealityObjectSyncInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        public IEmergencyObjectSyncService EmergencyObjectSyncService { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            this.EmergencyObjectSyncService.SyncEmergencyObject(entity);
            return this.Success();
        }
    }
}
