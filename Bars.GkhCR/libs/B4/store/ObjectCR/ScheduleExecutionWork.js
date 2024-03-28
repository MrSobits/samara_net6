Ext.define('B4.store.objectcr.ScheduleExecutionWork', {
    extend: 'B4.base.Store',
    requires: ['B4.model.objectcr.TypeWorkCr'],
    autoLoad: false,
    storeId: 'scheduleExecutionWork',
    model: 'B4.model.objectcr.TypeWorkCr'
});