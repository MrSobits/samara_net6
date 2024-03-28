Ext.define('B4.store.AppointmentDiffDayGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppointmentDiffDayGridModel'],
    autoLoad: false,
    storeId: 'appointmentDiffDayGridStore',
    model: 'B4.model.AppointmentDiffDayGridModel'
});