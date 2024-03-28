Ext.define('B4.store.taskcalendar.ListPrescription', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Prescription'],
    autoLoad: false,
    model: 'B4.model.Prescription',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TaskCalendar',
        listAction: 'GetListPrescriptionsGji'
    }
});