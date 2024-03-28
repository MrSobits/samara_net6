Ext.define('B4.model.AppointmentGridModel', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppointmentQueue'
    },
    fields: [
        { name: 'TypeOrganisation'},
        { name: 'RecordTo'},
        { name: 'Description' },
        { name: 'TimeSlot' }
    ]
});