Ext.define('B4.store.repairobject.ScheduleExecutionWork', {
    extend: 'B4.base.Store',
    requires: ['B4.model.repairobject.RepairWork'],
    autoLoad: false,
    storeId: 'scheduleExecutionRepairWork',
    model: 'B4.model.repairobject.RepairWork'
});