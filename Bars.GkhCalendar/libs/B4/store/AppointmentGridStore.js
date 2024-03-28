Ext.define('B4.store.AppointmentGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppointmentGridModel'],
    autoLoad: false,
    storeId: 'appointmentGridStore',
    model: 'B4.model.AppointmentGridModel'
});