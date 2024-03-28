Ext.define('B4.store.AppointmentTimeGridStore', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppointmentTimeGridModel'],
    autoLoad: false,
    storeId: 'appointmentTimeGridStore',
    model: 'B4.model.AppointmentTimeGridModel'
});